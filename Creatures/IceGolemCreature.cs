using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Creatures
{
    public class IceGolemCreature : Creature
    {
        public IceGolemCreature() : base("iceGolem", 80) { }

        public override Item? OnDeathDrop()
        {
            return new WeaponItem("iceClub", "formed eons ago from the Great Ice Mage; the golems strength is with you", 3);
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
                return base.GetAttackDamage();
            }
        }
    }
}
