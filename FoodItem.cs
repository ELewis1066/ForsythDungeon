using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class FoodItem : Item
    {
        private int HealAmount;
        public FoodItem(String name, String description, int heals) : base(name, description)
        {
            HealAmount = heals;
        }
        public override int GetHeals()
        {
            return HealAmount;
        }

        public override bool IsEdible()
        {
            return true;
        }

        public override string ToString()
        {
            return $"(food) {Name}, {Description} Heal amount: {HealAmount}";
        }


    }
}
