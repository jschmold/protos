using System;
using System.Collections.Generic;
using Engine.Exceptions;
using Engine.Types;
using static Engine.Helpers.Lang;
using static System.Math;
using Engine;
using Engine.Interfaces;

namespace Engine.Constructables {
    /// <summary>
    /// A bay that produces a resource from a recipe of resources.
    /// </summary>
    public class ProductionBay : Bay, IPowerable {
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

        public List<IPowerSource> EnergySources {
            get;
            set;
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
        public ProductionBay(uint occupantLimit, 
                List<Recipe<Resource, Resource>> recs, 
                (uint max, uint start) prodStations, 
                (uint max, uint start) pool, 
                (uint max, uint start) resv, 
                uint cargoCapacity,
                List<IPowerSource> energyGrid,
                uint maxEnergyDraw) : base(occupantLimit) {
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
        /// <exception cref="IndexOutOfRangeException">Thrown when slot is not a valid index</exception>
        /// <exception cref="UnsupportedException">Thrown when the recipe is not supported by the bay</exception>
        public void Craft(Recipe<Resource, Resource> rec, int slot, Action onUnsupportedRecipe = null) => Perform(slot > 0 && slot < Stations.Count - 1 && SupportedRecipes.Contains(rec),
            () => {
                ThrowIf(slot < 0 || slot >= Stations.Count, new IndexOutOfRangeException( ));
                DoOrThrow(!SupportedRecipes.Contains(rec), onUnsupportedRecipe, new UnsupportedException( ));
            },
            () => Stations[slot].ActivateRecipe(rec));

        /// <summary>
        /// Gets the first one that is not being used, or the one with the smallest lineup
        /// </summary>
        /// <returns>The index of the station</returns>
        public int FirstAvailableStation() {
            int lineup = int.MaxValue;
            int index = -1;
            for (int i = 0 ; i < Stations.Count ; i++) {
                var slot = Stations[i];
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
        public void TransferWorkerBetweenStations(Citizen wk, ProductionBaySlot from, ProductionBaySlot to, Action onWorkerNotInFrom = null, Action toLimitBreached = null) => Perform(from.Workers.Contains(wk),
            (onWorkerNotInFrom, new KeyNotFoundException("Worker not found in from")), () => {
                from.RemoveWorker(wk);
                to.AddWorker(wk, toLimitBreached);
            });

        public void RegeneratePower() => Perform(!Pool.IsFull || !Reserve.IsFull, () => {
            var reserveNeeds = (Reserve.Maximum - Reserve.Quantity);
            var poolNeeds = (Pool.Maximum - Pool.Quantity);
            (poolNeeds != 0 ? Pool : Reserve).Quantity += DrawEnergy(poolNeeds != 0 ? poolNeeds : reserveNeeds, DoNothing);
        });

        private void ThinkAll() => Stations.ForEach(bay => bay.Think( ));

        public override void Think() => Compose(ThinkAll, RegeneratePower);

        /// <summary>
        /// Draw either the amount requested or EnergyMaxDraw, whichever is smaller, from the energy source selected.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="energySource"></param>
        /// <param name="onNotEnoughEnergy"></param>
        /// <returns>The energy expended</returns>
        public uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null) {
            if (!EnergySwitch) {
                return 0;
            }
            if (energySource == null || energySource.PowerAvailable < amt) {
                DoOrThrow(onNotEnoughEnergy, new NotEnoughEnergyException( ));
                return 0;
            }
            uint drawamt = Min(amt, EnergyMaxDraw);
            energySource.ExpendEnergy(drawamt);
            return drawamt;
        }
        /// <summary>
        /// Draw energy from the first source with enough energy.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="onNotEnoughEnergy"></param>
        /// <returns></returns>
        public uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null) => DrawEnergy(amt, EnergySources.Find(src => src.PowerAvailable >= amt), onNotEnoughEnergy);
    }
}
