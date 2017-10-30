using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Engine.Math;
using static System.Math;
using static LangRoids;

using Engine;
using Engine.Exceptions;
using Engine.Entities;
/*
 * Construction: 
 *      Consumption of resources using recipe
 *      Utilizing workers to produce the bay
 *      Expending worker energy towards progress of bay
 */
namespace Engine.Constructables {

    /// <summary>
    /// A disc containing bays in the colony.
    /// </summary>
    public class Disc : IThinkable {
        struct ConstructionZone {
            public Bound3d Location => Blueprint.Location;

            public Blueprint<Bay> Blueprint {
                get; set;
            }

            public List<Citizen> Workers {
                get; set;
            }

            public ConstructionZone(Blueprint<Bay> bay, List<Citizen> workers = null) {
                Blueprint = bay;
                Workers = workers == null ? new List<Citizen>( ) : new List<Citizen>(workers);
            }
            
        }
        /// <summary>
        /// The Disc's boundaries. Implicitly describes the dimensions
        /// </summary>
        public Bound3d Topology {
            get; internal set;
        }

        /// <summary>
        /// The structures contained within the disc
        /// </summary>
        public List<Bay> Structures {
            get; internal set;
        }

        /// <summary>
        /// A collection of regions that are occupied
        /// </summary>
        public IEnumerable<Bound3d> Occupied => Structures.Select(str => str.Location);

        /// <summary>
        /// A collection of regions that are under construction
        /// </summary>
        public IEnumerable<Bound3d> OccupiedByConstruction => Construction.Select(bp => bp.Location);


        /// <summary>
        /// A c
        /// </summary>
        private List<ConstructionZone> Construction {
            get; set;
        }

        /// <summary>
        /// Build a bay at a specified location
        /// </summary>
        /// <param name="blueprint">The blueprint to reference and build</param>
        /// <param name="onSpaceOccupied"></param>
        /// <param name="onOutOfBounds"></param>
        public void Construct<T>(Blueprint<Bay> blueprint, Action onSpaceOccupied = null, Action onOutOfBounds = null) where T : Bay => Perform(
            (!Collisions.IsOverlapping(blueprint.Location, Occupied.ToArray( )), onSpaceOccupied, new Exception( )),
            (Collisions.IsContained(blueprint.Location, Topology), onOutOfBounds, new Exception( )),
            () => {
                Construction.Add(new ConstructionZone(blueprint));
            });



        public void Think() {
            ForEach(Structures, bay => bay.Think( ));
        }



    }
}