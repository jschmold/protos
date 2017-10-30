using System;
using System.Collections.Generic;
using System.Text;
using Engine;

namespace Engine.Entities {
    /// <summary>
    /// Something that a citizen can equip
    /// </summary>
    public class Equippable {
        /// <summary>
        /// The name of the equippable
        /// </summary>
        public string Name {
            get; set;
        }
        /// <summary>
        /// The description of an equippable
        /// </summary>
        public string Description {
            get; set;
        }
        /// <summary>
        /// The categories the equippable is applicable to
        /// </summary>
        public EquippableCategory[] Category {
            get; set;
        }
        /// <summary>
        /// The mass of the equippable
        /// </summary>
        public uint Mass {
            get; set;
        }
    }
}
