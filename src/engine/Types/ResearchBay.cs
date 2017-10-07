using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Utilities.LangHelpers;
using Engine.Exceptions;
using Engine.Utilities;


namespace Engine.Types {
    /*
     *  What do I want this to do?
     *  
     *  Research 1 thing at a time
     *  Contain a limited amount of research
     *  Only research things that are supported
     *  Contain a research repository of knowledge
     *  Indicate whether or not something is researched
     *  Consume resources on research
     *  Consume energy on research from both station and workers
     *  Let workers rest if nothing is active
     */
    public class ResearchBay : Bay {
        public Knowledge Active {
            get; private set;
        }
        public RegeneratingBank EnergyPool {
            get; private set;
        }
        public RegeneratingBank EnergyReserve {
            get; private set;
        }
        public List<Citizen> Researchers {
            get; private set;
        }
        public uint ResearcherLimit {
            get; set;
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

        public ResearchBay(Location loc, uint occLimit, (uint limit, uint start) pool, (uint limit, uint start) reserve, uint researcherLim, List<Knowledge> Supported, uint cargoSize) : base(loc, occLimit) {
            EnergyPool = new RegeneratingBank {
                Maximum = pool.limit,
                Quantity = pool.start
            };
            EnergyReserve = new RegeneratingBank {
                Maximum = reserve.limit,
                Quantity = reserve.start
            };
            ResearcherLimit = researcherLim;
            SupportedResearches = new List<Knowledge>(Supported);
            Resources = new ResourceBank(cargoSize);
            Recovering = new List<Citizen>( );
        }

        public void Think() => Compose(FinalizeResearchIfComplete, ProcessResearchers);

        /// <summary>
        /// If they need recovering, recover them until they are "rested". 
        /// If they are done recovering, put them to work.
        /// Expend energy and progress the research as needed
        /// </summary>
        public void ProcessResearchers() => Researchers.ForEach(wk => Compose(IsQualified(wk), wk, 
            _ => FinalizeResearchIfComplete(),
            DoRecovery, 
            ConvertResearcherEnergy
            ));

        /// <summary>
        /// Converts researcher energy into progress on Active
        /// </summary>
        /// <param name="wk">The worker to get the energy from</param>
        public void ConvertResearcherEnergy(Citizen wk) {
            wk.Energy.Quantity -= Active.WorkerCost;
            Active.Progress.Quantity += Active.WorkerCost;
        }

        public void DoRecovery(Citizen wk) => Compose(wk, RemoveFromRecoveryIfRested, AddToRecoveryIfNeedsRest);
        public void RemoveFromRecoveryIfRested(Citizen wk) => Perform(wk.IsRested, () => Recovering.Remove(wk));
        public void AddToRecoveryIfNeedsRest(Citizen wk) => Perform(wk.NeedsRest, () => Recovering.Add(wk));

        /// <summary>
        /// Start the research on a piece of knowledge
        /// </summary>
        /// <param name="know">The knowledge to research</param>
        public void Research(Knowledge know, Action onActiveNotNull = null) => Perform(Active != null, () => {
            DoOrThrow(onActiveNotNull, new InvalidOperationException("Active not null"));
            Active = new Knowledge(know);
        });

        public void Cancel() => Perform(Active != null, ClearActive);

        public void AddResearcher(Citizen wk, Action onLimitReached = null) => Perform(Researchers.Count >= ResearcherLimit, 
            // The failure condition
            () => DoOrThrow(onLimitReached, new LimitMetException( )),
            // The passing condition
            () => Researchers.Add(wk)
        );

        public void RemoveResearcher(Citizen wk) => Perform(Researchers.Contains(wk), () => Researchers.Remove(wk));
        public void RemoveResearcher(int index) => Perform(Researchers.Count <= index + 1 && index > 0, () => Researchers.RemoveAt(index));

        public void FinalizeResearchIfComplete() => Perform(Active.Progress.IsFull, FinalizeResearch);

        public void FinalizeResearch() => Compose(() => KnowledgeRepo.Add(Active), ClearActive);

        public void ClearActive() => Active = null;

        public bool HasEnoughEnergy(Citizen wk) => Active != null && wk.Energy.HasEnoughFor(Active.WorkerCost) && !wk.NeedsRest;
        public bool IsQualified(Citizen wk) => KnowledgeRepo.TrueForAll((kw) => wk.Skills.Contains(kw.Unlocks));
        public bool HasResearcher(Citizen wk) => Researchers.Contains(wk);
        public bool IsResearched(Knowledge knw) => KnowledgeRepo.Contains(knw);
        public bool CanResearch(Knowledge knw) => SupportedResearches.Contains(knw);
    }
}
