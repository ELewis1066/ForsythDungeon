using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Player
    {
        // attributes/properties
        private int Health;
        private Room Location;
        private List<Item> Inventory = new List<Item>();
        private Dictionary<string, int> SpellBook = new Dictionary<string, int> { { "frostbolt", 50 }, { "lightning", 80 } };
        private Random random;

        // Experience (could be a new class?)
        private static int[] LevelBoundaries = { 1000, 3000, int.MaxValue };
        private int Xp;
        private int Level;
        private int LevelDamageBonus;

        // Armour
        public ArmourItem? ArmourWorn { get; set; }

        public Player(int health)
        {
            Health = health;
            random = new Random();
            Xp = 0;
            Level = 0;
            LevelDamageBonus = 0;
        }

        public void DisplayArmour(string message)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat("=", message.Length)));
            Console.WriteLine(message);
            Console.WriteLine(string.Concat(Enumerable.Repeat("=", message.Length)));
        }
        public void WearArmour(string name)
        {
            if (ArmourWorn != null) 
            {
                Console.WriteLine($"You are already wearning armour {ArmourWorn.GetName()}");
            }
            else
            {
                /* check that armour exists in inventory. */
                var found = Inventory.Find(item => item.GetName() == name && item.IsArmour());
                if (found != null)
                {
                    // Downcasting, but better than holding base class pointer.
                    // We don't want to always through away strong typing.
                    ArmourWorn = (ArmourItem) found;
                    DisplayArmour($"| You put on the armour {ArmourWorn.GetName()} |");
                    Inventory.Remove(found);
                }
                else
                {
                    Console.WriteLine($"Sorry could not find armour {name}.");
                }
            }
        }

        public void RemoveArmour()
        {
            if (ArmourWorn != null)
            {
                DisplayArmour($"| You take off your {ArmourWorn.GetName()} armour |");
                Inventory.Add(ArmourWorn);
                ArmourWorn = null;
            }
            else
            {
                Console.WriteLine("You are not wearning any armour!.");
            }
        }

        public int GetXp()
        {
            return Xp;
        }

        public void AddToXp(int amount)
        {
            Xp += amount;
            /* Have we gone up a level? */
            if (Xp > LevelBoundaries[Level])
            {
                Level += 1;
                Console.Write("Congratulations, you have gone up a level.");
                Health += 50;
                LevelDamageBonus += 20;
            }
        }

        public int GetDamageBous()
        {
            return LevelDamageBonus;
        }


        public Dictionary<string, int> GetSpellBook()
        {
            return SpellBook;
        }

        public List<Item> GetInventory()
        {
            return Inventory;
        }

        public int GetHealth()
        {
            return Health;
        }
        public void SetHealth(int health)
        {
            Health = health;
        }

        public void Heal(int amount)
        {
            Health = Math.Max(100, Health + amount);
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
            if (Health == 0)
            {
                Console.WriteLine("Sorry, you are dead.");
            }
        }

        public bool IsDead()
        {
            return Health == 0;
        }

        public Room GetLocation()
        {
            return Location;
        }
        public void SetLocation(Room location)
        {
            Location = location;
            Console.WriteLine(Location.GetDescription());
        }
        // other methods
        public int AdjustHealth(int health)
        {
            Health += health;
            return Health;
        }

        public void AddToInventory(Item item)
        {
            Inventory.Add(item);
        }

        public void RemoveFromInventory(Item item)
        {
            Inventory.Remove(item);
        }
    }
}

