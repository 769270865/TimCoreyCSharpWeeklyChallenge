using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Reminder.PillReminnder.Model
{
    public class Pill : IEquatable<Pill>
    {

        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public Pill() { }

        public Pill(string name, int quantity, string description = "")
        {
            Name = name;
            Quantity = quantity;
            ID = Guid.NewGuid();
        }
        public Pill(string name,int quantity,Guid id,string description = "")
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pill);
        }

        public bool Equals(Pill other)
        {
            return other != null &&
                   ID == other.ID &&
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
