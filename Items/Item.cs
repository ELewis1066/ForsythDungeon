using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Items
{
    public class Item
    {
        protected string Name;
        protected string Description;

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public virtual int GetHeals()
        {
            return 0;
        }

        public virtual int GetDamageMultiplier()
        {
            return 1;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public string GetDescription()
        {
            return Description;
        }
        public void SetDescription(string description)
        {
            Description = description;
        }

        public virtual bool IsEdible() { return false; }

        public virtual bool IsWeapon() { return false; }

        public virtual bool IsArmour() { return false; }

        public virtual bool IsSpellBook() { return false; }


        public virtual bool CanPickUp() { return true; }

        public override string ToString()
        {
            return $"(item) {Name}, {Description}";
        }
    }
}
