using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;

namespace EngineTests.Types {
    [TestClass]
    [TestCategory("ResearchBay")]
    public class ResearchBay_Tests {
        Knowledge Science = new Knowledge { };
        Knowledge Math = new Knowledge { };
        public ResearchBay GetBay() => new ResearchBay(null, 100, (1000, 1000), (1000, 1000), 10, new List<Knowledge> { Science, Math }, 100);

        [TestMethod]
        public void AddResearcher_AddsResearcher() {
            ResearchBay bay = new ResearchBay(null, 0, (0, 0), (0, 0), 3, new List<Knowledge> {
                Science
            }, 0);
            bay.AddResearcher(new Citizen());
            Assert.IsTrue(bay.Researchers.Count == 1, $"Expected 1, actual {bay.Researchers.Count}");
        }

        [TestMethod]
        public void AddResearcher_ListensToLimit() {
            ResearchBay bay = new ResearchBay(null, 0, (0, 0), (0, 0), 3, new List<Knowledge> {
                Science
            }, 0);
            for (int i = 0 ; i < 3 ; i++) {
                bay.AddResearcher(new Citizen( ));
            }
            Assert.ThrowsException<LimitMetException>(() => bay.AddResearcher(new Citizen( )), "Expected to error on 4th addition.");
        }

           
    }
}
