using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types
{
    /// <summary>
    /// A recipe for creating an Amount of T, and the Resource R needed in the ingredients list
    /// </summary>
    /// <typeparam name="T">The type of resource created</typeparam>
    /// <typeparam name="I">The type of ingredient needed</typeparam>
    public class Recipe<T, I>
    {
        /// <summary>
        /// List of ingredients identifiers as the id, and the quantity as the value
        /// </summary>
        public List<Ingredient<I>> Ingredients { get; set; }

        /// <summary>
        /// List of research requirement identifiers required for
        /// </summary>
        public List<uint> ResearchRequirements { get; set; }

        /// <summary>
        /// Starts at 0, works its way up to max. Once max, produces T
        /// </summary>
        public Bank Progress {
            get {
                Bank prog = new Bank();
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
        public Quantified<T> Produces { get; set; }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="ings">The Ingredient collection (really more of a blueprint)</param>
        /// <param name="resReqs">The research requirements</param>
        /// <param name="prods">What is produced by the recipe, and how much</param>
        public Recipe(List<Ingredient<I>> ings, List<uint> resReqs, Quantified<T> prods)
        {
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
        public Recipe(List<Ingredient<I>> ings, List<uint> resReqs, T produces, uint quantity) 
            : this(ings, resReqs, new Quantified<T> { Contents = produces, Quantity = quantity }) { }
    }
}
