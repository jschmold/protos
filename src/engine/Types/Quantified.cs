using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types
{
    /// <summary>
    /// A Quantized T is when you want to have 1 of T and store an "amount" of T without having multiple of type T.
    /// </summary>
    /// <typeparam name="T">The type of thing you want to quantify</typeparam>
    public class Quantified<T>
    {
        public T Contents { get; set; }
        public uint Quantity { get; set; }

        public static implicit operator T(Quantified<T> slot)
            { return slot.Contents; }
        
        // The operators of BankSlot<T> and a number
        public static Quantified<T> operator +(Quantified<T> slot, uint amt)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity + amt }; }
        public static Quantified<T> operator +(Quantified<T> slot, int num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity + (uint)num }; }
        public static Quantified<T> operator -(Quantified<T> slot, uint amt)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity - amt }; }
        public static Quantified<T> operator -(Quantified<T> slot, int num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity - (uint)num }; }
        public static Quantified<T> operator *(Quantified<T> slot, int num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity * (uint)num }; }
        public static Quantified<T> operator *(Quantified<T> slot, uint num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity * num }; }
        public static Quantified<T> operator /(Quantified<T> slot, int num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity / (uint)num }; }
        public static Quantified<T> operator /(Quantified<T> slot, uint num)
            { return new Quantified<T> { Contents = slot.Contents, Quantity = slot.Quantity / num }; }

        // The operators of the number and a BankSlot<T>
        public static int operator +(int num, Quantified<T> slot)
            { return num + (int)slot.Quantity; }
        public static uint operator +(uint num, Quantified<T> slot)
            { return num + slot.Quantity; }
        public static int operator -(int num, Quantified<T> slot)
            { return num - (int)slot.Quantity; }
        public static uint operator -(uint num, Quantified<T> slot)
            { return num - slot.Quantity; }
        public static int operator *(int num, Quantified<T> slot)
            { return num * (int)slot.Quantity; }
        public static uint operator *(uint num, Quantified<T> slot)
            { return num * slot.Quantity; }
        public static int operator /(int num, Quantified<T> slot)
            { return num / (int)slot.Quantity; }
        public static uint operator /(uint num, Quantified<T> slot)
            { return num / slot.Quantity; }

        public static bool operator <(Quantified<T> slot, int num)
            { return slot.Quantity < num; }
        public static bool operator >(Quantified<T> slot, int num)
            { return slot.Quantity > num; }
        public static bool operator ==(Quantified<T> slot, int num)
            { return slot.Quantity == num; }
        public static bool operator !=(Quantified<T> slot, int num)
            { return slot.Quantity != num; }
        public static bool operator <(Quantified<T> slotA, Quantified<T> slotB)
            { return slotA.Quantity < slotB.Quantity; }
        public static bool operator >(Quantified<T> slotA, Quantified<T> slotB)
            { return slotA.Quantity < slotB.Quantity; }
        public static bool operator == (Quantified<T> slotA, Quantified<T> slotB)
            { return slotA.Quantity == slotB.Quantity && slotB.Contents.GetHashCode() == slotB.Contents.GetHashCode(); }
        public static bool operator != (Quantified<T> slotA, Quantified<T> slotB)
            { return slotA.Quantity != slotB.Quantity || slotA.Contents.GetHashCode() != slotB.Contents.GetHashCode(); }


        public static implicit operator int(Quantified<T> slot)
            { return (int)slot.Quantity; }
        public static implicit operator uint(Quantified<T> slot)
            { return slot.Quantity; }

        public override bool Equals(object obj)
        {
            var slot = obj as Quantified<T>;
            return slot != null &&
                   EqualityComparer<T>.Default.Equals(Contents, slot.Contents) &&
                   Quantity == slot.Quantity;
        }

        public override int GetHashCode()
        {
            var hashCode = 1613860915;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Contents);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
        }
    }
}
