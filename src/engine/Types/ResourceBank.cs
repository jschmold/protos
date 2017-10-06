using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using System.Linq;
using static System.Linq.Enumerable;
using static Engine.Utilities.LangHelpers;

namespace Engine.Types {
    public class ResourceBank : Bank {
        /// <summary>
        /// What is in the resource bank
        /// </summary>
        public List<Quantified<Resource>> Contents {
            get; private set;
        }

        public ResourceBank(uint max) {
            Contents = new List<Quantified<Resource>>( );
            Maximum = max;
            Quantity = 0;
        }
        public ResourceBank(uint max, List<Quantified<Resource>> res) :
            this(max) => Contents = res;
        public ResourceBank(uint max, List<(Resource, uint)> res) :
            this(max) => res.ForEach((cur) => Add(cur.Item1, amt: cur.Item2));

        /// <summary>
        /// Add a resource to the list
        /// </summary>
        /// <param name="res">The resource to add</param>
        /// <param name="onFailure">The function to call when adding fails</param>
        public void Add(Resource res, uint amt = 1, Action onFailure = null) {
            if (amt == 0) {
                return;
            }
            ThrowIf(res == null, new ArgumentNullException("res"));
            Perform(!HasSpaceFor(res.Volume * amt), () => DoOrThrow(onFailure, new NotEnoughCargoSpaceException( )));
            Perform(Contains(res), () => Contents.Add(new Quantified<Resource>(res, amt)), () => this[res].Quantity += amt);
            Quantity += res.Volume * amt;
        }

        /// <summary>
        /// Add a collection of resources
        /// </summary>
        /// <param name="res">The collection to add</param>
        /// <param name="onFailure">What to do if any of them fails. Will run until failure.</param>
        public void AddRange(IEnumerable<Resource> res, Action onFailure = null) => (res as List<Resource>).ForEach(resource => Add(resource, onFailure: onFailure));

        /// <summary>
        /// Attempts to remove an amount of a resource from the cargo hold. Will error if trying to remove more of a resource than is available.
        /// </summary>
        /// <param name="res">The resource in question to remove</param>
        /// <param name="amt">The amount of the resource to move</param>
        /// <param name="onFailure">The action to call when there is an attempt to remove more than is available.</param>
        public void Remove(Resource res, uint amt = 1, Action onFailure = null) {
            // There's nothing to remove.
            if (!Contains(res)) {
                return;
            }

            Quantified<Resource> slot = this[res];
            Perform((int)slot.Quantity - (int)amt < 0, () => DoOrThrow(onFailure, new NotEnoughOfCargoKindException( )));
            slot.Quantity -= amt;
            Quantity -= res.Volume * amt;
            Perform(slot.Quantity == 0, () => Contents.Remove(slot));
        }

        /// <summary>
        /// Has space for vol volume in the cargo bank.
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        public bool HasSpaceFor(uint vol) => Quantity + vol <= Maximum;

        /// <summary>
        /// Has space for res.Volume * amt worth of volume in the cargo bank.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public bool HasSpaceFor(Resource res, uint amt = 1) => HasSpaceFor(res.Volume * amt);

        /// <summary>
        /// Does the resource bank contain the resource?
        /// </summary>
        /// <param name="res">The resource to check</param>
        /// <returns>If you need an explanation, go somewhere else</returns>
        public bool Contains(Resource res) {
            foreach (Quantified<Resource> cur in Contents) {
                if (cur.Contents.Identifier == res.Identifier) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsAll(List<Quantified<Resource>> resources) => 
            resources.TrueForAll(qr => Contains(qr.Contents) && this[qr.Contents] >= qr.Quantity);
            

        /// <summary>
        /// Use a resource instance to lookup the quantity.
        /// </summary>
        /// <param name="res">The resource to lookup</param>
        /// <returns>A Quantified Resource of how much is contained, or null</returns>
        public Quantified<Resource> this[Resource res] {
            get {
                foreach (Quantified<Resource> cur in Contents) {
                    if (cur.Contents.Identifier == res.Identifier) {
                        return cur;
                    }
                }

                return null;
            }
        }
        /// <summary>
        /// Get the resource at the index <paramref name="key"/>
        /// </summary>
        /// <param name="key">Index of the Resource to look up</param>
        /// <returns>The Quantified Resource</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public Quantified<Resource> this[int key] => Contents[key];

        
    }
}
