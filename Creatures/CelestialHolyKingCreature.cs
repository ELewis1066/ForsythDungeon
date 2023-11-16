using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Creatures
{
    public class CelestialHolyKingCreature : Creature
    {
        public CelestialHolyKingCreature() : base("celstialHolyKing", 2000) { }


        public override int OnDeathGiveXp() 
        { 
            return 1000; 
        }

        public override int GetAttackDamage()
        {
            if (random.Next(1, 6) == 1)
            {
                Console.WriteLine("He stands behind you telling you; you are strong... be proud");
                return 9999999;
            }
            else
            {
                return random.Next(200, 600);
            }
        }
    }
}
