using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine_Tests.Types {
    [TestClass]
    [TestCategory("Banks")]
    public class ResourceBank_Tests {
        Resource TestResource = new Resource {
            Identifier = 0x0000001,
            Mass = 20,
            Name = "TestResource",
            Volume = 1
        };

        [TestMethod]
        public void ResourceBank_AddWorks() {
            ResourceBank cargo = new ResourceBank(20);
            cargo.Add(TestResource);
            bool found = false;
            foreach (Resource res in cargo.Contents) {
                if (res.Identifier == TestResource.Identifier) {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue(found, "Resource was not added");
        }

        [TestMethod]
        public void ResourceBank_Add_RespectsVolumeCapacity() {
            ResourceBank cargo = new ResourceBank(2);
            cargo.Add(TestResource);
            cargo.Add(TestResource);
            bool works = false;
            cargo.Add(TestResource, onFailure: () => works = true);
            Assert.IsTrue(works, "Does not respect volume capacity limits");
        }

        [TestMethod]
        public void ResourceBank_Add_ModifiesVolume() {
            ResourceBank cargo = new ResourceBank(20);
            uint curVol = cargo.Quantity;
            cargo.Add(TestResource);
            Assert.IsTrue(curVol != cargo.Quantity, "Does not modify volume");
        }

        [TestMethod]
        public void ResourceBank_Add_ModifiesVolumeCorrectly() {
            ResourceBank cargo = new ResourceBank(20);
            cargo.Add(TestResource, 8);
            Assert.IsTrue(cargo.Quantity == 8);
        }

        [TestMethod]
        public void ResourceBank_Bank_IndexerWorks() {
            ResourceBank bank = new ResourceBank(20);
            bank.Add(TestResource, 5);
            Assert.IsNotNull(bank[TestResource]);
        }

        [TestMethod]
        public void ResourceBank_RemoveWorks() {
            ResourceBank bank = new ResourceBank(20);
            bank.Add(TestResource, 5);

            bank.Remove(TestResource, 2);
            Assert.IsTrue(bank[TestResource].Quantity != 5, "Does not reduce quantity");
        }

        [TestMethod]
        public void ResourceBank_Remove_RemovesProperQuantity() {
            ResourceBank cargo = new ResourceBank(20);
            cargo.Add(TestResource, 10);
            cargo.Remove(TestResource, 2);
            Assert.IsTrue(cargo[TestResource].Quantity == 8, "Does not remove proper quantity");
        }
    }
}
