using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public struct EquippableCategory {
        public string Name {
            get; set;
        }
        public string Description {
            get; set;
        }

        public override bool Equals(object obj) {
            if (!(obj is EquippableCategory)) {
                return false;
            }

            var category = (EquippableCategory)obj;
            return Name == category.Name &&
                   Description == category.Description;
        }

        public override int GetHashCode() {
            var hashCode = -2030050305;
            hashCode = hashCode * -1521134295 + base.GetHashCode( );
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }

        public static bool operator ==(EquippableCategory a, EquippableCategory b) => a.Name == b.Name && a.Description == b.Description;
        public static bool operator !=(EquippableCategory a, EquippableCategory b) => !(a == b);
    }
}
