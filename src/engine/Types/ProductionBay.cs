using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Engine.Exceptions;

namespace Engine.Types {
    /// <summary>
    /// A bay that produces a resource from a recipe of resources.
    /// </summary>

    public class ProductionBay<T> : Bay {
        public ProductionBay(Location loc, uint occLimit) : base(loc, occLimit) {
        }

        /// <summary>
        /// The recipes supported by the production bay
        /// </summary>
        public List<Recipe> SupportedRecipes {
            get; set;
        }

        /// <summary>
        /// Best to set this to ProductionSlotQuantity 
        /// </summary>
        public ResourceBank Resources {
            get; private set;
        }

        /// <summary>
        /// The reserve of energy when the pool runs out.
        /// </summary>
        public RegeneratingBank EnergyReserve {
            get; private set;
        }

        /// <summary>
        /// The active energy pool to be used during crafting
        /// </summary>
        public RegeneratingBank EnergyPool {
            get; private set;
        }

        /// <summary>
        /// Is the grid feeding the energy pool and/or reserve?
        /// </summary>
        public bool GridFeed {
            get; set;
        }

        /// <summary>
        /// A bank containing recipes, where the max is the maximum amount of slots  
        /// </summary>
        public List<ProductionBaySlot> ProductionSlots {
            get; set;
        }

        /// <summary>
        /// The maximum amount of ongoing recipes that are processed
        /// </summary>
        public uint ProductionSlotCount { get; set; } = 1;
    }
}
