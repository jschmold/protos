using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    /// <summary>
    /// A recipe for creating an Amount of T, and the Resource R needed in the ingredients list
    /// </summary>
    public class Recipe {
        /// <summary>
        /// List of ingredients identifiers as the id, and the quantity as the value
        /// </summary>
        public List<Ingredient<Resource>> Ingredients {
            get; set;
        }

        /// <summary>
        /// List of research requirement identifiers required for
        /// </summary>
        public List<uint> ResearchRequirements {
            get; set;
        }

        /// <summary>
        /// Starts at 0, works its way up to max. Once max, produces T
        /// </summary>
        public Bank Progress {
            get {
                Bank prog = new Bank( );
                Ingredients.ForEach(ing => {
                    prog.Quantity += ing.Progress.Quantity;
                    prog.Maximum += ing.Progress.Quantity;
                });
                return prog;
            }
        }

        /// <summary>
        /// What is produced by this recipe, and how much
        /// </summary>
        public Quantified<Resource> Produces {
            get; set;
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="ings">The Ingredient collection (really more of a blueprint)</param>
        /// <param name="resReqs">The research requirements</param>
        /// <param name="prods">What is produced by the recipe, and how much</param>
        public Recipe(List<Ingredient<Resource>> ings, List<uint> resReqs, Quantified<Resource> prods) {
            Ingredients = ings;
            ResearchRequirements = resReqs;
            Produces = prods;
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="ings">The Ingredient collection (really more of a blueprint)</param>
        /// <param name="resReqs">The research requirements</param>
        /// <param name="produces">What is produced</param>
        /// <param name="quantity">How much is produced</param>
        public Recipe(List<Ingredient<Resource>> ings, List<uint> resReqs, Resource produces, uint quantity)
            : this(ings, resReqs, new Quantified<Resource> { Contents = produces, Quantity = quantity }) { }

        public Recipe(Recipe rec)
            : this(rec.Ingredients, rec.ResearchRequirements, rec.Produces) { }


    }
}
