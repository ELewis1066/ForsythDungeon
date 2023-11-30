using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Items
{
    public class ChestItem : Item
    {
        public List<Item> ChestContents { get; set; }
        public bool IsLocked { get; set; }

        public int KeyCode { get; set; }

        public ChestItem(string name, string description, int keyCode) : base(name, description)
        {
            ChestContents = new List<Item>();
            IsLocked = true;
            KeyCode = keyCode;
        }

        public void PutItemInChest(Item item)
        {
            ChestContents.Add(item);
        }

        public Item? TakeItemFromChest(string name)
        {
            var result = ChestContents.Find(x => x.GetName() == name);
            if (result == null)
            {
                return null;
            }
            else
            {
                ChestContents.Remove(result);
                return result;
            }
        }

        public override bool CanPickUp()
        {
            return false;
        }
        public void DisplayContent()
        {
            Console.WriteLine("The chest contains:");
            Console.WriteLine(string.Join("\n", ChestContents));
        }

        public void UnlockChest(KeyItem key)
        {
            if (key.KeyCode == KeyCode)
            {
                Console.WriteLine("Congratulations you found the right key!");
                Console.WriteLine("The chest is unlocked...");
                IsLocked = false;
                ToString();
            }
            else
            {
                Console.WriteLine("Sorry, that key does not fit this chest.");
            }
        }

        public override string ToString()
        {
            string repr = $"(chest) {Name}, {Description} Is it locked?: {IsLocked}";
            if (!IsLocked)
            {
                foreach (var item in ChestContents)
                {
                    repr += item.ToString();
                }
            }
            return repr;
        }

    }
}
