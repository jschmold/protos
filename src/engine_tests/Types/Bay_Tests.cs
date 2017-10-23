using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;
using Engine.Constructables;
using static LangRoids;
using Engine.Entities;

namespace EngineTests.Types {
    class TestBay : Bay {
        public override void Think() => throw new NotImplementedException( );
        public TestBay(uint occLim) : base(occLim) => DoNothing( );
    }
    [TestCategory("Bay")]
    [TestClass]
    public class Bay_Tests {
        [TestMethod]
        public void AddOccupant_Works() {
            Bay bay = new TestBay(20);
            bay.AddOccupant(new Citizen {
                Name = "TestWorker"
            });

            Assert.IsTrue(bay.Occupants[0].Name == "TestWorker", "Could not find the worker that we just added");
        }

        [TestMethod]
        public void AddOccupant_RespectsLimit() {
            Bay bay = new TestBay(2);
            bay.AddOccupant(new Citizen {
                Name = "TestWorker1"
            });
            bay.AddOccupant(new Citizen {
                Name = "TestWorker2"
            });
            bool works = false;
            bay.AddOccupant(new Citizen { }, () => works = true);
            Assert.IsTrue(works, "Did not respect limit");
        }

        [TestMethod]
        public void AddOccupant_CollectionWorks() {
            Bay bay = new TestBay(10);
            List<Citizen> work = new List<Citizen>( );
            Repeat(8, i => work.Add(new Citizen { Name = $"Worker_{i}" }));
            bay.AddOccupantRange(work);
            Repeat(8, i => Assert.IsTrue(bay.Occupants[i].Name == $"Worker_{i}", $"Missing worker {i}"));
        }

        [TestMethod]
        public void AddOccupant_RunsFailureInsteadOfException() {
            Bay bay = new TestBay(10);
            Repeat(10, i => bay.AddOccupant(new Citizen { Name = $"Worker_{i}" }));
            bool works = false;
            bay.AddOccupant(new Citizen { }, () => works = true);
            Assert.IsTrue(works);
        }

        [TestMethod]
        public void RemoveOccupant_Works() {
            Bay bay = new TestBay(10);
            Citizen work = new Citizen { Name = "TestWorker" };
            bay.AddOccupant(work);
            bay.RemoveOccupant(work);
            Assert.IsFalse(bay.Occupants.Contains(work), "Citizen was not removed");
        }
    }
}
