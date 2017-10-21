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
        static EquippableCategory Feet = new EquippableCategory { Name = "Feet", Description = "The feet of a human" };
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

        public Citizen Human => new Citizen(
            "Humnannan", 
            (1000, 1000), 
            (1000, 1000), 
            default, 
            CitizenCategory.Human, 
            new (int, EquippableCategory)[] { (1, Head), (1, Torso), (2, Legs), (2, Feet), (1, LHand), (1, RHand) });

        [TestMethod]
        [TestCategory("Citizen_Equip")]
        public void Equip_Equips() {
            var cit = Human;
            cit.Equip(Gun, RHand);
            Assert.IsTrue(cit.Outfit[RHand].Find(slot => slot.Equipped == Gun) != null);
        }

        [TestMethod]
        [TestCategory("Citizen_Equip")]
        public void Unequip_Equippable_unequips() {
            var cit = Human;
            cit.Equip(Gun, RHand);
            // This must be true to continue
            Assert.IsTrue(cit.Outfit[RHand].Find(slot => slot.Equipped == Gun) != null, "Equip is not properly implemented");
            cit.Unequip(Gun);
            Assert.IsTrue(cit.Outfit[RHand].Find(slot => slot.Equipped == Gun) == null, "Unequip did not remove the gun from the citizen");
        }
    }
}
