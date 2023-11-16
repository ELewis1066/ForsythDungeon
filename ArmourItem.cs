using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class ArmourItem : Item
    {


        public int ArmourClassModifier { get; set; }

        public ArmourItem(string name, string description, int armourClassModifier) : base(name, description)
        {
            ArmourClassModifier = armourClassModifier;
        }

        public override bool IsArmour()
        {
            return true;
        }

        public override string ToString()
        {
            return $"(armour) {Name}, {Description} Armour Class Modifier: +{ArmourClassModifier}";
        }

    }
}
