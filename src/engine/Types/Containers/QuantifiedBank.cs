using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Containers {
    /// <summary>
    /// A "Quantified bank", where it represents not only the amount, but also "what" it is storing.
    /// </summary>
    /// <typeparam name="T">The thing to be stored.</typeparam>
    public class QuantifiedBank<T> : Bank {
        public T Currency {
            get; set;
        }
    }
}
