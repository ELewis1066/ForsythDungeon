using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Items;

namespace Dungeon.Creatures
{
    public class IceGolemCreature : Creature
    {
        public IceGolemCreature() : base("iceGolem", 800) { }

        public override Item? OnDeathDrop()
        {
            return new WeaponItem("iceClub", "formed eons ago from the Great Ice Mage; the golems strength is with you", 3);
        }

        public override int OnDeathGiveXp()
        {
            return 450;
        }


        public override int GetAttackDamage()
        {
            if (random.Next(1, 20) == 1)
            {
                Console.WriteLine("The iceGolems sheer strength eviscerated you ");
                return 9999999;
            }
            else
            {
                return random.Next(100, 300);
            }
        }
    }
}
