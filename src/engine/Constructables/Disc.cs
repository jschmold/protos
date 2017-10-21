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
    public class Disc : IThinkable {
        private (Bound3d Bound, Type Kind, Bay Obj)[] Spaces {
            get; set;
        } 

        public List<IPowerSource> EnergyGrid {
            get; set;
        }

        private List<int> ActiveConstructions {
            get; set;
        }

        public IEnumerable<Bound3d> AvailableSpaces {
            get {
                foreach ((Bound3d Bound, Type Kind, Bay obj) in Spaces) {
                    if (obj == null) {
                        yield return Bound;
                    }
                }
            }
        }

        public IEnumerable<(Bound3d Bound, Type Kind, Bay Obj)> Map => Spaces;

        public void Construct(Recipe<Resource, Bay> recipe, Bound3d location, uint occLimit) {
            
        }

        public void Think() => throw new NotImplementedException( );


        public IEnumerable<T> GetBays<T>() where T : Bay {
            foreach ((Bound3d Bound, Type Kind, Bay Obj) in Spaces) {
                if (typeof(T) == Kind) {
                    yield return Obj as T;
                }
            }
        }

        public (Type Kind, Bay Obj) AtBound(Bound3d bound) {
            foreach ((Bound3d Bound, Type Kind, Bay Obj) in Spaces) {
                if (bound == Bound) {
                    return (Kind, Obj);
                }
            }
            throw new KeyNotFoundException(nameof(bound));
        }

    }
}