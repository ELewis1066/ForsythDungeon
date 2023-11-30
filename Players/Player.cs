using Dungeon.Items;

namespace Dungeon.Players
{

    public class Player
    {
        private Random random;


        // attributes/properties
        private int Health;
        private Room Location;



        private static int[] LevelBoundaries = { 1000, 3000, int.MaxValue };


        private Inventory _inventory = new Inventory();
        private readonly Experience _experience = new Experience();

        // Armour
        public ArmourItem? ArmourWorn { get; set; }

        public Player(int health)
        {
            Health = health;
            random = new Random();


            // Give a player a spell book; only one spell book per player right now.
            _inventory.AddItem(new SpellBookItem("Minters Spell Book", "A spell book with lots pictures of Llamas - lots of missing pages!"));
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
                var found = _inventory.ItemList.Find(item => item.GetName() == name && item.IsArmour());
                if (found != null)
                {
                    // Downcasting, but better than holding base class pointer.
                    // We don't want to always through away strong typing.
                    ArmourWorn = (ArmourItem)found;
                    DisplayArmour($"| You put on the armour {ArmourWorn.GetName()} |");
                    _inventory.RemoveItem(found);
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
                _inventory.AddItem(ArmourWorn);
                ArmourWorn = null;
            }
            else
            {
                Console.WriteLine("You are not wearning any armour!.");
            }


        }

        public int GetXp()
        {
            return _experience.Xp;
        }

        public void AddToXp(int amount)
        {
            _experience.Xp += amount;
            /* Have we gone up a level? */
            if (_experience.Xp > LevelBoundaries[_experience.Level])
            {
                _experience.Level += 1;
                Console.Write("Congratulations, you have gone up a level.");
                Health += 50;
                _experience.LevelDamageBonus += 20;
            }
        }

        public int GetDamageBous()
        {
            return _experience.LevelDamageBonus;
        }


        public SpellBookItem? GetSpellBook()
        {
            /* Assume just one spell book for now. */
            Item? found = _inventory.ItemList.Find(item => item.IsSpellBook());
            if (found != null)
            {
                return (SpellBookItem)found;
            }
            else
            {
                return null;
            }
        }

        public List<Item> GetInventory()
        {
            return _inventory.ItemList;
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
            _inventory.AddItem(item);
        }

        public void RemoveFromInventory(Item item)
        {
            _inventory.RemoveItem(item);
        }

    }
}

