using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;
using Engine.Bays;

namespace EngineTests.Types {
    [TestClass]
    [TestCategory("Bay")]
    public class Bay_Tests {
        [TestMethod]
        public void AddOccupant_Works() {
            Bay bay = new Bay(null, 20);
            bay.AddOccupant(new Citizen {
                Name = "TestWorker"
            });

            Assert.IsTrue(bay.Occupants[0].Name == "TestWorker", "Could not find the worker that we just added");
        }

        [TestMethod]
        public void AddOccupant_RespectsLimit() {
            Bay bay = new Bay(null, 2);
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
            Bay bay = new Bay(null, 10);
            List<Citizen> work = new List<Citizen>( );
            for (int i = 0 ; i < 9 ; i++) {
                work.Add(new Citizen { Name = $"Worker_{i}" });
            }

            bay.AddOccupant(work);
            for (int i = 0 ; i < 9 ; i++) {
                Assert.IsTrue(bay.Occupants[i].Name == $"Worker_{i}", $"Missing worker {i}");
            }
        }

        [TestMethod]
        public void AddOccupant_RunsFailureInsteadOfException() {
            Bay bay = new Bay(null, 10);
            for (int i = 0 ; i < 10 ; i++) {
                bay.AddOccupant(new Citizen { Name = $"Worker_{i}" });
            }

            bool works = false;
            bay.AddOccupant(new Citizen { }, () => works = true);
            Assert.IsTrue(works);
        }

        [TestMethod]
        public void RemoveOccupant_Works() {
            Bay bay = new Bay(null, 10);
            Citizen work = new Citizen { Name = "TestWorker" };
            bay.AddOccupant(work);
            bay.RemoveOccupant(work);
            Assert.IsFalse(bay.Occupants.Contains(work), "Citizen was not removed");
        }
    }
}
