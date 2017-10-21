using System;
using System.Collections.Generic;
using System.Text;
using Engine.Helpers;
using static LangRoids;
using Engine.Exceptions;
using Engine.Types;

namespace Engine.Entities {
    public class Citizen {
        public Dictionary<EquippableCategory, EquipSlot> Outfit {
            get; private set;
        }
        /// <summary>
        /// Either the charge or the mental energy of the worker.
        /// </summary>
        public Bank Energy {
            get; set;
        }
        /// <summary>
        /// Where is the Citizen in the station
        /// </summary>
        public Location Position {
            get; set;
        }
        /// <summary>
        /// The category of the citizen (bot, human, etc)
        /// </summary>
        public CitizenCategory Category {
            get; set;
        }
        /// <summary>
        /// The name of the citizen
        /// </summary>
        public string Name {
            get; set;
        }
        /// <summary>
        /// How close to death is the citizen
        /// </summary>
        public Bank Health {
            get; set;
        }
        /// <summary>
        /// What the citizen is capable of, and what they are trained on
        /// </summary>
        public List<Skill> Skills {
            get; set;
        } = new List<Skill>( );
        /// <summary>
        /// A user-friendly indicator of the task currently being performed, as well as a locking indicator for what is holding the task.
        /// </summary>
        public CitizenActivity CurrentActivity {
            get; set;
        }
        /// <summary>
        /// Is the energy level of the citizen under 8%
        /// </summary>
        public bool NeedsRest => Energy.Maximum * 0.08 > Energy.Quantity;
        /// <summary>
        /// Is the energy level of the citizen above 45%
        /// </summary>
        public bool IsRested => Energy.Maximum * 0.45 < Energy.Quantity;

        /// <summary>
        /// Does the citizen have enough energy for amt
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public bool HasEnoughEnergy(uint amt) => Energy.HasEnoughFor(amt) && !NeedsRest;


        /// <summary>
        /// Unequip whatever is at the category specified
        /// </summary>
        /// <param name="category"></param>
        public void Unequip(EquippableCategory category) => Perform(Outfit.ContainsKey(category), () => Outfit[category].Equipped = null);
        /// <summary>
        /// Unequip a specific equippable
        /// </summary>
        /// <param name="eq"></param>
        public void Unequip(Equippable eq) {
            foreach ((EquippableCategory cat, EquipSlot slot) in Outfit) {
                if (slot.Equipped == eq) {
                    slot.Equipped = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Does the EquipSlot matching the category have anything equipped?
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool HasEquipped(EquippableCategory category) => Outfit[category].Equipped == null;

        /// <summary>
        /// Is a specific equippable equipped?
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        public bool IsEquipped(Equippable eq) {
            foreach ((EquippableCategory cat, EquipSlot slot) in Outfit) {
                if (slot.Equipped == eq) {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Is a specific category supported by the citizen
        /// </summary>
        /// <param name="sup"></param>
        /// <returns></returns>
        public bool SupportsCategory(EquippableCategory sup) => Outfit.ContainsKey(sup);

        /// <summary>
        /// Is a specific equippable supported by the citizen?
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        public bool SupportedEquippable(Equippable eq) {
            foreach (var cat in eq.Category) {
                if (SupportsCategory(cat)) {
                    return true;
                }
            }
            return false;
        }
    }
}
