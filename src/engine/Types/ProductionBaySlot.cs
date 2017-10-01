using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Engine.Exceptions;

namespace Engine.Types {

    /// <summary>
    /// Needs to pull from cargo location
    /// </summary>
    public class ProductionBaySlot {
        /// <summary>
        /// The recipe currently being worked on
        /// </summary>
        public Recipe Active {
            get; set;
        }

        /// <summary>
        /// The lineup of recipes to work on
        /// </summary>
        public List<Recipe> Lineup {
            get; set;
        } = new List<Recipe>( );
        /// <summary>
        /// The workers dedicated to this BayStation
        /// </summary>
        public List<Citizen> Workers {
            get; set;
        }

        /// <summary>
        /// A dictionary of workers and what they are working on (one ingredient at a time). Makes finding who is doing what easier.
        /// </summary>
        private Dictionary<Citizen, Ingredient<Resource>> WorkPairings;

        /// <summary>
        /// For proper behavior, this must NOT be given its own energy pool unless that is the desired effect. 
        /// If you pass in the Bay's energy (as it is intended), modifying the local variable here also modifies the bay energy.
        /// </summary>
        public RegeneratingBank Pool {
            get; set;
        }
        /// <summary>
        /// For proper behavior, this must NOT be given its own energy pool unless that is the desired effect.
        /// If you pass in the Bay's reserve (as it is intended), modifying the local variable here also modifies the bay energy.
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

        public ProductionBaySlot(RegeneratingBank pool, RegeneratingBank reserve, ResourceBank resources) {
            Pool = pool;
            Reserve = reserve;
            Resources = resources;
            WorkPairings = new Dictionary<Citizen, Ingredient<Resource>>( );
        }

        /// <summary>
        /// Takes an ingredient from the resources repo and expends it.
        /// </summary>
        /// <param name="res">The Ingredient with the resource requirement</param>
        /// <param name="onLackingResource">What to do instead of throwing LackingResourceException</param>
        /// <exception cref="LackingResourceException"></exception>
        public void ExpendIngredient(Ingredient<Resource> res, Action onLackingResource = null) {
            if (res == null) {
                throw new ArgumentNullException("res");
            }
            Resource expend = res.Requirement.Contents;
            if (!Resources.Contains(expend) || res.Requirement.Quantity > Resources[expend].Quantity) {
                if (onLackingResource == null) {
                    throw new LackingResourceException( );
                } else {
                    onLackingResource.Invoke( );
                    return;
                }
            }
            Resources[expend].Quantity -= res.Requirement;
        }

        /// <summary>
        /// Expends energy from either the Pool or the Reserve. 
        /// Expends the remainder of the pool if it is not completely empty, then the rest from the reserve.
        /// </summary>
        /// <param name="amt">The amount to expend</param>
        /// <param name="onNotEnoughEnergy">What to do instead of throwing an exception if there's not enough energy.</param>
        public void ExpendEnergy(uint amt, Action onNotEnoughEnergy = null) {
            if (!Reserve.HasEnoughFor(amt) && !Pool.HasEnoughFor(amt)) {
                if (onNotEnoughEnergy == null) {
                    throw new NotEnoughEnergyException( );
                } else {
                    onNotEnoughEnergy.Invoke( );
                    return;
                }
            }
            int result = (int)(Pool - amt);
            Pool.Quantity -= Math.Min(Pool.Quantity, amt);
            Reserve.Quantity += (uint)(result < 0 ? result : 0);
        }

        /// <summary>
        /// Clears the active recipe and clears WorkPairings
        /// </summary>
        public void ClearActive() {
            Active = null;
            WorkPairings = new Dictionary<Citizen, Ingredient<Resource>>( );
        }

        /// <summary>
        /// Clears the active recipe, and sets the rec to the active recipe.
        /// </summary>
        /// <param name="rec">The recipe to activate for crafting</param>
        public void ActivateRecipe(Recipe rec) {
            if (Active != null) {
                ClearActive( );
            }
            Active = new Recipe(rec);
        }

        /// <summary>
        /// Clears the active recipe and sets the recipe at Lineup[<paramref name="index"/>] as active.
        /// Once Lineup[<paramref name="index"/>] is activated, it is removed from the lineup.
        /// </summary>
        /// <param name="index">The index to retrieve from the lineup</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void ActivateRecipe(int index) {
            if (Lineup.Count < (index + 1)) {
                throw new IndexOutOfRangeException( );
            }
            ActivateRecipe(Lineup[index]);
            Lineup.RemoveAt(index);
        }

        /// <summary>
        /// Creates the new resource in the Resources repo and calls ClearActive.
        /// Throws NotYetCompletedException if Active is not yet done
        /// </summary>
        /// <exception cref="NotYetCompletedException"></exception>
        public void FinishRecipe(Action onNotYetCompleted = null, Action onActiveIsNull = null) {
            if (Active == null) {
                onActiveIsNull?.Invoke( );
                return;
            }
            if (!Active.Progress.IsFull) {
                if (onNotYetCompleted != null) {
                    onNotYetCompleted.Invoke( );
                    return;
                } else {
                    throw new NotYetCompletedException( );
                }
            }
            Resources.Add(Active.Produces.Contents, Active.Produces.Quantity);
            ClearActive( );
        }

        /// <summary>
        /// The main function that is called on every loop to process this slot's functionality.
        /// Increments progress on all WorkPairings.
        /// </summary>
        public void Think() {
            ManageWorkers( );
            ManageProduction( );
        }

        /// <summary>
        /// Gives workers a rest if their energy is not sufficient for the ingredient they're working on.
        /// Workers that have enough energy to work and are not working are put to work.
        /// </summary>
        public void ManageWorkers() => Workers.ForEach(wk => {
            if (!WorkPairings.ContainsKey(wk) && wk.IsRested) {
                WorkPairings.Add(wk, null);
            } else if (WorkPairings.ContainsKey(wk) && wk.NeedsRest) {
                WorkPairings.Remove(wk);
            }
        });

        /// <summary>
        /// Finish anything that is completed, and progress anything being worked on. Queue up the next thing if Active was finished.
        /// Otherwise, continue working on things.
        /// </summary>
        public void ManageProduction() {
            // Is there a reason to do computation?
            if ((Active == null && Lineup.Count == 0 ) || Workers.Count == 0) {
                return;
            }
            // Is what we are doing done? If so, finish it.
            if (Active.Progress.IsFull) {
                FinishRecipe( );
                if (Lineup.Count > 0) {
                    ActivateRecipe(0);
                }
                return;
            }
            for (int num = 0 ; num < WorkPairings.Count ; num++) {
                (Citizen worker, Ingredient<Resource> ingredient) = (WorkPairings.ElementAt(num));
                if (ingredient == null || ingredient.IsComplete) {
                    var next = NextAvailableIngredient( );
                    if (next == null) {
                        continue;
                    }
                    WorkPairings[worker] = next;
                    ExpendIngredient(next);
                    continue;
                }
                try {
                    ExpendEnergy(ingredient.StationCost);
                    worker.Energy.Quantity -= ingredient.WorkerCost;
                    ingredient.Process(1);
                } catch (NotEnoughEnergyException) {
                    return;
                }
            }
        }
        /// <summary>
        /// Get the next ingredient that is not being worked on and not completed.
        /// </summary>
        /// <returns></returns>
        private Ingredient<Resource> NextAvailableIngredient() {
            if (Active == null) {
                return null;
            }

            foreach (var ingr in Active.Ingredients) {
                if (!WorkPairings.ContainsValue(ingr)) {
                    return ingr;
                }
            }
            return null;
        }

    }
}
