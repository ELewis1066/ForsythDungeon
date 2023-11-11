using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Creatures
{

    public class DragonCreature : Creature
    {
        public DragonCreature() : base("dragon", 100) { }

        public override int GetAttackDamage()
        {
            if (random.Next(1, 10) == 1)
            {
                Console.WriteLine("The dragon engulfs you with his fiery breath");
                return 9999999;
            }
            else
            {
                return base.GetAttackDamage();
            }
        }
        public override Item? OnDeathDrop()
        {
            return new WeaponItem("holySword", "a weapon crafted by the Archangels of heaven", 4);
        }

        public override bool TakeSpellDamage(string spell, int damage)
        {
            if (spell == "frostbolt")
            {
                return true;
            }
            else
            {
                return base.TakeSpellDamage(spell, damage);
            }
        }
    }
}
