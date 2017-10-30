using System;
using System.Collections.Generic;
using System.Text;

namespace Engine {
    public class CitizenActivity {
        public string Activity {
            get; set;
        }
        public bool IsInterruptable {
            get; set;
        }
        public object Locker {
            get; set;
        }
        /// <summary>
        /// Create a new WorkerActivity
        /// </summary>
        /// <param name="act">The name of the activity. IE: "Sleeping"</param>
        /// <param name="interr">Can you interrupt the activity?</param>
        /// <param name="lk">What is locking the activity</param>
        public CitizenActivity(string act, bool interr, object lk = null) {
            Activity = act;
            IsInterruptable = interr;
            Locker = lk;
        }
    }
}
