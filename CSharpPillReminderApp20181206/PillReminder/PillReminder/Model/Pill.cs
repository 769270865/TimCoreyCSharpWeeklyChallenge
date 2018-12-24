using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    public class Pill
    {
       
        public string Name { get; set; }
        public int Quantity { get; set; }


        public Pill(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
       
    }
}
