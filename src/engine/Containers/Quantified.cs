using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Containers {
    /// <summary>
    /// A Quantified is a type that indicates a given quantity of one instance of T
    /// </summary>
    /// <typeparam name="T">The type of thing you want to quantify</typeparam>
    public class Quantified<T> {
        /// <summary>
        /// The instance of type T to be quantified
        /// </summary>
        public T Contents {
            get; set;
        }
        /// <summary>
        /// How much of the T instance there is
        /// </summary>
        public uint Quantity {
            get; set;
        }

        public Quantified(T cont, uint quan = 0) {
            Contents = cont;
            Quantity = quan;
        }

        public static implicit operator T(Quantified<T> slot) => slot.Contents;

        // The operators of BankSlot<T> and a number
        public static Quantified<T> operator +(Quantified<T> slot, uint amt) => new Quantified<T>(slot.Contents, slot.Quantity + amt);
        public static Quantified<T> operator +(Quantified<T> slot, int num) => new Quantified<T>(slot.Contents, slot.Quantity + (uint)num);
        public static Quantified<T> operator -(Quantified<T> slot, uint amt) => new Quantified<T>(slot.Contents, slot.Quantity - amt);
        public static Quantified<T> operator -(Quantified<T> slot, int num) => new Quantified<T>(slot.Contents, slot.Quantity - (uint)num);
        public static Quantified<T> operator *(Quantified<T> slot, int num) => new Quantified<T>(slot.Contents, slot.Quantity * (uint)num);
        public static Quantified<T> operator *(Quantified<T> slot, uint num) => new Quantified<T>(slot.Contents, slot.Quantity * num);
        public static Quantified<T> operator /(Quantified<T> slot, int num) => new Quantified<T>(slot.Contents, slot.Quantity / (uint)num);
        public static Quantified<T> operator /(Quantified<T> slot, uint num) => new Quantified<T>(slot.Contents, slot.Quantity / num);

        // The operators of the number and a BankSlot<T>
        public static int operator +(int num, Quantified<T> slot) => num + (int)slot.Quantity;
        public static uint operator +(uint num, Quantified<T> slot) => num + slot.Quantity;
        public static int operator -(int num, Quantified<T> slot) => num - (int)slot.Quantity;
        public static uint operator -(uint num, Quantified<T> slot) => num - slot.Quantity;
        public static int operator *(int num, Quantified<T> slot) => num * (int)slot.Quantity;
        public static uint operator *(uint num, Quantified<T> slot) => num * slot.Quantity;
        public static int operator /(int num, Quantified<T> slot) => num / (int)slot.Quantity;
        public static uint operator /(uint num, Quantified<T> slot) => num / slot.Quantity;

        public static bool operator <(Quantified<T> slot, int num) => slot.Quantity < num;
        public static bool operator >(Quantified<T> slot, int num) => slot.Quantity > num;
        public static bool operator ==(Quantified<T> slot, int num) => slot.Quantity == num;
        public static bool operator !=(Quantified<T> slot, int num) => slot.Quantity != num;
        public static bool operator <(Quantified<T> slotA, Quantified<T> slotB) => slotA.Quantity < slotB.Quantity;
        public static bool operator >(Quantified<T> slotA, Quantified<T> slotB) => slotA.Quantity < slotB.Quantity;
        public static bool operator ==(Quantified<T> slotA, Quantified<T> slotB) => slotA.Quantity == slotB.Quantity && slotB.Contents.GetHashCode( ) == slotB.Contents.GetHashCode( );
        public static bool operator !=(Quantified<T> slotA, Quantified<T> slotB) => slotA.Quantity != slotB.Quantity || slotA.Contents.GetHashCode( ) != slotB.Contents.GetHashCode( );


        public static implicit operator int(Quantified<T> slot) => (int)slot.Quantity;
        public static implicit operator uint(Quantified<T> slot) => slot.Quantity;

        public override bool Equals(object obj) {
            var slot = obj as Quantified<T>;
            return slot != null &&
                   EqualityComparer<T>.Default.Equals(Contents, slot.Contents) &&
                   Quantity == slot.Quantity;
        }

        public override int GetHashCode() {
            var hashCode = 1613860915;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Contents);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode( );
            return hashCode;
        }
    }
}
