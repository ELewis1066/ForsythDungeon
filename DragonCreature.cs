using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{

    class DragonCreature : Creature
    {
        public DragonCreature() : base("dragon", 100) { }

        public override int GetAttackDamage()
        {
            if (random.Next(1, 3) == 1)
            {
                Console.WriteLine("The dragon engulfs you with his fiery breath");
                return 9999999;
            }
            else
            {
                return base.GetAttackDamage();
            }
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
