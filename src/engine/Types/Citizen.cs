using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public class Citizen {
        /// <summary>
        /// Either the charge or the mental energy of the worker.
        /// </summary>
        public Bank Energy {
            get; set;
        }
        public Location Position {
            get; set;
        }
        public CitizenCategory Category {
            get; set;
        }
        public string Name {
            get; set;
        }
        public Bank Health {
            get; set;
        }
        /// <summary>
        /// A user-friendly indicator of the task currently being performed, as well as a locking indicator for what is holding the task.
        /// </summary>
        public CitizenActivity CurrentActivity {
            get; set;
        }

        public bool NeedsRest => Energy.Maximum * 0.08 > Energy.Quantity;
        public bool IsRested => Energy.Maximum * 0.45 < Energy.Quantity;
    }
}
