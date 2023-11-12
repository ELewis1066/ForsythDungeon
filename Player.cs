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

        private static int[] LevelBoundaries = { 1000, 3000, int.MaxValue };

        private int Xp;
        private int Level;
        private int LevelDamageBonus;

        public Player(int health)
        {
            Health = health;
            random = new Random();
            Xp = 0;
            Level = 0;
            LevelDamageBonus = 0;
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

