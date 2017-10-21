using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;

namespace Engine_Tests.Types {
    [TestClass]
    [TestCategory("Banks")]
    public class RegeneratingBank_Tests {
        [TestMethod]
        public void FailsOnDecayTooLarge() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4,
                DecayRate = 5,
            };
            bool worked = false;
            bank.Decay(() => worked = true);
            Assert.IsTrue(worked);
        }

        [TestMethod]
        public void DecayWorks() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4,
                DecayRate = 2
            };
            bank.Decay(onFailure: () => {
                Assert.Fail("Not enough energy. Something is wrong with the Quantity.");
            });
            Assert.IsTrue(bank == 2);
        }

        [TestMethod]
        public void RegenWorks() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4,
                Maximum = 10,
                RegenRate = 4
            };
            bank.Regen(1);
            Assert.IsTrue(bank == 5);

            bank.Regen( );
            Assert.IsTrue(bank == 9, "Does not respect default regeneration value");
        }

        [TestMethod]
        public void RegenRespectsCapacity() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4,
                Maximum = 5,
                RegenRate = 5
            };
            bank.Regen( );
            Assert.IsTrue(bank == 5, "Does not respect capacity");
        }

        [TestMethod]
        public void PlusOperatorWorks() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4,
            };
            Assert.IsTrue(bank + 2 == 6, "+ operator does not work properly");
        }

        [TestMethod]
        public void SubtractOperatorWorks() {
            RegeneratingBank bank = new RegeneratingBank {
                Quantity = 4
            };
            Assert.IsTrue(bank - 2 == 2, "- operator does not work properly");
        }

        [TestMethod]
        public void ImplicitOperatorUintExists() {
            RegeneratingBank bank = new RegeneratingBank { Quantity = 3 };
            uint hi = bank;
            Assert.IsTrue(hi == (uint)3, "Something went wrong with the conversion");
        }

        [TestMethod]
        public void ImplicitOperatorIntExists() {
            RegeneratingBank bank = new RegeneratingBank { Quantity = 4 };
            int hi = bank;
            Assert.IsTrue(hi == (int)4, "Something went wrong with the conversion");
        }

        [TestMethod]
        public void ImplicitOperatorStringExists() {
            RegeneratingBank bank = new RegeneratingBank { Quantity = 4 };
            string amt = bank;
            Assert.IsTrue(amt == "4", "Something went wrong with the conversion");
        }
    }
}
