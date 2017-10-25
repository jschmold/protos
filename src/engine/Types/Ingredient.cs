using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    /// <summary>
    /// A datatype containing the information required to process the production of a Recipe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Ingredient<T> {
        /// <summary>
        /// The resource to be converted
        /// </summary>
        public Quantified<T> Requirement {
            get; set;
        }
        /// <summary>
        /// Cost per worker per frame
        /// </summary>
        public uint WorkerCost {
            get; set;
        }
        /// <summary>
        /// The cost on the station per frame. This is used to maintain the production process but does not contribute to the progress.
        /// </summary>
        public uint StationCost {
            get; set;
        }

        /// <summary>
        /// Maximum is the "Energy" needing to be expended by workers to consume a single resource
        /// </summary>
        public Bank Progress {
            get; set;
        }

        /// <summary>
        /// Whether or not this ingredient is completed
        /// </summary>
        public bool IsComplete => Progress.Quantity == Progress.Maximum;

        /// <summary>
        /// Clone an ingredient, resetting the progress.
        /// </summary>
        /// <param name="ing"></param>
        public Ingredient(Ingredient<T> ing) : this(ing.Requirement, ing.Progress.Maximum, ing.WorkerCost, ing.StationCost) { }

        /// <summary>
        /// Create a new ingredient
        /// </summary>
        /// <param name="req"></param>
        /// <param name="WorkerTotalCost"></param>
        /// <param name="WorkerFrameCost"></param>
        /// <param name="StationFrameCost"></param>
        public Ingredient(Quantified<T> req, uint WorkerTotalCost, uint WorkerFrameCost, uint StationFrameCost) {
            Requirement = req;
            WorkerCost = WorkerFrameCost;
            StationCost = StationFrameCost;
            Progress = new Bank { Quantity = 0, Maximum = WorkerTotalCost };
        }

        /// <summary>
        /// Process an ingredient by adding WorkerCost * Workers to the Progress
        /// </summary>
        /// <param name="workers"></param>
        public void Process(uint workers) => Progress.Quantity += WorkerCost * workers;
    }
}
