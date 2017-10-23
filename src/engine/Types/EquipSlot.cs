using System;
using System.Collections.Generic;
using System.Text;
using Engine.Entities;

namespace Engine.Types {
    public class EquipSlot {
        public EquippableCategory Category {
            get; set;
        }
        public Equippable Equipped {
            get; set;
        }
    }
}
