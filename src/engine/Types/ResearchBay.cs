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
        public QuantifiedBank<Knowledge> Active {
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
        } = 1;

        public List<Knowledge> KnowledgeRepo {
            get; private set;
        }
        public List<Knowledge> SupportedResearches {
            get; private set;
        }
        public ResourceBank Resources {
            get; private set;
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
            SupportedResearches = Supported;
            Resources = new ResourceBank(cargoSize);
        }

        public void Think() => throw new NotImplementedException( );

        /// <summary>
        /// Start the research on a piece of knowledge
        /// </summary>
        /// <param name="know">The knowledge to research</param>
        public void Research(Knowledge know) => throw new NotImplementedException( );

        public void Cancel() => throw new NotImplementedException( );

        public void AddResearcher(Citizen wk, Action onLimitReached = null) =>
            Perform(Researchers.Count >= ResearcherLimit, () => DoOrThrow(onLimitReached, new LimitMetException( )), () => Researchers.Add(wk));

        public void RemoveResearcher(Citizen wk) => Perform(Researchers.Contains(wk), () => Researchers.Remove(wk));
        public void RemoveResearcher(int index) => Perform(Researchers.Count <= index + 1 && index > 0, () => Researchers.RemoveAt(index));


        public bool IsQualified(Citizen wk) => KnowledgeRepo.TrueForAll((kw) => wk.Skills.Contains(kw.Unlocks));
        public bool HasResearcher(Citizen wk) => Researchers.Contains(wk);
        public bool IsResearched(Knowledge knw) => KnowledgeRepo.Contains(knw);
        public bool CanResearch(Knowledge knw) => SupportedResearches.Contains(knw);

    }
}
