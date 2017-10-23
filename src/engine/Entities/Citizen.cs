using System;
using System.Collections.Generic;
using System.Text;
using Engine.Helpers;
using static LangRoids;
using Engine.Exceptions;
using Engine.Types;
using Engine.Interfaces;

namespace Engine.Entities {
    public class Citizen : IEntity {
        public Dictionary<EquippableCategory, EquipSlot> Outfit {
            get; private set;
        }

        public Citizen() {
            Skills = new List<Skill>( );
            Outfit = new Dictionary<EquippableCategory, EquipSlot>( );
        }

        /// <summary>
        /// Create a fully detailed citizen
        /// </summary>
        /// <param name="name"></param>
        /// <param name="max"></param>
        /// <param name="start"></param>
        public Citizen(string name,
            (uint max, uint start) health,
            (uint max, uint start) energy,
            Location pos,
            CitizenCategory category,
            IEnumerable<EquippableCategory> bodyParts) : this() {
            Health = new Bank { Maximum = health.max, Quantity = health.start };
            Energy = new Bank { Maximum = energy.max, Quantity = energy.start };
            Position = pos;
            Category = category;
            ForEach(bodyParts, cat => {
                Outfit.Add(cat, new EquipSlot( ));
            });
            
        }
        
        /// <summary>
        /// Get the EquipSlot of an EquippableCategory
        /// </summary>
        /// <returns>Null or EquipSlot</returns>
        public EquipSlot this[EquippableCategory key] => SupportsCategory(key) ? Outfit[key] : null;

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

        public void Equip(Equippable eq, EquipSlot slot, Action onSlotOccupied = null, Action onInvalidEquippable = null) => Perform(
            (SupportedEquippable(eq), onInvalidEquippable, new InappropriateEquippableException( )),
            (slot.Equipped == null, onSlotOccupied, new EquipSlotOccupiedException( )), 
            () => slot.Equipped = eq);

        public void Equip(Equippable eq, EquippableCategory cat, Action onNoSlotsAvailable = null, Action onInvalidEquippable = null) => Perform(
            (GetEquippableContainingSlot(eq) == null, DoNothing),
            (eq.Category.Filter(obj => obj == cat) != null, new InappropriateEquippableException()),
            (SupportedEquippable(eq), onInvalidEquippable, new InappropriateEquippableException()),
            () => {
                var slot = AvailableEquipslot(cat);
                if (slot != null) {
                    slot.Equipped = eq;
                    return;
                }
                DoOrThrow(onNoSlotsAvailable, new NoAvailableSlotsException( ));
            });
        /// <summary>
        /// Unequip everything in the category provided
        /// </summary>
        /// <param name="category"></param>
        public void Unequip(EquippableCategory category) => Perform(SupportsCategory(category), () => Outfit[category].Equipped = null);
        /// <summary>
        /// Unequip a specific equippable
        /// </summary>
        /// <param name="eq"></param>
        public void Unequip(Equippable eq) => Perform(SupportedEquippable(eq), () => {
            var slot = GetEquippableContainingSlot(eq);
            Perform(slot != null, () => slot.Equipped = null);
        });

        /// <summary>
        /// Does the EquipSlot matching the category have anything equipped?
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool HasEquipped(EquipSlot slot) => slot.Equipped == null;

        /// <summary>
        /// Is a specific equippable equipped?
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        public bool IsEquipped(Equippable eq) => SupportedEquippable(eq) ? GetEquippableContainingSlot(eq) != null : false;

        public EquipSlot GetEquippableContainingSlot(Equippable eq) {
            foreach (var cat in eq.Category.Filter(cat => SupportsCategory(cat))) {
                var Slot = Outfit[cat];
                if (Slot.Equipped == eq) {
                    return Slot;
                }
            }
            return null;
        }

        /// <summary>
        /// Is a specific category supported by the citizen?
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public bool SupportsCategory(EquippableCategory cat) => Outfit.ContainsKey(cat);

        /// <summary>
        /// Is a specific equippable supported by the citizen?
        /// </summary>
        /// <param name="eq">The equippable to check</param>
        /// <returns>If you can equip eq</returns>
        public bool SupportedEquippable(Equippable eq) => eq.Category.Filter(SupportsCategory) != null;

        private EquipSlot AvailableEquipslot(EquippableCategory cat, Action onUnsupportedCategory = null) {
            if (!SupportsCategory(cat)) {
                DoOrThrow(onUnsupportedCategory, new InappropriateEquippableException( ));
                return null;
            }
            var slot = Outfit[cat];
            return slot.Equipped == null ? slot : null;
        }

        public void Think() => DoNothing( ); 
    }
}
