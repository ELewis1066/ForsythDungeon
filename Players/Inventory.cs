using Dungeon.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Players
{
    public class Inventory
    {
        public  List<Item> ItemList { get;} // get only. 

        public Inventory() 
        {
            ItemList = new List<Item>();
        }

        public void AddItem(Item item)
        {
            // We create own method here so that we can add
            // in validation (how many items etc at a later stage).
            ItemList.Add(item);
        }

        public void RemoveItem(Item item)
        {
            // We may have cursed items that can't be dropped?
            ItemList.Remove(item);  
        }

    }
}
