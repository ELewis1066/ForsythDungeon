using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Creatures
{
    public class ShadowKnightCreature : Creature
    {
        public ShadowKnightCreature() : base("shadowKnight", 950) { }

        public override int GetAttackDamage()
        {
            if (random.Next(1, 15) == 1)
            {
                Console.WriteLine("darkness is cloaked around you... confused whether your eyes are opened or not death gives you there hand");
                return 9999999;
            }
            else
            {
                return random.Next(75, 400);
            }
        }

        public override int OnDeathGiveXp()
        {
            return 650;
        }


        public override Item? OnDeathDrop()
        {
            return new WeaponItem("demonicSword", "the deviation of mans sins formed into one weapon *** ORINGIN UNKNOWN ***", 4);
        }

    }
}
