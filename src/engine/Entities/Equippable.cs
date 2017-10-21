using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;

namespace Engine.Entities {
    public class Equippable {
        public string Name {
            get; set;
        }
        public string Description {
            get; set;
        }
        public EquippableCategory[] Category {
            get; set;
        }
        public uint Mass {
            get; set;
        }
    }
}
