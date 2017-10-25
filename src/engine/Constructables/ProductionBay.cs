using System;
using System.Collections.Generic;
using Engine.Exceptions;
using Engine.Types;
using static LangRoids;
using static System.Math;
using Engine.Entities;
using Engine;
using Engine.Interfaces;

namespace Engine.Constructables {
    /// <summary>
    /// A bay that produces a resource from a recipe of resources.
    /// </summary>
    public class ProductionBay : PoweredBay {
        /// <summary>
        /// The recipes supported by the production bay
        /// </summary>
        public List<Recipe<Resource, Resource>> SupportedRecipes {
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
        public RegeneratingBank Reserve {
            get; private set;
        }

        /// <summary>
        /// The active energy pool to be used during crafting
        /// </summary>
        public RegeneratingBank Pool {
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
        private CappedList<ProductionBaySlot> Stations {
            get; set;
        }

        /// <summary>
        /// The maximum amount of ongoing recipes that are processed
        /// </summary>
        public int ProductionSlotCount => Stations.Count;

        /// <summary>
        /// Create a new production bay
        /// </summary>
        /// <param name="recs">The list of recipes that are supported by the bay</param>
        /// <param name="prodStations">The (Max, Start) tuple for how many ProductionBaySlots should be in the bay</param>
        /// <param name="pool">The (Max, Start) tuple for how big the bay's energy pool should be</param>
        /// <param name="resv">The (Max, Start) tuple for how big the bay's reserve pool should be</param>
        /// <param name="cargoCapacity">The maximum amount of cargo supported by the bay</param>
        /// <param name="energyGrid">The available power sources provided by the grid</param>
        /// <param name="maxEnergyDraw">The maximum amount of energy this production bay is allowed to pull from the grid</param>
        /// <remarks>Note: If your "start" is ever bigger than the max, the max will be the start</remarks>
        public ProductionBay(List<Recipe<Resource, Resource>> recs, 
                (uint max, uint start) prodStations, 
                (uint max, uint start) pool, 
                (uint max, uint start) resv, 
                uint cargoCapacity,
                List<IPowerSource> energyGrid,
                uint maxEnergyDraw) : base(energyGrid, maxEnergyDraw) {
            Stations = new CappedList<ProductionBaySlot>(Min(prodStations.start, prodStations.max));
            Pool = new RegeneratingBank {
                Maximum = pool.max,
                Quantity = pool.start
            };
            Reserve = new RegeneratingBank {
                Maximum = resv.max,
                Quantity = resv.start
            };
            Resources = new ResourceBank(cargoCapacity);

            // Create the slots
            for (int i = 0 ; i < Math.Min(prodStations.start, prodStations.max) ; i++) {
                Stations.Add(new ProductionBaySlot(Pool, Reserve, Resources, 0));
            }
            SupportedRecipes = new List<Recipe<Resource, Resource>>(recs);
            EnergySources = energyGrid;
            EnergyMaxDraw = maxEnergyDraw;
            EnergySwitch = true;
        }

        /// <summary>
        /// Add a new station to the bay
        /// </summary>
        /// <param name="onLimitMet">What to do instead of throwing LimitMetException</param>
        /// <param name="seats">How many people are allowed in the production station</param>
        public void AddProductionStation(uint seats, Action onLimitMet = null) => Perform(ProductionSlotCount == Stations.Limit,
                (onLimitMet, new LimitMetException( )), () => Stations.Add(new ProductionBaySlot(Pool, Reserve, Resources, seats)));

        /// <summary>
        /// Destroy the production station at the slot indicated
        /// </summary>
        /// <param name="slot"></param>
        public void DestroyProductionStation(int slot) => Perform(slot > 0 && slot <= Stations.Count, () => Stations.RemoveAt(slot));

        /// <summary>
        /// Craft a recipe at the first available slot, or the one with the least lineup.
        /// </summary>
        /// <param name="rec">The recipe to craft</param>
        /// <exception cref="UnsupportedException"></exception>
        public void Craft(Recipe<Resource, Resource> rec) => Craft(rec, FirstAvailableStation( ));

        /// <summary>
        /// Crafts a recipe at the slot indicated.
        /// </summary>
        /// <param name="rec">The recipe to craft</param>
        /// <param name="slot">The slot to craft at</param>
        /// <param name="onUnsupportedRecipe">What to do if the recipe is not supported, instead of throwing an error</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when slot is not a valid index</exception>
        /// <exception cref="UnsupportedException">Thrown when the recipe is not supported by the bay</exception>
        public void Craft(Recipe<Resource, Resource> rec, int slot, Action onUnsupportedRecipe = null) => Perform(
            (slot >= 0 && slot < Stations.Count, new IndexOutOfRangeException( )),
            (SupportedRecipes.Contains(rec), onUnsupportedRecipe, new UnsupportedException( )),
            () => Stations[slot].ActivateRecipe(rec));

        /// <summary>
        /// Gets the first one that is not being used, or the one with the smallest lineup
        /// </summary>
        /// <returns>The index of the station</returns>
        public int FirstAvailableStation() {
            int lineup = int.MaxValue;
            int index = -1;
            Repeat(Stations.Count, i => {
                var slot = Stations[i];
                if (slot.Active == null || slot.Lineup.Count == 0) {
                    index = i;
                    return;
                }
                if (lineup > slot.Lineup.Count) {
                    index = i;
                    lineup = slot.Lineup.Count;
                }
            });
            return index;
        }

        /// <summary>
        /// Move a worker from one station to another
        /// </summary>
        /// <param name="wk">The worker to move</param>
        /// <param name="from">What station the worker is at</param>
        /// <param name="to">What station the worker is to move to</param>
        /// <param name="onWorkerNotInFrom">What to do when the worker is not actually in the "from" production bay slot</param>
        /// <param name="toLimitBreached">What to do when the limit is reached in the to production bay slot</param>
        public void TransferWorkerBetweenStations(Citizen wk, ProductionBaySlot from, ProductionBaySlot to, Action onWorkerNotInFrom = null, Action toLimitBreached = null) => Perform(from.Workers.Contains(wk),
            (onWorkerNotInFrom, new KeyNotFoundException("Worker not found in from")), () => {
                from.RemoveWorker(wk);
                to.AddWorker(wk, toLimitBreached);
            });

        /// <summary>
        /// Draw on the energy grid for more power
        /// </summary>
        public void RegeneratePower() => Perform(!Pool.IsFull || !Reserve.IsFull, () => {
            var reserveNeeds = (Reserve.Maximum - Reserve.Quantity);
            var poolNeeds = (Pool.Maximum - Pool.Quantity);
            (poolNeeds != 0 ? Pool : Reserve).Quantity += DrawEnergy(Min(poolNeeds != 0 ? poolNeeds : reserveNeeds, EnergyMaxDraw), DoNothing);
        });

        /// <summary>
        /// Call think on all of the stations
        /// </summary>
        private void ThinkAll() => Stations.ForEach(bay => bay.Think( ));

        /// <summary>
        /// ThinkAll and regenerate power on every cycle.
        /// </summary>
        public override void Think() => Compose(ThinkAll, RegeneratePower);

    }
}
