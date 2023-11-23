using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
  
    public class SpellBookItem : Item
    {
        /**
         * At the moment a spell Book only contains one spell.
         */

        private Dictionary<string,int> spells = new Dictionary<string,int>();

        public SpellBookItem(String name, String description) : base(name, description) 
        {
            spells = new Dictionary<string, int>()
            {
                {"frostbolt", 50 },
                {"lightning", 80 }
            };
        }

        public bool ContainsSpell(string spellName)
        {
            return spells.ContainsKey(spellName);
        }

        public int GetDamage(string spellName)
        {
            return spells[spellName];
        }
        public override bool IsSpellBook()
        {
            return true;
        }

        public override string ToString()
        {
            string spellList = string.Join("\n",spells.Keys.ToList());
            return $"(Spell Book) {Name}, {Description};\n It contains these spells:\n{spellList}";
        }


    }
}
