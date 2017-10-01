using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using static System.Math;


namespace Engine.Types {
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
        public bool HasEnoughFor(int amt) => this - amt >= 0;
        public bool HasEnoughFor(uint amt) => (int)(this - amt) >= 0;
        public bool IsFull => Quantity == Maximum;

        public static int operator +(Bank bank, int num) => (int)bank.Quantity + num;
        public static int operator -(Bank bank, int num) => (int)bank.Quantity - num;
        public static uint operator +(Bank bank, uint num) => bank.Quantity + num;
        public static uint operator -(Bank bank, uint num) => bank.Quantity - num;
        public static uint operator *(Bank bank, int num) => bank.Quantity * (uint)num;
        public static int operator /(Bank bank, int num) => (int)bank.Quantity / num;
        public static int operator /(int num, Bank bank) => num / (int)bank.Quantity;

        public static implicit operator int(Bank bank) => (int)bank.Quantity;
        public static implicit operator uint(Bank bank) => bank.Quantity;
        public static implicit operator string(Bank bank) => bank.Quantity.ToString( );
    }
}
