using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class Player
    {
        // attributes/properties
        private int Health;
        private Room Location;
        private List<Item> Inventory = new List<Item>();
        private Dictionary<string, int> SpellBook = new Dictionary<string, int> { { "frostbolt", 50 }, { "lightning", 80 } };
        private Random random;

        public Player(int health)
        {
            Health = health;
            random = new Random();
        }
        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public int GetHealth()
        {
            return Health;
        }
        public void SetHealth(int health)
        {
            Health = health;
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
        public Boolean DoCommand(String command)
        {
            if (command == "QUIT")
            {
                return true;
            }

            List<String> instructions = command.Split(' ').ToList();

            switch (instructions[0])
            {
                case "look":
                    Console.WriteLine(Location.GetDescription());
                    break;
                case "health":
                case "h":
                    Console.WriteLine($"You have {Health} health.");
                    break;
                case "move":
                case "go":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Move where?");
                    }
                    else
                    {
                        Move(instructions[1]);
                    }
                    break;
                case "get":
                case "take":
                    Inventory.Add(Location.RemoveItem(instructions[1]));
                    break;
                case "drop":
                    string itemToDrop = instructions[1];
                    Item droppedItem = null;

                    foreach (Item item in Inventory)
                    {
                        if (item.GetName() == itemToDrop)
                        {
                            droppedItem = item;
                        }
                    }
                    if (droppedItem != null)
                    {
                        Inventory.Remove(droppedItem);
                        Location.AddItem(droppedItem);
                    }
                    else
                    {
                        Console.WriteLine("You don't have a " + itemToDrop);
                    }
                    break;
                case "attack":
                    Creature deadCreature = null;
                    foreach (Creature creature in Location.GetCreatures())
                    {
                        if (creature.GetName() == instructions[1])
                        {
                            int damage = random.Next(1, 100);
                            bool dead = creature.TakeDamage(damage);
                            if (dead)
                            {
                                Console.WriteLine($"Your attack killed the {creature.GetName()}");
                                deadCreature = creature;
                            }
                            else
                            {
                                Console.WriteLine($"Your attack caused the {creature.GetName()} to lose {damage} health.");
                                int damageTaken = creature.GetAttackDamage();
                                Console.WriteLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                                Health -= damageTaken;
                                if (Health < 0)
                                {
                                    Console.WriteLine("You die.");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (deadCreature != null)
                    {
                        Location.RemoveCreature(deadCreature);
                    }
                    break;
                case "cast":
                    if (instructions.Count < 3)
                    {
                        Console.WriteLine("Cast which spell at which target?");
                    }
                    else
                    {
                        string spell = instructions[1];
                        string target = instructions[2];
                        Creature creatureToRemove = null;

                        if (SpellBook.ContainsKey(spell))
                        {
                            foreach (Creature creature in Location.GetCreatures())
                            {
                                if (creature.GetName() == target)
                                {
                                    bool dead = creature.TakeSpellDamage(spell, SpellBook[spell]);
                                    if (dead)
                                    {
                                        Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                        creatureToRemove = creature;
                                    }
                                    else
                                    {
                                        Health -= creature.GetAttackDamage();
                                        if (Health < 0)
                                        {
                                            Console.WriteLine("You die.");
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            if (creatureToRemove != null)
                            {
                                Location.RemoveCreature(creatureToRemove);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't know that spell!");
                        }
                    }
                    break;
                case "examine":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Examine what?");
                    }
                    else
                    {
                        for (int i = 0; i < Inventory.Count; i++)
                        {
                            if (instructions[1] == Inventory[i].GetName())
                            {
                                Console.WriteLine(Inventory[i].GetDescription());
                            }
                        }

                    }

                    break;
                case "eat":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Eat what?");
                    }
                    else
                    {
                        Eat(instructions[1]);
                    }

                    break;
                case "inventory":
                case "i":
                    String items = "\n";

                    if (Inventory.Count > 0)
                    {
                        if (Inventory.Count == 1)
                        {
                            items += $"You have the following item: {Inventory[0].GetName()}.";
                        }
                        else
                        {
                            items += $"You have the following items: {Inventory[0].GetName()}";
                            for (int i = 1; i < Inventory.Count - 1; i++)
                            {
                                items += ", " + Inventory[i].GetName();
                            }
                            items += $" and {Inventory[Inventory.Count - 1].GetName()}.";
                        }
                    }
                    else
                    {
                        items += "You aren't carrying anything.";
                    }
                    Console.WriteLine(items);
                    break;
                default:
                    Console.WriteLine("You can't do that!");
                    break;
            }

            return false; // they didn't quit
        }
        private void Move(String direction)
        {
            List<String> exits = Location.GetDirections();
            Boolean directionFound = false;

            for (int i = 0; i < exits.Count; i++)
            {
                if (direction == exits[i])
                {
                    directionFound = true;
                    if (!Location.GetConnections()[i].GoThrough(this, direction))
                    {
                        Console.WriteLine($"You can't go {direction}");
                    }
                }
            }

            if (!directionFound)
            {
                Console.WriteLine($"There is no exit to the {direction}");
            }
        }
        private void Eat(string food)
        {
            int foodPosition;

            for (foodPosition = 0; foodPosition < Inventory.Count; foodPosition++)
            {
                Item? item = Inventory[foodPosition];
                if (item.GetName() == food)
                {
                    if (item.IsEdible())
                    //if (item.GetType() == typeof(FoodItem))
                    {
                        Health += Inventory[foodPosition].GetHeals();
                        Inventory.RemoveAt(foodPosition);
                    }
                    else
                    {
                        Console.WriteLine($"Sorry, {food} is not edible.");
                    }
                    break;
                }
            }
        }
    }
}
