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
        /// <summary>
        /// Where is the Citizen in the station
        /// </summary>
        public Location Position {
            get; set;
        }
        /// <summary>
        /// The category of the citizen (bot, human, etc)
        /// </summary>
        public CitizenCategory Category {
            get; set;
        }
        /// <summary>
        /// The name of the citizen
        /// </summary>
        public string Name {
            get; set;
        }
        /// <summary>
        /// How close to death is the citizen
        /// </summary>
        public Bank Health {
            get; set;
        }
        /// <summary>
        /// What the citizen is capable of, and what they are trained on
        /// </summary>
        public List<Skill> Skills {
            get; set;
        } = new List<Skill>( );
        /// <summary>
        /// A user-friendly indicator of the task currently being performed, as well as a locking indicator for what is holding the task.
        /// </summary>
        public CitizenActivity CurrentActivity {
            get; set;
        }
        /// <summary>
        /// Is the energy level of the citizen under 8%
        /// </summary>
        public bool NeedsRest => Energy.Maximum * 0.08 > Energy.Quantity;
        /// <summary>
        /// Is the energy level of the citizen above 45%
        /// </summary>
        public bool IsRested => Energy.Maximum * 0.45 < Energy.Quantity;

        public bool HasEnoughEnergy(uint amt) => Energy.HasEnoughFor(amt) && !NeedsRest;

    }
}
