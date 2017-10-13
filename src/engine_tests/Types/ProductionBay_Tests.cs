using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;
using Engine.Constructables;
using static Engine.Helpers.Lang;
using Engine.Interfaces;

namespace EngineTests.Types
{
    [TestClass]
    [TestCategory("ProductionBay")]
    public class ProductionBay_Tests
    {
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
        static Recipe<Resource, Resource> MetalRecipe = new Recipe<Resource, Resource>(
            new List<Ingredient<Resource>> { ScrapIng },
            null,
            new Quantified<Resource>(Metal, 1)
        );
        static Skill MetalWork = new Skill {
            Description = "The ability to work with metal",
            Identifier = 0x0000001,
            Name = "MetalWorking"
        };
        static Recipe<Resource, Resource> MetalRecipe_WithSkillReq = new Recipe<Resource, Resource>(
            MetalRecipe.Ingredients, new List<Skill> {
                MetalWork
            },
            MetalRecipe.Produces);

        [TestMethod]
        public void Craft_FailsOnUnsupported() {
            ProductionBay bay = new ProductionBay(
                null,
                100,
                new List<Recipe<Resource, Resource>> {
                    MetalRecipe_WithSkillReq
                },
                (4, 4),
                (1000, 1000),
                (100, 100),
                1000,
                null,
                0);
            bay.Resources.Add(Scrap, 1000);
            Assert.ThrowsException<UnsupportedException>(() => bay.Craft(MetalRecipe));
        }

        [TestMethod]
        public void Regenerate_RegeneratesEnergy() {
            PowerCellCluster cluster = new PowerCellCluster(10);
            Repeat(10, _ => cluster.Add(1000, 1000, 0, 10));
            ProductionBay bay = new ProductionBay(
                null,
                100,
                new List<Recipe<Resource, Resource>> {
                    MetalRecipe_WithSkillReq
                },
                (5, 1),
                (100, 0),
                (100, 0),
                0,
                new List<IPowerSource> { cluster },
                5);
            Repeat(20, i => {
                bay.RegeneratePower( );
                var expected = (i + 1) * 5;
                Assert.IsTrue(bay.Pool.Quantity == expected, $"Expected power in pool to be {expected}, actually {bay.Pool.Quantity}. Failed on index {i}");
            });
            Repeat(20, i => {
                bay.RegeneratePower( );
                var expected = (i + 1) * 5;
                Assert.IsTrue(bay.Reserve.Quantity == expected, $"Expected power in reserve to be {expected}, actually {bay.Pool.Quantity}. Failed on index  {i}");
            });
        }

        [TestMethod]
        public void DrawEnergy_DrawsFromSource() {
            PowerCellCluster cluster = new PowerCellCluster(10);
            Repeat(10, _ => cluster.Add(1000, 1000, 0, 10));
            ProductionBay bay = new ProductionBay(
                null,
                100,
                new List<Recipe<Resource, Resource>> {
                    MetalRecipe_WithSkillReq
                },
                (5, 1),
                (100, 0),
                (100, 0),
                0,
                new List<IPowerSource> { cluster },
                10);
            bay.DrawEnergy(10);
            Assert.IsTrue(cluster.PowerAvailable == 9990, $"Expected only 10 energy to be drawn, actually {10000 - cluster.PowerAvailable}");
        }
    }
}
