using System;
using System.Collections.Generic;
using System.Text;
using static Engine.LangHelpers;

namespace Engine.Types {
    public class Knowledge {
        public int Identifier {
            get; set;
        }
        public List<Knowledge> KnowledgeRequirements {
            get; set;
        }
        public List<Quantified<Resource>> ResourceRequirements {
            get; set;
        }
        public uint TotalWorkerCost {
            get; set;
        }
        // Per frame
        public uint WorkerCost {
            get; set;
        }
        // Per frame
        public uint StationCost {
            get; set;
        }
        public Skill Unlocks {
            get; set;
        }

        public Knowledge() => DoNothing( );

        public Knowledge(List<Knowledge> knReqs, List<Quantified<Resource>> resReqs, (uint total, uint frame) workerCost, uint stationCost, Skill unlocks) {
            Perform(knReqs != null && knReqs.Count > 0, () => KnowledgeRequirements = new List<Knowledge>(knReqs));
            Perform(resReqs != null && resReqs.Count > 0, () => ResourceRequirements = new List<Quantified<Resource>>(resReqs));
            WorkerCost = workerCost.frame;
            StationCost = stationCost;
            Unlocks = unlocks;
        }
        public Knowledge(Knowledge kn)
            : this(kn.KnowledgeRequirements, kn.ResourceRequirements, (kn.TotalWorkerCost, kn.WorkerCost), kn.StationCost, kn.Unlocks) => DoNothing( );
    }
}
