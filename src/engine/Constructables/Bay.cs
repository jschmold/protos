using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Exceptions;
using Engine.Types;
using static LangRoids;
using Engine;
using Engine.Interfaces;
using Engine.Entities;

namespace Engine.Constructables {
    public abstract class Bay : IThinkable {
        /// <summary>
        /// The occupants in the bay
        /// </summary>
        public CappedList<Citizen> Occupants {
            get; private set;
        }
        /// <summary>
        /// Whether or not the bay is permitted to draw energy from one of the energy sources.
        /// </summary>
        public bool EnergySwitch {
            get; set;
        }
        /// <summary>
        /// The maximum amount the bay is permitted to pull from the sources at any given time.
        /// </summary>
        public uint EnergyMaxDraw {
            get; set;
        }

        public abstract void Think();

        /// <summary>
        /// Create a new bay.
        /// </summary>
        /// <param name="loc">Where the bay is located</param>
        /// <param name="occLimit">The maximum occupant limit for the bay</param>
        public Bay(uint occLimit) => Occupants = new CappedList<Citizen>(occLimit);

        /// <summary>
        /// Add a single occupant to the bay
        /// </summary>
        /// <param name="work">The occupant to add</param>
        /// <param name="onFailure">The optional thing to do if it is not possible to add the occupant</param>
        public void AddOccupant(Citizen work, Action onFailure = null) => Perform(Occupants.CanHold(1),
            (onFailure, new PopulationExceedsMaximumException( )), () => Occupants.Add(work));

        /// <summary>
        /// Add a bunch of workers
        /// </summary>
        /// <param name="workers">The collection of workers to add</param>
        /// <param name="onFailure">What to do if it is not possible to add all of the workers</param>
        public void AddOccupantRange(IEnumerable<Citizen> workers, Action onFailure = null) => Perform(Occupants.CanHold(Occupants.Count + workers.Count( )),
            (onFailure, new PopulationExceedsMaximumException( )), () => Occupants.AddRange(workers));

        /// <summary>
        /// Remove a single occupant from the bay
        /// </summary>
        /// <param name="work">The occupant to remove</param>
        public void RemoveOccupant(Citizen work) => Perform(Occupants.Contains(work), () => Occupants.Remove(work));

        /// <summary>
        /// Remove a collection of occupants from the bay
        /// </summary>
        /// <param name="workers">The collection to remove</param>
        public void RemoveOccupant(IEnumerable<Citizen> workers) => ForEach(workers, RemoveOccupant);

    }
}
