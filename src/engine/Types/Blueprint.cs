using System;
using System.Collections.Generic;
using System.Text;
using Engine.Constructables;
using Engine.Interfaces;
using Engine.Exceptions;
using static System.MathF;
using static Engine.Helpers.Lang;

namespace Engine.Types {
    /**
     *  Needs a recipe
     *    - cleanup time
     *    - minimum workers
     *    - knowledge requirements
     *    - outfitting requirements
     */
    public class Blueprint<T> : Recipe<Resource, T> where T : Bay {
        public Blueprint(IEnumerable<Ingredient<Resource>> Ingredients,
            IEnumerable<Skill> SkillReqs,
            List<object> outfitReqs,
            T Bay, uint CleanupTime,
            uint MinimumWorkers) : base(Ingredients, SkillReqs, Bay, 1) {

        }
        public uint CleanupTime {
            get; set;
        }
        /// <summary>
        /// The minimum amount of workers required to make this work
        /// </summary>
        public uint MinimumWorkers {
            get; set;
        }
        // Todo: Make outfits
        public List<object> OutfittingRequirements {
            get; set;
        }
    }
}
