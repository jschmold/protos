using System;
using System.Collections.Generic;
using System.Text;
using Engine.Constructables;
using static Engine.Helpers.Lang;
namespace Engine.Types {
    class BayRecipe : Recipe<Resource, Bay> {
        public (float, float) Dimensions {
            get; set;
        }
        public BayRecipe((float, float) dimensions, Recipe<Resource, Bay> rec)
            : base(rec)
            => Dimensions = (dimensions.Item1, dimensions.Item2);

        public BayRecipe((float, float) dimensions, List<Ingredient<Resource>> ings, List<Skill> resReqs, Quantified<Bay> prods)
            : base(ings, resReqs, prods)
            => Dimensions = (dimensions.Item1, dimensions.Item2);

        public BayRecipe((float, float) dimensions, List<Ingredient<Resource>> ings, List<Skill> resReqs, Bay produces, uint quantity)
            : base(ings, resReqs, produces, quantity)
            => Dimensions = (dimensions.Item1, dimensions.Item2);
    }
}
