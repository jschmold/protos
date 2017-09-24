using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;

namespace EngineTests.Types {
    [TestClass]
    public class ProductionBayStation_Tests {
        [TestMethod]
        public void SharesReserves() {
            ProductionBay bay = new ProductionBay(
                null,
                100,
                null,
                new RegeneratingBank {
                    Quantity = 100,
                    Maximum = 100,
                    RegenRate = 0,
                    DecayRate = 0
                },
                new RegeneratingBank {
                    Quantity = 10,
                    Maximum = 10,
                    RegenRate = 0,
                    DecayRate = 0
                });
            ProductionBayStation station = new ProductionBayStation(bay.EnergyPool, bay.EnergyReserve);

            station.Pool.Decay( );
            Assert.IsTrue(bay.EnergyPool < 100);
        }
    }
}
