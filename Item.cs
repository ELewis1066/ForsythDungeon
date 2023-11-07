using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class Item
    {
        protected String Name;
        protected String Description;

        public Item(String name, String description)
        {
            Name = name;
            Description = description;
        }
        public virtual int GetHeals()
        {
            return 0;
        }
        public String GetName()
        {
            return Name;
        }
        public void SetName(String name)
        {
            Name = name;
        }
        public String GetDescription()
        {
            return Description;
        }
        public void SetDescription(String description)
        {
            Description = description;
        }

        public virtual bool IsEdible() { return false; }
    }
}
