using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Engine.Exceptions;

namespace Engine.Types {
    public class ProductionBayStation {
        /// <summary>
        /// The recipe currently being worked on
        /// </summary>
        public Recipe Active {
            get; set;
        }

        /// <summary>
        /// The lineup of recipes to work on
        /// </summary>
        public Queue<Recipe> Lineup {
            get; set;
        }
        /// <summary>
        /// The workers dedicated to this BayStation
        /// </summary>
        public List<Worker> Workers {
            get; set;
        }

        /// <summary>
        /// A dictionary of workers and what they are working on (one ingredient at a time)
        /// </summary>
        private Dictionary<Worker, Ingredient<Resource>> WorkPairings;

        /// <summary>
        /// For proper behavior, this must NOT be given its own energy pool unless that is the desired effect. 
        /// If you pass in the Bay's energy (as it is intended), modifying the local variable here also modifies the bay energy.
        /// </summary>
        public RegeneratingBank Pool {
            get; set;
        }
        /// <summary>
        /// For proper behavior, this must NOT be given its own energy pool unless that is the desired effect.
        /// If you pass in the Bay'sreserve (as it is intended), modifying the local variable here also modifies the bay energy.
        /// </summary>
        public RegeneratingBank Reserve {
            get; set;
        }

        /// <summary>
        /// Set this to the ResourceBank to be drawn from for resources for every ingredient.
        /// </summary>
        public ResourceBank Resources {
            get; set;
        }

        /// <summary>
        /// The maximum amount of workers that can tend the station
        /// </summary>
        public uint WorkerSeats {
            get; set;
        }

        public ProductionBayStation(RegeneratingBank pool, RegeneratingBank res) {
            Pool = pool;
            Reserve = res;
            WorkPairings = new Dictionary<Worker, Ingredient<Resource>>( );
        }

        /// <summary>
        /// 1 Worker per ingredient
        /// </summary>
        public void Process() {
            // Ensure all workers are in the WorkPairings if they have energy
            Workers.ForEach(wk => {
                if (WorkPairings.ContainsKey(wk)) {
                    return;
                }
                if (wk.Energy > 0) {
                    WorkPairings.Add(wk, null);
                }
            });
            // For those currently working, keep working
            // For those who just finished, give them something new
            foreach (KeyValuePair<Worker, Ingredient<Resource>> WorkerIng in WorkPairings) {
                var Worker = WorkerIng.Key;
                var WorkingIngredient = WorkerIng.Value;
                // If done, give them something else
                if (WorkingIngredient.IsComplete) {
                    WorkingIngredient = NextAvailableIngredient();
                }
                // If they have just finished, but there's nothing left, this will get called
                if (WorkingIngredient == null) {
                    // Is there another ingredient to work on?
                    WorkingIngredient = NextAvailableIngredient( );
                    // If no, then just remove the worker from the work pairings so they can do whatever
                    // Call continue to avoid "working" on non-existent stuff
                    if (WorkingIngredient == null) {
                        GiveWorkerBreak(Worker);
                        continue;
                    }
                }
                // This now ASSUMES there's something to work on
                if (Worker.Energy.HasEnoughFor(WorkingIngredient.WorkerCost)) {
                    // Take all that energy bruh
                    Worker.Energy.Quantity = Worker.Energy.Quantity - WorkingIngredient.WorkerCost;
                    WorkingIngredient.Progress.Quantity += WorkingIngredient.WorkerCost;
                    ExpendEnergy(WorkingIngredient.StationCost);
                } else {
                    // There's nothing to work on, or the worker lacks the energy to work on it
                    GiveWorkerBreak(Worker);
                }
            }
        }

        /// <summary>
        /// Get the next available ingredient to work on for a worker
        /// </summary>
        /// <returns>An ingredient</returns>
        private Ingredient<Resource> NextAvailableIngredient() {
            foreach (var ing in Active.Ingredients) {
                if (!ing.IsComplete || !WorkPairings.ContainsValue(ing)) {
                    return ing;
                }
            }
            return null;
        }

        /// <summary>
        /// Expend energy from either the Pool or the Reserve.
        /// </summary>
        /// <param name="amt">The amount to expend</param>
        /// <param name="onFailure">What to do instead of throwing an exception if there's not enough energy</param>
        private void ExpendEnergy(uint amt, Action onFailure = null) {
            if (!Reserve.HasEnoughFor(amt) && !Reserve.HasEnoughFor(amt)) {
                if (onFailure == null) {
                    throw new NotEnoughEnergyException( );
                } else {
                    onFailure.Invoke( );
                    return;
                }
            }
            (Pool >= amt ? Pool : Reserve).Quantity -= amt;
        }

        /// <summary>
        /// Removes the worker from the workpairings and sets the activity to an Inactive Interuptable state
        /// </summary>
        /// <param name="wk">The worker to give a break to</param>
        private void GiveWorkerBreak(Worker wk) {
            WorkPairings.Remove(wk);
            wk.CurrentActivity = new WorkerActivity("Inactive", true);
        }
    }
}
