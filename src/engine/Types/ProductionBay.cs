using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Engine.Exceptions;
using static Engine.Utilities.LangHelpers;

namespace Engine.Types {
    /// <summary>
    /// A bay that produces a resource from a recipe of resources.
    /// </summary>
    public class ProductionBay : Bay {
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
        private List<ProductionBaySlot> ProductionSlots {
            get; set;
        }

        /// <summary>
        /// The maximum amount of ongoing recipes that are processed
        /// </summary>
        public int ProductionSlotCount => ProductionSlots.Count;
        /// <summary>
        /// The maximum amount of production slots permitted
        /// </summary>
        private uint MaxProductionSlots {
            get; set;
        }


        public ProductionBay(Location loc, uint occupantLimit, List<Recipe> recs, (uint max, uint start) prodStations, (uint max, uint start) pool, (uint max, uint start) resv, uint cargoCapacity, List<Recipe> supportedRecs) : base(loc, occupantLimit) {
            SupportedRecipes = recs;
            MaxProductionSlots = prodStations.max;
            EnergyPool = new RegeneratingBank {
                Maximum = pool.max,
                Quantity = pool.start
            };
            EnergyReserve = new RegeneratingBank {
                Maximum = resv.max,
                Quantity = resv.start
            };
            Resources = new ResourceBank(cargoCapacity);

            // Create the slots
            for (int i = 0 ; i < prodStations.start ; i++) {
                ProductionSlots.Add(new ProductionBaySlot(EnergyPool, EnergyReserve, Resources));
            }
            SupportedRecipes = new List<Recipe>( );
            supportedRecs.ForEach(SupportedRecipes.Add);
        }

        public void AddProductionStation(Action onLimitMet = null) {
            if (ProductionSlotCount == MaxProductionSlots) {
                DoOrThrow(onLimitMet, new LimitMetException( ));
            }

        }
    }
}
