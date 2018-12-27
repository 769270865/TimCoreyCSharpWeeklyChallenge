using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    public class Pill : IEquatable<Pill>
    {
       
        public string Name { get; set; }
        public int Quantity { get; set; }


        public Pill(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pill);
        }

        public bool Equals(Pill other)
        {
            return other != null &&
                   Name == other.Name &&
                   Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            var hashCode = 1276681454;
            hashCode = hashCode * -1521134295 + Name.GetHashCode();
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
        }
    }
}
