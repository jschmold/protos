using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Utilities.LangHelpers;

namespace Engine.Types {
    // Todo: Finish Research class 
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
    }
}
