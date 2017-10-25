using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LangRoids;
using Engine.Interfaces;
using Engine.Exceptions;
using Engine.Types;
using Engine.Constructables;

namespace EngineTests.Types
{
    [TestClass]
    [TestCategory("PowerProducingBay")]
    public class PowerProducingBay_Tests
    {
        private PowerProducingBay EmptyBay => new PowerProducingBay(10);
        private PowerProducingBay GetBay(int cells) {
            var bay = new PowerProducingBay((uint)cells);
            Repeat(cells, () =>  bay.Cells.Add(1000, 1000, 0, 10));
            return bay;   
        }
        [TestMethod]
        public void ExpendEnergy_ExpendsIfEnough() {
            var bay = GetBay(1);
            bay.ExpendEnergy(10);
            Assert.IsTrue(bay.PowerAvailable == 990, $"Expected energy to be 995, actually {bay.PowerAvailable}");
        }
        [TestMethod]
        public void ExpendEnergy_ExpendsMultipleCells() {
            var bay = GetBay(10);
            bay.ExpendEnergy(4300);
            Repeat(4, i => Assert.IsTrue(bay.Cells[i].available == 0, $"Did not empty cell {i + 1} of 4"));
            Assert.IsTrue(bay.Cells[4].available == 700, $"Expected cell 5 to have 700 energy, actually {bay.Cells[4].available}");
        }

        [TestMethod]
        public void Think_RegeneratesProperly() {
            var bay = GetBay(5);
            bay.ExpendEnergy(2500);
            bay.Think( );
            Assert.IsTrue(bay.Cells[0].available == 10, $"Did not regen properly. Expected available to be 10, actually {bay.Cells[0].available}");
            Assert.IsTrue(bay.Cells[1].available == 10, $"Did not regen properly. Expected available to be 10, actually {bay.Cells[1].available}");
            Assert.IsTrue(bay.Cells[2].available == 510, $"Did not regen properly. Expected available to be 510, actually {bay.Cells[2].available}");
        }

        [TestMethod]
        public void Think_DecaysProperly() {
            var bay = new PowerProducingBay(10);
            Repeat(10, () => bay.Cells.Add(100, 100, 10, 0));
            bay.Think( );
            Repeat(10, i => Assert.IsTrue(bay.Cells[i].available == 90, $"Expected cell at index {i} to be 90, actually {bay.Cells[i].available}"));
        }
    }
}
