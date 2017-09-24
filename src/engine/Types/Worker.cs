using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public class Worker {
        /// <summary>
        /// Either the charge or the mental energy of the worker.
        /// </summary>
        public Bank Energy {
            get; set;
        }
        public Location Position {
            get; set;
        }
        public WorkerCategory Category {
            get; set;
        }
        public string Name {
            get; set;
        }
        public Bank Health {
            get; set;
        }
        /// <summary>
        /// Null when doing nothing
        /// </summary>
        public WorkerActivity CurrentActivity {
            get; set;
        }
    }
}
