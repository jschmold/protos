using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using static System.Math;


namespace Engine.Containers {
    /// <summary>
    /// A bank is when you want to limit the values on a uint amount of a thing. 
    /// For example, it makes no sense to have less than 0 of oxygen, but it does make sense to have a max amount of oxygen.
    /// Banks also support decay and regen (default amounts) as nice helpers.
    /// </summary>
    public class Bank {
        private uint _quantity = 0;
        /// <summary>
        /// The quantity of a given uint-based resource in the bank. If the value passed to the setter is higher than the Maximum, the Maximum is used instead.
        /// </summary>                                                        
        public uint Quantity {
            get => _quantity;
            set => _quantity = Min(value, Maximum);
        }

        /// <summary>
        /// The maximum amount of energy the bank can hold
        /// </summary>
        public uint Maximum { get; set; } = uint.MaxValue;
        /// <summary>
        /// Whether or not the bank has enough for amt
        /// </summary>
        /// <param name="amt">The amount to check</param>
        /// <returns></returns>
        public bool HasEnoughFor(int amt) => this - amt >= 0;
        /// <summary>
        /// <see cref="HasEnoughFor(int)"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public bool HasEnoughFor(uint amt) => (int)(this - amt) >= 0;
        /// <summary>
        /// Whether or not the bank has reached its capacity
        /// </summary>
        public bool IsFull => Quantity == Maximum;

        /// <summary>
        /// Get the quantity + num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose quantity we are to use</param>
        /// <param name="num">The number to add</param>
        /// <returns></returns>
        public static int operator +(Bank bank, int num) => (int)bank.Quantity + num;
        /// <summary>
        /// Get the quantity - num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose quantity we are to use</param>
        /// <param name="num">The number to subtract</param>
        /// <returns></returns>
        public static int operator -(Bank bank, int num) => (int)bank.Quantity - num;
        /// <summary>
        /// Adds the quantity of b to a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A bank with the quantity of a + b, and a maximum of a.Maximum</returns>
        public static Bank operator +(Bank a, Bank b) => new Bank { Maximum = a, Quantity = a + b };
        /// <summary>
        /// Subtracts the quantity of b from a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A bank with the quantity of a - b, and a maximum of a.Maximum</returns>
        public static Bank operator -(Bank a, Bank b) => new Bank { Maximum = a, Quantity = a - b };
        /// <summary>
        /// Get the quantity + num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose quantity we are to use</param>
        /// <param name="num">The number to add</param>
        /// <returns></returns>
        public static uint operator +(Bank bank, uint num) => bank.Quantity + num;
        /// <summary>
        /// Get the quantity - num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose quantity we are to use</param>
        /// <param name="num">The number to subtract</param>
        /// <returns></returns>
        public static uint operator -(Bank bank, uint num) => bank.Quantity - num;
        /// <summary>
        /// Get the quantity * num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose quantity we are to use</param>
        /// <param name="num">The number we are to multiply by</param>
        /// <returns></returns>
        public static uint operator *(Bank bank, int num) => bank.Quantity * (uint)num;
        /// <summary>
        /// Get the quantity / num result of a bank
        /// </summary>
        /// <param name="bank">The bank whose result we are to use</param>
        /// <param name="num">The number to divide by</param>
        /// <returns></returns>
        public static int operator /(Bank bank, int num) => (int)bank.Quantity / num;
        /// <summary>
        /// Get the num / quantity of a bank
        /// </summary>
        /// <param name="num">The number the bank's quantity is to be divided by</param>
        /// <param name="bank">The divisor</param>
        /// <returns></returns>
        public static int operator /(int num, Bank bank) => num / (int)bank.Quantity;
        /// <summary>
        /// Is bank A smaller than bank B?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bank operator <(Bank a, Bank b) => a.Quantity < b.Quantity ? a : b;
        /// <summary>
        /// Is bank A larger than bank B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bank operator >(Bank a, Bank b) => a.Quantity > b.Quantity ? a : b;

        /// <summary>
        /// Gets the int-casted quantity in a bank
        /// </summary>
        /// <param name="bank"></param>
        public static implicit operator int(Bank bank) => (int)bank.Quantity;
        /// <summary>
        /// Gets the quantity in a bank
        /// </summary>
        /// <param name="bank"></param>
        public static implicit operator uint(Bank bank) => bank.Quantity;
        /// <summary>
        /// Outputs the quantity to a string
        /// </summary>
        /// <param name="bank"></param>
        public static implicit operator string(Bank bank) => bank.Quantity.ToString( );

        /// <summary>
        /// Merge the quantity and the maximum of the other bank into this one
        /// </summary>
        /// <param name="other"></param>
        public void Absorb(Bank other) {
            Maximum += other.Maximum;
            Quantity += other.Quantity;
        }
        /// <summary>
        /// Merge the quantities and maximums of two banks
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bank Absorb(Bank a, Bank b) => new Bank {
            Maximum = a.Maximum + b.Maximum,
            Quantity = a.Quantity + b.Quantity
        };
    }
}
