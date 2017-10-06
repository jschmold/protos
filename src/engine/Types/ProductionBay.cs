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

        /// <summary>
        /// Create a new production bay
        /// </summary>
        /// <param name="loc">Where the bay is located</param>
        /// <param name="occupantLimit">The maximum amount of people permitted in the bay (including workers)</param>
        /// <param name="recs">The list of recipes that are supported by the bay</param>
        /// <param name="prodStations">The (Max, Start) tuple for how many ProductionBaySlots should be in the bay</param>
        /// <param name="pool">The (Max, Start) tuple for how big the bay's energy pool should be</param>
        /// <param name="resv">The (Max, Start) tuple for how big the bay's reserve pool should be</param>
        /// <param name="cargoCapacity">The maximum amount of cargo supported by the bay</param>
        /// <remarks>Note: If your "start" is ever bigger than the max, the max will be the start</remarks>
        public ProductionBay(Location loc, uint occupantLimit, List<Recipe> recs, (uint max, uint start) prodStations, (uint max, uint start) pool, (uint max, uint start) resv, uint cargoCapacity) : base(loc, occupantLimit) {
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
            for (int i = 0 ; i < Math.Min(prodStations.start, prodStations.max) ; i++) {
                ProductionSlots.Add(new ProductionBaySlot(EnergyPool, EnergyReserve, Resources, 0));
            }
            SupportedRecipes = new List<Recipe>(recs);
        }

        /// <summary>
        /// Add a new station to the bay
        /// </summary>
        /// <param name="onLimitMet">What to do instead of throwing LimitMetException</param>
        public void AddProductionStation(uint seats, Action onLimitMet = null) =>
            Perform(ProductionSlotCount == MaxProductionSlots,
                () => DoOrThrow(onLimitMet, new LimitMetException( )),
                () => ProductionSlots.Add(new ProductionBaySlot(EnergyPool, EnergyReserve, Resources, seats)));

        //public void AddProductionStation(uint seats, Action onLimitMet = null) {
        //    if (ProductionSlotCount == MaxProductionSlots) {
        //        DoOrThrow(onLimitMet, new LimitMetException( ));
        //    }
        //    ProductionSlots.Add(new ProductionBaySlot(EnergyPool, EnergyReserve, Resources, seats));
        //}

        /// <summary>
        /// Destroy the production station at the slot indicated
        /// </summary>
        /// <param name="slot"></param>
        public void DestroyProductionStation(int slot) => Perform(slot > 0 && slot <= ProductionSlots.Count, () => ProductionSlots.RemoveAt(slot));

        /// <summary>
        /// Craft a recipe at the first available slot, or the one with the least lineup.
        /// </summary>
        /// <param name="rec">The recipe to craft</param>
        /// <exception cref="UnsupportedRecipeException"></exception>
        public void Craft(Recipe rec) => Craft(rec, FirstAvailableStation( ));

        /// <summary>
        /// Crafts a recipe at the slot indicated.
        /// </summary>
        /// <param name="rec">The recipe to craft</param>
        /// <param name="slot">The slot to craft at</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when slot is not a valid index</exception>
        /// <exception cref="UnsupportedRecipeException">Thrown when the recipe is not supported by the bay</exception>
        public void Craft(Recipe rec, int slot, Action onUnsupportedRecipe = null) {
            if (slot < 0) {
                throw new IndexOutOfRangeException("slot");
            }
            if (!SupportedRecipes.Contains(rec)) {
                DoOrThrow(onUnsupportedRecipe, new UnsupportedRecipeException( ));
            }
            ProductionSlots[slot].ActivateRecipe(rec);
        }

        /// <summary>
        /// Gets the first one that is not being used, or the one with the smallest lineup
        /// </summary>
        /// <returns>The index of the station</returns>
        public int FirstAvailableStation() {
            int lineup = int.MaxValue;
            int index = -1;
            for (int i = 0 ; i < ProductionSlots.Count ; i++) {
                var slot = ProductionSlots[i];
                if (slot.Active == null || slot.Lineup.Count == 0) {
                    index = i;
                    break;
                }
                if (lineup > slot.Lineup.Count) {
                    index = i;
                    lineup = slot.Lineup.Count;
                }
            }
            return index;
        }

        /// <summary>
        /// Move a worker from one station to another
        /// </summary>
        /// <param name="wk">The worker to move</param>
        /// <param name="from">What station the worker is at</param>
        /// <param name="to">What station the worker is to move to</param>
        public void TransferWorkerBetweenStations(Citizen wk, ProductionBaySlot from, ProductionBaySlot to) {
            from.Workers.Remove(wk);
            to.AddWorker(wk);
        }

    }
}
