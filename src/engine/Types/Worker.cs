using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types
{
    public class Worker
    {
        /// <summary>
        /// Either the charge or the mental energy of the worker.
        /// </summary>
        public Bank Energy { get; set; }
        public Location Position { get; set; }
        public WorkerCategory Category { get; set; }
        public string Name { get; set; }
        public Bank Health { get; set; } = new Bank { Quantity = 100, Maximum = 100 };
        public WorkerActivity CurrentActivity { get; set; }
    }
}
