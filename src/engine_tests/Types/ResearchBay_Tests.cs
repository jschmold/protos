using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;

namespace EngineTests.Types {
    [TestClass]
    [TestCategory("ResearchBay")]
    public class ResearchBay_Tests {
        Knowledge Science = new Knowledge {

        };
        public ResearchBay GetBay() => new ResearchBay(null, 100, (1000, 1000), (1000, 1000), 10, null, 100);

        [TestMethod]
        public void AddResearcher_AddsResearcher() {
            ResearchBay bay = new ResearchBay(null, 0, (0, 0), (0, 0), 3, null, 0);
            bay.AddResearcher(new Citizen( ));
            Assert.IsTrue(bay.Researchers.Count == 1, $"Expected 1, actual {bay.Researchers.Count}");
        }
        
        [TestMethod]
        public void AddResearcher_ListensToLimit() => Assert.Fail("Unwritten test");

        [TestMethod]
        public void AddResearcher_ThrowsErrorAfterLimit() => Assert.Fail("Test not written");

        [TestMethod]
        public void AddResearcher_DoesActionIfNotNull() => Assert.Fail("Test not written");
    }
}
