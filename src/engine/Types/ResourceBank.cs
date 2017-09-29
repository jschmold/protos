using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using System.Linq;
using static System.Linq.Enumerable;
namespace Engine.Types {
    public class ResourceBank : Bank {
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

            if (res == null) {
                throw new ArgumentNullException("res");
            }
            if (!HasSpaceFor(res.Volume * amt)) {
                onFailure.Invoke( );
                return;
            }
            if (Contains(res)) {
                this[res].Quantity += amt;
            } else {
                Contents.Add(new Quantified<Resource> (res, amt));
            }
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
            if (onFailure == null) {
                onFailure = () => {
                    throw new NotEnoughOfCargoKindException( );
                };
            }

            // There's nothing to remove.
            if (!Contains(res)) {
                return;
            }

            Quantified<Resource> slot = this[res];
            if ((int)slot.Quantity - (int)amt < 0) {
                onFailure.Invoke( );
                return;
            }

            slot.Quantity -= amt;
            if (slot.Quantity == 0) {
                Contents.Remove(slot);
            }
            Quantity -= res.Volume * amt;
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

        public bool Contains(Resource res) {
            foreach (Quantified<Resource> cur in Contents) {
                if (cur.Contents.Identifier == res.Identifier) {
                    return true;
                }
            }
            return false;
        }

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
        public Quantified<Resource> this[int key] => Contents[key];
    }
}
