using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using static System.Math;
using static LangRoids;
using Engine.Interfaces;
using Engine;
using Engine.Exceptions;
using Engine.Types;
using Engine.Entities;
/*
 * Construction: 
 *      Consumption of resources using recipe
 *      Utilizing workers to produce the bay
 *      Expending worker energy towards progress of bay
 */
namespace Engine.Constructables {

    struct 

    /// <summary>
    /// A disc containing bays in the colony.
    /// </summary>
    public class Disc : IThinkable {
        /// <summary>
        /// The Disc's boundaries. Implicitly describes the dimensions
        /// </summary>
        public Bound3d Topology {
            get; private set;
        }

        /// <summary>
        /// The structures contained within the disc
        /// </summary>
        public List<Bay> Structures {
            get; private set;
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
        /// List of all the bays being actively constructed
        /// </summary>
        private List<Blueprint<Bay>> Construction {
            get; set;
        }

        

        /// <summary>
        /// Build a bay at a specified location
        /// </summary>
        /// <param name="blueprint">The blueprint to reference and build</param>
        /// <param name="onSpaceOccupied"></param>
        /// <param name="onOutOfBounds"></param>
        public void Construct<T>(Blueprint<T> blueprint, Action onSpaceOccupied = null, Action onOutOfBounds = null) where T : Bay => Perform(
            (!EngineMath.IsOverlapping(blueprint.Location, Occupied.ToArray( )), onSpaceOccupied, new Exception( )),
            (EngineMath.IsContained(blueprint.Location, Topology), onOutOfBounds, new Exception( )),
            () => {
                Structures.Add(blueprint.Produces.Contents);
                Construction.Add(new Blueprint<Bay>(blueprint as Blueprint<Bay>));
            });

        public void Think() {
            ForEach(Structures, bay => bay.Think( ));
            
        }


    }
}