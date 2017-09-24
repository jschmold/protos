using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;

namespace EngineTests.Types {
    [TestClass]
    public class Bay_Tests {
        [TestMethod]
        public void AddOccupant_Works() {
            Bay bay = new Bay(null, 20);
            bay.AddOccupant(new Worker {
                Name = "TestWorker"
            });

            Assert.IsTrue(bay.Occupants[0].Name == "TestWorker", "Could not find the worker that we just added");
        }

        [TestMethod]
        public void AddOccupant_RespectsLimit() {
            Bay bay = new Bay(null, 2);
            bay.AddOccupant(new Worker {
                Name = "TestWorker1"
            });
            bay.AddOccupant(new Worker {
                Name = "TestWorker2"
            });
            bool works = false;
            bay.AddOccupant(new Worker { }, () => works = true);
            Assert.IsTrue(works, "Did not respect limit");
        }

        [TestMethod]
        public void AddOccupant_CollectionWorks() {
            Bay bay = new Bay(null, 10);
            List<Worker> work = new List<Worker>( );
            for (int i = 0 ; i < 9 ; i++)
                work.Add(new Worker { Name = $"Worker_{i}" });
            bay.AddOccupant(work);
            for (int i = 0 ; i < 9 ; i++)
                Assert.IsTrue(bay.Occupants[i].Name == $"Worker_{i}", $"Missing worker {i}");
        }

        [TestMethod]
        public void AddOccupant_RunsFailureInsteadOfException() {
            Bay bay = new Bay(null, 10);
            for (int i = 0 ; i < 10 ; i++)
                bay.AddOccupant(new Worker { Name = $"Worker_{i}" });
            bool works = false;
            bay.AddOccupant(new Worker { }, () => works = true);
            Assert.IsTrue(works);
        }

        [TestMethod]
        public void RemoveOccupant_Works() {
            Bay bay = new Bay(null, 10);
            Worker work = new Worker { Name = "TestWorker" };
            bay.AddOccupant(work);
            bay.RemoveOccupant(work);
            Assert.IsFalse(bay.Occupants.Contains(work), "Worker was not removed");
        }
    }
}
