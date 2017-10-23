using System;
using System.Collections.Generic;
using System.Text;
using Engine.Constructables;
using Engine.Interfaces;
using Engine.Exceptions;
using static System.MathF;
using Engine.Entities;


namespace Engine.Types {
    /**
     *  Needs a recipe
     *      - Enhance by having quantity of worker required, and the requirement.
     *          eg: 5 workers with X outfitting, 2 workers of Y skill, 1 worker of Z skill, 
     */
    public class Blueprint<T> : Recipe<Resource, T> where T : Bay {
        /// <summary>
        /// Create a new blueprint
        /// </summary>
        /// <param name="Ingredients"><see cref="Recipe{I, P}.Ingredients"/></param>
        /// <param name="SkillReqs"><see cref="SkillRequiremenets"/></param>
        /// <param name="outfitReqs"><see cref="OutfittingRequirements"/></param>
        /// <param name="Bay"><see cref="Recipe{I, P}.Produces"/></param>
        /// <param name="CleanupTime"><see cref="CleanupTime"/></param>
        /// <param name="MinimumWorkers"><see cref="MinimumWorkers"/></param>
        public Blueprint(IEnumerable<Ingredient<Resource>> Ingredients,
            IEnumerable<Skill> SkillReqs,
            List<object> outfitReqs,
            T Bay, uint CleanupTime,
            uint MinimumWorkers) : base(Ingredients, SkillReqs, Bay, 1) {

        }
        /// <summary>
        /// How many frames it will take to clean up and ready the created bay
        /// </summary>
        public uint CleanupTime {
            get; set;
        }
        /// <summary>
        /// The minimum amount of workers required to make this work
        /// </summary>
        public uint MinimumWorkers {
            get; set;
        }
        
        /// <summary>
        /// The required equippables to complete the job
        /// </summary>
        public List<Equippable> OutfittingRequirements {
            get; set;
        }
        /// <summary>
        /// The required skills to complete the job
        /// </summary>
        public List<Skill> SkillRequiremenets {
            get;set;
        }
    }
}
