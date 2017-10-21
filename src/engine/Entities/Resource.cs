using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;

namespace Engine.Entities {
    public struct Resource {
        public string Name {
            get; set;
        }
        public uint Volume {
            get; set;
        }
        public uint Mass {
            get; set;
        }
        public uint Identifier {
            get; set;
        }

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

        public override int GetHashCode() {
            var hashCode = 53095276;
            hashCode = hashCode * -1521134295 + base.GetHashCode( );
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Volume.GetHashCode( );
            hashCode = hashCode * -1521134295 + Mass.GetHashCode( );
            hashCode = hashCode * -1521134295 + Identifier.GetHashCode( );
            return hashCode;
        }

        public static bool operator ==(Resource a, Resource b) =>
            a.Name == b.Name &&
            a.Volume == b.Volume &&
            a.Mass == b.Mass &&
            a.Identifier == b.Identifier;
        public static bool operator !=(Resource a, Resource b) => !(a == b);
    }
}
