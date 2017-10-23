using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;

namespace Engine.Entities {
    /// <summary>
    /// A resource/material/object in the game that is not autonomous, and is not a bay
    /// </summary>
    public struct Resource {
        /// <summary>
        /// The name of the resource
        /// </summary>
        public string Name {
            get; set;
        }
        /// <summary>
        /// How much volume it takes up
        /// </summary>
        public uint Volume {
            get; set;
        }
        /// <summary>
        /// The mass of the Resource
        /// </summary>
        public uint Mass {
            get; set;
        }
        /// <summary>
        /// The identifier of the Resource.
        /// </summary>
        public uint Identifier {
            get; set;
        }

        /// <summary>
        /// Whether or not the object equals an instance of Resource
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is Resource)) {
                return false;
            }

            var resource = (Resource)obj;
            return Name == resource.Name &&
                   Volume == resource.Volume &&
                   Mass == resource.Mass &&
                   Identifier == resource.Identifier;
        }

        /// <summary>
        /// Obligatory overload
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var hashCode = 53095276;
            hashCode = hashCode * -1521134295 + base.GetHashCode( );
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Volume.GetHashCode( );
            hashCode = hashCode * -1521134295 + Mass.GetHashCode( );
            hashCode = hashCode * -1521134295 + Identifier.GetHashCode( );
            return hashCode;
        }

        /// <summary>
        /// Whether or not two resources are one and the same
        /// </summary>
        /// <param name="a">The first resource</param>
        /// <param name="b">The second resource</param>
        /// <returns>Whether or not they're the same</returns>
        public static bool operator ==(Resource a, Resource b) =>
            a.Name == b.Name &&
            a.Volume == b.Volume &&
            a.Mass == b.Mass &&
            a.Identifier == b.Identifier;
        /// <summary>
        /// Whether two resources are not the same
        /// </summary>
        /// <param name="a">First resource</param>
        /// <param name="b">Second resource</param>
        /// <returns>Whether or not they're not the same</returns>
        public static bool operator !=(Resource a, Resource b) => !(a == b);
    }
}
