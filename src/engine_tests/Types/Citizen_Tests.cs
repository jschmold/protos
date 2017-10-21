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

        public Citizen Human => new Citizen {
            Category = CitizenCategory.Human,
            Energy = new Bank {
                Maximum = 1000,
                Quantity = 1000
            },
            Health = new Bank {
                Maximum = 100,
                Quantity = 100
            }
        };

        [TestMethod]
        public void Equip_Equips() {
            var cit = Human;
        }
    }
}
