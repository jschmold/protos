using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;

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
        static Recipe MetalRecipe = new Recipe(
            new List<Ingredient<Resource>> { ScrapIng },
            null,
            new Quantified<Resource>(Metal, 1)
        );
        static Skill MetalWork = new Skill {
            Description = "The ability to work with metal",
            Identifier = 0x0000001,
            Name = "MetalWorking"
        };
        static Recipe MetalRecipe_WithSkillReq = new Recipe(
            MetalRecipe.Ingredients, new List<Skill> {
                MetalWork
            },
            MetalRecipe.Produces);

        public void Craft_FailsOnUnsupported() {
            ProductionBay bay = new ProductionBay(
                null,
                100,
                new List<Recipe> {
                    MetalRecipe_WithSkillReq
                },
                (4, 4),
                (1000, 1000),
                (100, 100),
                1000
                );
            bay.Resources.Add(Scrap, 1000);
            Assert.ThrowsException<UnsupportedRecipeException>(() => bay.Craft(MetalRecipe));
        }
    }
}
