using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class KeyItem : Item
    {
        public int KeyCode { get; set; }

        public KeyItem(string name, string description, int keyCode) : base(name, description) 
        { 
            KeyCode = keyCode;
        }

        public override string ToString()
        { 
            return $"(key) {Name}, {Description}";
        }
    }
}
