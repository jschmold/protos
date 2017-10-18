using System;
using System.Collections.Generic;
using Engine.Exceptions;
using Engine.Types;
using static Engine.Helpers.Lang;
using Engine.Helpers;
using Engine.Interfaces;

namespace Engine.Constructables {
    public class ResearchBay : Bay, IPowerable {
        public QuantifiedBank<Knowledge> Active {
            get; private set; 
        }
        public CappedList<Citizen> Researchers {
            get; private set;
        }
        public List<Knowledge> KnowledgeRepo {
            get; private set;
        }
        public List<Knowledge> SupportedResearches {
            get; private set;
        }
        public ResourceBank Resources {
            get; private set;
        }
        private List<Citizen> Recovering {
            get; set;
        }
        public List<IPowerSource> EnergySources {
            get;
            set;
        }

        public ResearchBay(uint occLimit, uint researcherLim, List<Knowledge> Supported, uint cargoSize, List<IPowerSource> energySources, uint maxEnergyDraw) : base(occLimit) {
            SupportedResearches = new List<Knowledge>(Supported);
            Resources = new ResourceBank(cargoSize);
            Recovering = new List<Citizen>( );
            Researchers = new CappedList<Citizen>(researcherLim);
            KnowledgeRepo = new List<Knowledge>();
            EnergySources = energySources;
            EnergyMaxDraw = maxEnergyDraw;
        }

        /// <summary>
        /// The think cycle
        /// </summary>
        // Todo: Figure out what to do if there's not enough energy
        public override void Think() => Compose(
            FinalizeResearchIfComplete,
            () => ExpendEnergyFromActive(DoNothing),
            ProcessResearchers);

        /// <summary>
        /// If they need recovering, recover them until they are "rested". 
        /// If they are done recovering, put them to work.
        /// Expend energy and progress the research as needed
        /// </summary>
        public void ProcessResearchers() => Researchers.ForEach(wk => Compose(wk,
            DoRecovery,
            cit => Perform(Active != null && IsQualified(cit), () => ConvertResearcherEnergy(cit))
        ));
            

        /// <summary>
        /// Converts researcher energy into progress on Active
        /// </summary>
        /// <param name="wk">The worker to get the energy from</param>
        public void ConvertResearcherEnergy(Citizen wk) {
            wk.Energy.Quantity -= Active.Currency.WorkerCost;
            Active.Quantity += Active.Currency.WorkerCost;
        }

        /// <summary>
        /// Recover workers needing rest, and put workers to work that are rested
        /// </summary>
        /// <param name="wk"></param>
        public void DoRecovery(Citizen wk) => Compose(wk, RemoveFromRecoveryIfRested, AddToRecoveryIfNeedsRest);
        /// <summary>
        /// Remove a rested worker from the Recovery
        /// </summary>
        /// <param name="wk"></param>
        public void RemoveFromRecoveryIfRested(Citizen wk) => Perform(wk.IsRested, () => Recovering.Remove(wk));

        /// <summary>
        /// Add an exhausted worker to the Recovery
        /// </summary>
        /// <param name="wk"></param>
        public void AddToRecoveryIfNeedsRest(Citizen wk) => Perform(wk.NeedsRest, () => Recovering.Add(wk));

        /// <summary>
        /// Start the research on a piece of knowledge
        /// </summary>
        /// <param name="know">The knowledge to research</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Research(Knowledge know, Action onActiveIsNotNull = null, Action onUnsupportedResearch = null, Action onInsufficientResources = null) => Perform(
            Active == null && CanResearch(know),
            () => {
                DoOrThrow(Active != null, onActiveIsNotNull, new InvalidOperationException("Active not null"));
                DoOrThrow(!CanResearch(know), onUnsupportedResearch, new UnsupportedException( ));
            },
            () => {
                if(know.ResourceRequirements != null && !Resources.ContainsAll(know.ResourceRequirements)) {
                    DoOrThrow(onInsufficientResources, new LackingResourceException());
                    return;
                }
                Active = new QuantifiedBank<Knowledge> {
                    Currency = know,
                    Maximum = know.TotalWorkerCost,
                    Quantity = 0
                };
                know.ResourceRequirements?.ForEach(qr => Resources -= qr);
            });

        /// <summary>
        /// Cancel the current research
        /// </summary>
        public void Cancel() => Perform(Active != null, ClearActive);

        /// <summary>
        /// Add a researcher to the bay for work
        /// </summary>
        /// <param name="wk">The worker to put to work</param>
        /// <param name="onLimitReached">What to do instead of throwing LimitMetException</param>
        /// <exception cref="LimitMetException"></exception>
        public void AddResearcher(Citizen wk, Action onLimitReached = null) => Perform(Researchers.Count < Researchers.Limit,
            (onLimitReached, new LimitMetException( )), () => Researchers.Add(wk));

        /// <summary>
        /// Remove a researcher
        /// </summary>
        /// <param name="wk"></param>
        public void RemoveResearcher(Citizen wk) => Researchers.Remove(wk);

        /// <summary>
        /// Remove the researcher at <paramref name="index"/>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveResearcher(int index) => Perform(Researchers.Count <= index + 1 && index > 0, 
            new IndexOutOfRangeException(), () => Researchers.RemoveAt(index));

        /// <summary>
        /// Finalize the active research if it has been completed
        /// </summary>
        public void FinalizeResearchIfComplete() => Perform(Active?.IsFull ?? false, FinalizeResearch);

        /// <summary>
        /// Finalize the research.
        /// </summary>
        public void FinalizeResearch() {
            KnowledgeRepo.Add(Active.Currency);
            Researchers.ForEach(wk => Perform(IsQualified(wk), () => wk.Skills.Add(Active.Currency.Unlocks)));
            ClearActive();
        }

        /// <summary>
        /// Clear what is being actively worked on
        /// </summary>
        public void ClearActive() => Active = null;

        /// <summary>
        /// Expect the station cost on the bay from Active, starting with the Pool, then the reserve.
        /// </summary>
        /// <param name="onNotEnoughEnergy">What to do if there's not enough energy</param>
        /// <exception cref="NotEnoughEnergyException"></exception>
        public void ExpendEnergyFromActive(Action onNotEnoughEnergy = null) => DrawEnergy(Active?.Currency.StationCost ?? 0, onNotEnoughEnergy);

        /// <summary>
        /// Whether or not a worker is able to perform the research
        /// </summary>
        /// <param name="wk">The worker to check</param>
        /// <returns></returns>
        public bool IsQualified(Citizen wk) => Active?.Currency.KnowledgeRequirements?.TrueForAll(kw => wk.Skills.Contains(kw.Unlocks)) ?? true;

        /// <summary>
        /// Whether or not the researcher is ready for work in the bay
        /// </summary>
        /// <param name="wk">The worker to check</param>
        /// <returns></returns>
        public bool HasResearcher(Citizen wk) => Researchers.Contains(wk);

        /// <summary>
        /// Whether or not something has been researched and is stored in the knowledge repository
        /// </summary>
        /// <param name="kw">The piece of knowledge to check</param>
        /// <returns></returns>
        public bool IsResearched(Knowledge kw) {
            if (KnowledgeRepo.Contains(kw)) {
                return true;
            }
            foreach (var obj in KnowledgeRepo) {
                if (obj == kw || obj.Identifier == kw.Identifier) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not the piece of knowledge is able to be researched here
        /// </summary>
        /// <param name="kw">The piece of knowledge to check</param>
        /// <returns></returns>
        public bool CanResearch(Knowledge kw) => SupportedResearches.Contains(kw);
        public uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null) => IPowerableUtils.Draw(amt, energySource, EnergySwitch, onNotEnoughEnergy);

        public uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null) => IPowerableUtils.DrawFromManySources(amt, EnergySources);
    }
}
