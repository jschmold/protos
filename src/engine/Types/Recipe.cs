using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;
using Engine.Entities;

namespace Engine.Types {
    /// <summary>
    /// A recipe for creating an Amount of T, and the Resource R needed in the ingredients list
    /// </summary>
    /// <typeparam name="I">The type for the ingredients</typeparam>
    /// <typeparam name="P">The type to be produced</typeparam>
    public class Recipe<I, P> {
        /// <summary>
        /// List of ingredients identifiers as the id, and the quantity as the value
        /// </summary>
        /// I
        public List<Ingredient<I>> Ingredients {
            get; set;
        }

        /// <summary>
        /// List of research requirements to create the resource
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
                Ingredients.ForEach(ing => prog.Absorb(ing.Progress));
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
        public Recipe(IEnumerable<Ingredient<I>> ings, IEnumerable<Skill> resReqs, Quantified<P> prods) {
            Ingredients = new List<Ingredient<I>>();
            ForEach(ings, ing => Ingredients.Add(new Ingredient<I>(ing)));
            Perform(resReqs != null, () => ResearchRequirements = new List<Skill>(resReqs));
            Produces = prods;
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="ings">The Ingredient collection (really more of a blueprint)</param>
        /// <param name="resReqs">The research requirements</param>
        /// <param name="produces">What is produced</param>
        /// <param name="quantity">How much is produced</param>
        public Recipe(IEnumerable<Ingredient<I>> ings, IEnumerable<Skill> resReqs, P produces, uint quantity)
            : this(ings, resReqs, new Quantified<P> (produces, quantity)) { }

        /// <summary>
        /// Clone a recipe. Good for resetting progress.
        /// </summary>
        /// <param name="rec"></param>
        public Recipe(Recipe<I, P> rec)
            : this(rec.Ingredients, rec.ResearchRequirements, rec.Produces.Contents, rec.Produces.Quantity) 
            => DoNothing( );

        /// <summary>
        /// Does a citizen meet the requirements of a recipe
        /// </summary>
        /// <param name="wk">The citizen to test</param>
        /// <returns>Whether or not the citizen meets the work requirements</returns>
        public bool MeetsRequirements(Citizen wk) => ResearchRequirements == null || ResearchRequirements.Count == 0 || ResearchRequirements.TrueForAll((Skill sk) => wk.Skills.Contains(sk));
    }
}
