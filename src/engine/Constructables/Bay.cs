using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Exceptions;
using Engine.Types;
using static Engine.LangHelpers;

namespace Engine.Constructables {
    public class Bay {
        /// <summary>
        /// Where is the bay?
        /// </summary>
        public Location Location {
            get; private set;
        }
        /// <summary>
        /// The occupants in the bay
        /// </summary>
        public List<Citizen> Occupants {
            get; private set;
        }
        /// <summary>
        /// The maximum amount of occupants allowed in the bay
        /// </summary>
        public uint OccupantLimit {
            get; private set;
        }
        /// <summary>
        /// Create a new bay.
        /// </summary>
        /// <param name="loc">Where the bay is located</param>
        /// <param name="occLimit">The maximum occupant limit for the bay</param>
        public Bay(Location loc, uint occLimit) {
            Location = loc;
            OccupantLimit = occLimit;
            Occupants = new List<Citizen>( );
        }

        /// <summary>
        /// Add a single occupant to the bay
        /// </summary>
        /// <param name="work">The occupant to add</param>
        /// <param name="onFailure">The optional thing to do if it is not possible to add the occupant</param>
        public void AddOccupant(Citizen work, Action onFailure = null) => Perform(Occupants.Count + 1 <= OccupantLimit,
            (onFailure, new PopulationExceedsMaximumException( )), () => Occupants.Add(work));

        /// <summary>
        /// Add a bunch of workers
        /// </summary>
        /// <param name="workers">The collection of workers to add</param>
        /// <param name="onFailure">What to do if it is not possible to add all of the workers</param>
        public void AddOccupant(IEnumerable<Citizen> workers, Action onFailure = null) => Perform(Occupants.Count + workers.Count( ) <= OccupantLimit,
            () => DoOrThrow(onFailure, new PopulationExceedsMaximumException( )),
            () => Occupants.AddRange(workers));

        /// <summary>
        /// Remove a single occupant from the bay
        /// </summary>
        /// <param name="work">The occupant to remove</param>
        public void RemoveOccupant(Citizen work) => Perform(Occupants.Contains(work), () => Occupants.Remove(work));

        /// <summary>
        /// Remove a collection of occupants from the bay
        /// </summary>
        /// <param name="workers">The collection to remove</param>
        public void RemoveOccupant(IEnumerable<Citizen> workers) {
            foreach (Citizen work in workers) {
                RemoveOccupant(work);
            }
        }

    }
}
