using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public class Ingredient<T> {
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

        public bool IsComplete => Progress.Quantity == Progress.Maximum;

        public Ingredient(Ingredient<T> ing) : this(ing.Requirement, ing.Progress.Maximum, ing.WorkerCost, ing.StationCost) { }

        public Ingredient(Quantified<T> req, uint WorkerTotalCost, uint WorkerFrameCost, uint StationFrameCost) {
            Requirement = req;
            WorkerCost = WorkerFrameCost;
            StationCost = StationFrameCost;
            Progress = new Bank { Quantity = 0, Maximum = WorkerTotalCost };
        }

        public void Process(uint workers) => Progress.Quantity += WorkerCost * workers;
    }
}
