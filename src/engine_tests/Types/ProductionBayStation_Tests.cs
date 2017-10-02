using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Engine.Types;
using Engine.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace EngineTests.Types {
    [TestClass]
    public class ProductionBaySlot_Tests {
        static Resource Scrap = new Resource {
            Identifier = 0x00000001,
            Name = "Scrap"
        };
        static Resource Metal = new Resource {
            Identifier = 0x00000002,
            Name = "Metal"
        };
        static Ingredient<Resource> ScrapIng = new Ingredient<Resource>(
            new Quantified<Resource>(Scrap, 4),
            10,
            1,
            1);
        static Ingredient<Resource> ScrapIngHighEnergyCost = new Ingredient<Resource>(
            new Quantified<Resource>(Scrap, 4),
            100,
            10,
            10);

        static Recipe MetalRecipe = new Recipe(
            new List<Ingredient<Resource>> { ScrapIng },
            null,
            new Quantified<Resource>(Metal, 1)
        );

        #region ExpendIngredient
        [TestMethod]
        public void ExpendIngredient_Works() {
            ProductionBaySlot slot = new ProductionBaySlot(null, null, new ResourceBank(200, new List<Quantified<Resource>> {
                new Quantified<Resource>(Scrap, 20)
            }));
            slot.ExpendIngredient(ScrapIng);
            Assert.IsTrue(slot.Resources[Scrap] == 16, $"Expected 16, instead have {slot.Resources[Scrap].Quantity}");
        }
        [TestMethod]
        public void ExpendIngredient_ErrorsIfLackingMaterials() {
            ProductionBaySlot slot = new ProductionBaySlot(null, null, new ResourceBank(200, new List<Quantified<Resource>> {
                new Quantified<Resource>(Scrap, 3)
            }));
            Assert.ThrowsException<LackingResourceException>(() => slot.ExpendIngredient(ScrapIng));
        }
        [TestMethod]
        public void ExpendIngredient_CallsOnLackingInsteadOfError() {
            ProductionBaySlot slot = new ProductionBaySlot(null, null, new ResourceBank(200, new List<Quantified<Resource>> {
                new Quantified<Resource>(Scrap, 3)
            }));
            bool works = false;
            slot.ExpendIngredient(ScrapIng, () => works = true);
            Assert.IsTrue(works, "Did not call action");
        }
        #endregion

        #region ExpendEnergy
        [TestMethod]
        public void ExpendEnergy_TakesFromPoolFirst() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 8
                },
                new RegeneratingBank {
                    Maximum = 10,
                    Quantity = 10
                },
                null);
            slot.ExpendEnergy(10, () => Assert.Fail("Should not error."));

        }
        [TestMethod]
        public void ExpendEnergy_TakesFromReserveIfPoolEmpty() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null);
            slot.ExpendEnergy(10);
            Assert.IsTrue(slot.Reserve == 10, $"Expected slot.Reserve to be 10, actually {slot.Reserve}");
        }

        [TestMethod]
        public void ExpendEnergy_ErrorsIfNotEnoughEnergy() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null);
            Assert.ThrowsException<NotEnoughEnergyException>(() => slot.ExpendEnergy(1000));
        }
        #endregion

        #region ClearActive
        [TestMethod]
        public void ClearActive_ClearsActive() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null) {
                Active = MetalRecipe
            };
            slot.ClearActive( );
            Assert.IsTrue(slot.Active == null);
        }
        [TestMethod]
        public void ClearActive_ClearsWorkPairings() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null) {
                Active = MetalRecipe
            };
            // Use reflection to read and write to a private value
            var field = typeof(ProductionBaySlot)
                .GetField("WorkPairings", BindingFlags.NonPublic | BindingFlags.Instance);
            // Give it a "valid" value
            field.SetValue(slot, new Dictionary<Citizen, Ingredient<Resource>> {
                { new Citizen(), ScrapIng }
            });
            // Clear it
            slot.ClearActive( );
            // See what happened
            Dictionary<Citizen, Ingredient<Resource>> pairs = field.GetValue(slot) as Dictionary<Citizen, Ingredient<Resource>>;
            Assert.IsTrue(pairs == null || pairs.Keys.Count == 0, "Pairs was neither empty or null");

        }
        #endregion

        #region ActivateRecipe
        [TestMethod]
        public void ActivateRecipeRec_Activates() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null) {
            };

            slot.ActivateRecipe(MetalRecipe);
            Assert.IsNotNull(slot.Active, "Active was not set");
        }
        [TestMethod]
        public void ActivateRecipeRec_CallsClearActiveWhenNotNull() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null) {
                Active = new Recipe(MetalRecipe)
            };
            var field = typeof(ProductionBaySlot)
                .GetField("WorkPairings", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(slot, new Dictionary<Citizen, Ingredient<Resource>> {
                { new Citizen(), ScrapIng }
            });

            slot.ActivateRecipe(MetalRecipe);

            Dictionary<Citizen, Ingredient<Resource>> pairs = field.GetValue(slot) as Dictionary<Citizen, Ingredient<Resource>>;
            Assert.IsTrue(pairs == null || pairs.Keys.Count == 0, "Did not clear workpairings, must not have called ClearActive");

        }
        [TestMethod]
        public void ActivateRecipeN_RemovesNFromLineup() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                null) {
                Lineup = new List<Recipe> {
                    new Recipe(MetalRecipe)
                }
            };
            slot.ActivateRecipe(0);
            Assert.IsTrue(slot.Lineup.Count == 0, $"Expected the lineup to be empty, size is {slot.Lineup.Count}");
        }
        #endregion

        #region FinishRecipe
        [TestMethod]
        public void FinishRecipe_ErrorsOnIncompleteActive() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                new ResourceBank(100)) {
                Active = new Recipe(MetalRecipe)
            };
            Assert.ThrowsException<NotYetCompletedException>(() => slot.FinishRecipe( ));
        }
        [TestMethod]
        public void FinishRecipe_CreatesNewResourceOnComplete() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                new ResourceBank(100)) {
                Active = new Recipe(MetalRecipe)
            };
            slot.Active.Ingredients.ForEach(ing => ing.Progress.Quantity = ing.Progress.Maximum);
            slot.FinishRecipe(
                () => {
                    Assert.Fail($"Not yet completed: {slot.Active.Progress.IsFull.ToString( )}");
                });
            Assert.IsTrue(slot.Resources.Contents.Count > 0, "Was expecting something to be added to the resources.");
        }

        [TestMethod]
        public void FinishRecipe_CreatesCorrectAmountOnComplete() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                new ResourceBank(100)) {
                Active = new Recipe(MetalRecipe)
            };
            slot.Active.Ingredients.ForEach(ing => ing.Progress.Quantity = ing.Progress.Maximum);
            slot.FinishRecipe( );
            Assert.IsTrue(slot.Resources[0].Quantity == 1, $"Produced too many. Expected 1, actual {slot.Resources[0].Quantity}");
        }
        [TestMethod]
        public void FinishRecipe_CallsClearActiveWhenComplete() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                new ResourceBank(100)) {
                Active = new Recipe(MetalRecipe)
            };
            slot.Active.Ingredients.ForEach(ing => ing.Progress.Quantity = ing.Progress.Maximum);
            slot.FinishRecipe( );
            Assert.IsTrue(slot.Active == null, "Active is not null, must not have cleared");
        }

        [TestMethod]
        public void FinishRecipe_AddsCorrectAmountOnComplete() {
            ProductionBaySlot slot = new ProductionBaySlot(
                new RegeneratingBank {
                    Maximum = 1000,
                    Quantity = 0
                },
                new RegeneratingBank {
                    Maximum = 100,
                    Quantity = 20
                },
                new ResourceBank(100, new List<Quantified<Resource>> {
                    new Quantified<Resource>(Metal, 1)
                })) {
                Active = new Recipe(MetalRecipe)
            };
            slot.Active.Ingredients.ForEach(ing => ing.Progress.Quantity = ing.Progress.Maximum);
            slot.FinishRecipe( );
            Assert.IsTrue(slot.Resources[Metal].Quantity > 1, $"Expected 1, actually {slot.Resources[Metal].Quantity}");
        }
        #endregion

        #region ManageWorkers
        [TestMethod]
        public void ManageWorkers_GivesLowEnergyWorkersRest() {
            ProductionBaySlot slot = new ProductionBaySlot(null, null, null) {
                Workers = new List<Citizen> {
                    new Citizen {
                        Energy = new Bank {
                            Maximum = 100,
                            Quantity = 4
                        }
                    },
                    new Citizen {
                        Energy = new Bank {
                            Maximum = 100,
                            Quantity = 80
                        }
                    }
                },
                WorkerSeats = 4
            };
            var WorkPairingsAccessor = typeof(ProductionBaySlot)
                .GetField("WorkPairings", BindingFlags.NonPublic | BindingFlags.Instance);
            WorkPairingsAccessor.SetValue(slot, new Dictionary<Citizen, Ingredient<Resource>> {
                { slot.Workers[0], ScrapIng },
                { slot.Workers[1], ScrapIng }
            });
            slot.ManageWorkers( );
            int pairCount = (WorkPairingsAccessor.GetValue(slot) as Dictionary<Citizen, Ingredient<Resource>>).Count;
            Assert.IsTrue(pairCount == 1, $"Expected only 1 workpairing, actual {pairCount}");

        }
        [TestMethod]
        public void ManageWorkers_GivesRefreshedWorkersAJob() {
            ProductionBaySlot slot = new ProductionBaySlot(null, null, null) {
                Workers = new List<Citizen> {
                    new Citizen {
                        Energy = new Bank {
                            Maximum = 100,
                            Quantity = 80
                        }
                    },
                    new Citizen {
                        Energy = new Bank {
                            Maximum = 100,
                            Quantity = 80
                        }
                    }
                },
                WorkerSeats = 4
            };
            slot.ManageWorkers( );
            var WorkPairingsAccessor = typeof(ProductionBaySlot)
                .GetField("WorkPairings", BindingFlags.NonPublic | BindingFlags.Instance);
            var pairCount = (WorkPairingsAccessor.GetValue(slot) as Dictionary<Citizen, Ingredient<Resource>>).Count;
            Assert.IsTrue(pairCount == 2, $"Expected 2 workpairings, actual {pairCount}");
        }
        #endregion

        #region Think

        private ProductionBaySlot GetWorkingStation() => new ProductionBaySlot(
            new RegeneratingBank {
                Maximum = 1000,
                Quantity = 1000
            },
            new RegeneratingBank {
                Maximum = 100,
                Quantity = 100
            },
            new ResourceBank(100, new List<Quantified<Resource>> {
                new Quantified<Resource>(Scrap, 1000)
            })
        ) {
            Workers = (from num in Enumerable.Range(1, 4)
                       select new Citizen {
                           Name = $"Worker_{num}",
                           Energy = new Bank {
                               Maximum = 1000,
                               Quantity = 1000
                           }
                       }).ToList( ),
            WorkerSeats = 4
        };



        [TestMethod]
        public void Think_DoesStationEnergyWork() {
            var Slot = GetWorkingStation( );
            Slot.ActivateRecipe(MetalRecipe);
            for (int i = 0 ; i < 4 ; i++) {
                Slot.Think( );
            }
            Assert.IsTrue(Slot.Pool.IsFull == false, $"Expected some energy to go missing from thinking. Actual: {Slot.Pool.Quantity}");
        }

        [TestMethod]
        public void Think_DoesEventuallyProduceGoods() {
            var Slot = GetWorkingStation( );
            Slot.ActivateRecipe(MetalRecipe);
            while (!Slot.Resources.Contains(Metal)) {
                Slot.Think( );
            }
            Assert.IsTrue(Slot.Resources.Contains(Metal), "No metal added. Expected 1");
        }

        [TestMethod]
        public void Think_CanHandleLineup() {
            var Slot = GetWorkingStation( );
            Slot.ActivateRecipe(MetalRecipe);
            Enumerable.Range(0, 5)
                .ToList( )
                .ForEach(met => Slot.Lineup.Add(MetalRecipe));
            while (!Slot.Resources.Contains(Metal) || (Slot.Resources.Contains(Metal) && Slot.Resources[Metal] < 5)) {
                Slot.Think( );
            }
            Assert.IsTrue(Slot.Resources[Metal].Quantity == 5, $"Expected at least 5 metal, actual: {Slot.Resources[Metal].Quantity}");
        }
        #endregion
    }
}