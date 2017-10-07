using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Utilities.LangHelpers;

namespace Engine.Types {
    public class Knowledge {
        public Bank Progress {
            get; set;
        }
        public List<Knowledge> KnowledgeRequirements {
            get; set;
        }
        public List<Quantified<Resource>> ResourceRequirements {
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
        public bool Completed => Progress.IsFull;

        public Knowledge() => DoNothing( );

        public Knowledge(List<Knowledge> knReqs, List<Quantified<Resource>> resReqs, (uint total, uint frame) workerCost, uint stationCost, Skill unlocks) {
            KnowledgeRequirements = new List<Knowledge>(knReqs);
            ResourceRequirements = new List<Quantified<Resource>>(resReqs);
            WorkerCost = workerCost.frame;
            StationCost = stationCost;
            Progress = new Bank {
                Quantity = 0,
                Maximum = workerCost.total
            };
        }
        public Knowledge(Knowledge kn) 
            : this(kn.KnowledgeRequirements, kn.ResourceRequirements, (kn.Progress.Maximum, kn.WorkerCost), kn.StationCost, kn.Unlocks) => DoNothing( );
    }
}
