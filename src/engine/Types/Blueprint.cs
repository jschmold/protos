using System;
using System.Collections.Generic;
using System.Text;
using Engine.Constructables;
using Engine.Math;
using Engine.Exceptions;
using static System.Math;
using Engine.Entities;
using static LangRoids;

namespace Engine {
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
        /// <param name="outfitReqs"><see cref="OutfittingRequirements"/></param>
        /// <param name="Bay"><see cref="Recipe{I, P}.Produces"/></param>
        /// <param name="cleanTime"><see cref="CleanupTime"/></param>
        /// <param name="MinimumWorkers"><see cref="MinimumWorkers"/></param>
        /// <param name="SkillReqs"><see cref="SkillRequirements"/></param>
        /// <param name="resReqs"><see cref="Recipe{I, P}.ResearchRequirements"/></param>
        public Blueprint(IEnumerable<Ingredient<Resource>> Ingredients,
            IEnumerable<Skill> SkillReqs,
            IEnumerable<Skill> resReqs,
            List<Equippable> outfitReqs,
            T Bay,
            uint cleanTime,
            uint MinimumWorkers)
            : base(Ingredients, resReqs, Bay, 1)
            => CleanupTime = cleanTime;

        /// <summary>
        /// The location the bay is to be created at
        /// </summary>
        public Bound3d Location => Produces.Contents.Location;

        /// <summary>
        /// Clone a blueprint. Good for ensuring the progress isn't being meddled with.
        /// </summary>
        /// <param name="bp"></param>
        public Blueprint(Blueprint<T> bp) 
            : this(bp.Ingredients, bp.SkillRequirements, bp.ResearchRequirements, bp.OutfittingRequirements, bp.Produces.Contents, bp.CleanupTime, bp.MinimumWorkers) 
            => DoNothing( );
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
        /// The skills required by every worker to complete the task
        /// </summary>
        public List<Skill> SkillRequirements {
            get;set;
        }
    }
}
