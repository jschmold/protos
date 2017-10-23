using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LangRoids;
using Engine.Types;
using Engine.Exceptions;
using Engine.Interfaces;
using Engine.Constructables;
using Engine.Entities;

namespace EngineTests.Types
{
    [TestClass]
    [TestCategory("Citizen")]
    public class Citizen_Tests
    {
        static EquippableCategory Head = new EquippableCategory { Name = "Head", Description = "The head of a human" };
        static EquippableCategory Torso = new EquippableCategory { Name = "Torso", Description = "The body of a human" };
        static EquippableCategory Legs = new EquippableCategory { Name = "Legs", Description = "The legs of a human" };
        static EquippableCategory LFoot = new EquippableCategory { Name = "LFoot", Description = "The left foot of a human" };
        static EquippableCategory RFoot = new EquippableCategory { Name = "RFoot", Description = "The right foot of a human" };
        static EquippableCategory LHand = new EquippableCategory { Name = "LHand", Description = "The left hand of a human" };
        static EquippableCategory RHand = new EquippableCategory { Name = "RHand", Description = "The right hand of a human" };
        static Equippable Gun = new Equippable {
            Category = new EquippableCategory[] { RHand, LHand },
            Description = "A gun",
            Name = "Gun",
            Mass = 5
        };
        static Equippable Shirt = new Equippable {
            Category = new EquippableCategory[] { Torso },
            Description = "Standard issue shirt",
            Name = "Generic Shirt"
        };
        static Equippable LBoot = new Equippable {
            Category = new EquippableCategory[] { LFoot },
            Description = "A boot for ye might pirates",
            Name = "Boot"
        };
        public Citizen Human => new Citizen(
            "Humnannan", 
            (1000, 1000), 
            (1000, 1000), 
            default, 
            CitizenCategory.Human, 
            new EquippableCategory[] { Head, Torso, Legs, LFoot, RFoot, LHand, RHand });

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void EquipCategory_Equips() {
            var cit = Human;
            cit.Equip(Gun, RHand);
            Assert.IsTrue(cit[RHand].Equipped == Gun);
        }

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void EquipNoSlot_Equips() {
            var cit = Human;
            var slot = cit[RHand];
            cit.Equip(Gun, slot);
            Assert.IsTrue(cit[RHand].Equipped == Gun, "Did not equip gun at right hand");
        }


        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void Unequip_Equippable_unequips() {
            var cit = Human;
            cit.Equip(Gun, RHand);
            // This must be true to continue
            Assert.IsTrue(cit[RHand].Equipped == Gun, "Equip is not properly implemented");
            cit.Unequip(Gun);
            Assert.IsTrue(cit[RHand].Equipped == null, "Unequip did not remove the gun from the citizen");
        }

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void UnequipCategory_UnequipsAll() {
            var cit = Human;
            var lfoot = cit[LFoot];
            cit.Equip(LBoot, lfoot);
            cit.Unequip(LFoot);
            Assert.IsTrue(lfoot.Equipped == null, "Did not remove from all in a category");
        }

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void IsEquipped_TrueWhenActuallyTrue() {
            var cit = Human;
            cit[LFoot].Equipped = LBoot;
            Assert.IsTrue(cit.IsEquipped(LBoot), "Did not return correct value");
        }

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void IsEquipped_FalseWhenNotEquipped(){ 
            var cit = Human;
            Assert.IsFalse(cit.IsEquipped(LBoot), "Did not return correct value");
        }

        [TestMethod]
        [TestCategory("Citizen_Outfitting")]
        public void GetEquippableContainingSlot_GetsCorrectSlot(){ 
            var cit = Human;
            var leftFoot = cit[LFoot];
            cit.Equip(LBoot, leftFoot);
            Assert.IsTrue(cit.GetEquippableContainingSlot(LBoot) == leftFoot, "Did not retrieve correct slot");
        }
        
    }
}
