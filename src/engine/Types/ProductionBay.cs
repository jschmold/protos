using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Types
{
    /// <summary>
    /// A bay that produces C in recipe using R resources.
    /// </summary>
    public class ProductionBay<R, C> : Bay
    {
        /// <summary>
        /// The recipes supported by the production bay
        /// </summary>
        public List<Recipe<R, C>> SupportedRecipes { get; set; }

        /// <summary>
        /// Best to set this to ProductionSlotQuantity 
        /// </summary>
        public ResourceBank Completed { get; private set; }

        /// <summary>
        /// The reserve of energy when the pool runs out.
        /// </summary>
        public RegeneratingBank EnergyReserve { get; private set; }

        /// <summary>
        /// The active energy pool to be used during crafting
        /// </summary>
        public RegeneratingBank EnergyPool { get; private set; }

        /// <summary>
        /// Is the grid feeding the energy pool and/or reserve?
        /// </summary>
        public bool GridFeed { get; set; }

        /// <summary>
        /// Multiple "tasks" as threads acting as production slots with a task quantity limiter for the list. 
        /// A production "slot" is in use when it is null or set to Completed. 
        /// </summary>
        public List<Task> ProductionSlots { get; set; }

        /// <summary>
        /// The amount of concurrent things that can be created at once.
        /// </summary>
        public uint ProductionSlotQuantity { get; set; } = 1;

        /// <summary>
        /// Create a new production bay.
        /// </summary>
        /// <param name="loc">Where the bay is located</param>
        /// <param name="occLimit">The maximum amount of occupants</param>
        /// <param name="recs">The recipes supported by the bay</param>
        /// <param name="reserve">The (Maximum, RegenRate, DecayRate)</param>
        /// <param name="pool">The (Maximum, RegenRate, DecayRate) tuple for the pool</param>
        public ProductionBay(Location loc, 
                             uint occLimit, 
                             List<Recipe<R, C>> recs, 
                             (uint, uint, uint) reserve, 
                             (uint, uint, uint) pool, 
                             bool gridFeed = true) 
            : base(loc, occLimit)
        {
            SupportedRecipes = recs;
            EnergyReserve = new RegeneratingBank
            {
                Maximum = reserve.Item1,
                RegenRate = reserve.Item2,
                DecayRate = reserve.Item3
            };
            
            EnergyPool = new RegeneratingBank
            {
                Maximum = pool.Item1,
                RegenRate = pool.Item2,
                DecayRate = pool.Item3
            };
            GridFeed = gridFeed;
        }

        /// <summary>
        /// Create a new production bay
        /// </summary>
        /// <param name="loc">Where the bay is located</param>
        /// <param name="occLimit">The maximum amount of occupants</param>
        /// <param name="recs">The recipes supported by the bay</param>
        /// <param name="reserve">The RegeneratingBank to be used for the EnergyReserve</param>
        /// <param name="pool">The RegeneratingBank to be used for the EnergyPool</param>
        public ProductionBay(Location loc, 
                             uint occLimit, 
                             List<Recipe<R, C>> recs, 
                             RegeneratingBank reserve, 
                             RegeneratingBank pool)
            : this(loc, occLimit, recs, (reserve.Maximum, reserve.RegenRate, reserve.DecayRate), (pool.Maximum, pool.RegenRate, pool.DecayRate)) { }
    }
}
