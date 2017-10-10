using System;
using System.Collections.Generic;
using System.Text;
using static Engine.LangHelpers;

namespace Engine.Types {
    /// <summary>
    /// A recipe for creating an Amount of T, and the Resource R needed in the ingredients list
    /// </summary>
    public class Recipe<I, P> {
        /// <summary>
        /// List of ingredients identifiers as the id, and the quantity as the value
        /// </summary>
        /// I
        public List<Ingredient<I>> Ingredients {
            get; set;
        }

        /// <summary>
        /// List of research requirement identifiers required for
        /// </summary>
        public List<Skill> ResearchRequirements {
            get; set;
        }

        /// <summary>
        /// Starts at 0, works its way up to max. Once max, produces T
        /// </summary>
        public Bank Progress {
            get {
                Bank prog = new Bank {
                    Maximum = 0,
                    Quantity = 0
                };
                // The order of the below DOES MATTER.
                // The maximum must FIRST be altered because it is used to check if adding to the quantity is valid.
                Ingredients.ForEach(ing => {
                    prog.Maximum += ing.Progress.Maximum;
                    prog.Quantity += ing.Progress.Quantity;
                });
                return prog;
            }
        }

        /// <summary>
        /// What is produced by this recipe, and how much
        /// </summary>
        /// P
        public Quantified<P> Produces {
            get; set;
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="ings">The Ingredient collection (really more of a blueprint)</param>
        /// <param name="resReqs">The research requirements</param>
        /// <param name="prods">What is produced by the recipe, and how much</param>
        public Recipe(List<Ingredient<I>> ings, List<Skill> resReqs, Quantified<P> prods) {
            Ingredients = new List<Ingredient<I>>( );
            ings.ForEach((Ingredient<I> ing) => Ingredients.Add(new Ingredient<I>(ing)));
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
        public Recipe(List<Ingredient<I>> ings, List<Skill> resReqs, P produces, uint quantity)
            : this(ings, resReqs, new Quantified<P> (produces, quantity )) { }

        public Recipe(Recipe<I, P> rec)
            : this(rec.Ingredients, rec.ResearchRequirements, rec.Produces.Contents, rec.Produces.Quantity) { }

        /// <summary>
        /// Does a citizen meet the requirements of a recipe
        /// </summary>
        /// <param name="wk">The citizen to test</param>
        /// <returns>Whether or not the citizen meets the work requirements</returns>
        public bool MeetsRequirements(Citizen wk) => ResearchRequirements == null || ResearchRequirements.Count == 0 || ResearchRequirements.TrueForAll((Skill sk) => wk.Skills.Contains(sk));
    }
}
