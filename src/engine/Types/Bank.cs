using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using static System.Math;
using Newtonsoft.Json;
namespace Engine.Types
{
    /// <summary>
    /// A bank is when you want to limit the values on a uint amount of a thing. 
    /// For example, it makes no sense to have less than 0 of oxygen, but it does make sense to have a max amount of oxygen.
    /// Banks also support decay and regen (default amounts) as nice helpers.
    /// </summary>
    public class Bank
    {
        private uint _quantity = 0;
        /// <summary>
        /// The quantity of a given uint-based resource in the bank. If the value passed to the setter is higher than the Maximum, the Maximum is used instead.
        /// </summary>                                                        
        public uint Quantity {                                                
            get {                                                             
                return _quantity;                                             
            }                                               
            set {
                _quantity = Min(value, Maximum);
            }
        }                             

        /// <summary>
        /// The maximum amount of energy the bank can hold
        /// </summary>
        public uint Maximum { get; set; } = uint.MaxValue;
       
        public static int operator +(Bank bank, int num)
            { return (int)bank.Quantity + num; }
        public static int operator -(Bank bank, int num)
            { return (int)bank.Quantity - num; }
        public static uint operator +(Bank bank, uint? num)
            { return bank.Quantity + num ?? 0; }
        public static uint operator -(Bank bank, uint? num)
            { return bank.Quantity - num ?? 0; }
        public static uint operator *(Bank bank, int num)
            { return bank.Quantity * (uint)num; }
        public static int operator /(Bank bank, int num)
            { return (int)bank.Quantity / num; }
        public static int operator /(int num, Bank bank)
            { return num / (int)bank.Quantity; }

        public static implicit operator int(Bank bank)
            { return (int)bank.Quantity; }
        public static implicit operator uint(Bank bank)
            { return bank.Quantity; }
        public static implicit operator string(Bank bank)
            { return bank.Quantity.ToString(); }
    }
}
