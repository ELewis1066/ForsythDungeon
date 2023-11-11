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

                    string nameOfItem = instructions[1];
                    if (Location.ContainsName(nameOfItem))
                    {
                        Item removedItem = Location.RemoveItem(nameOfItem);
                        Inventory.Add(removedItem);
                    }
                    else
                    {
                        Console.WriteLine($"Item {nameOfItem} does not exist in the location.");
                    }
                    //Inventory.Add(Location.RemoveItem(instructions[1]));
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
                            int damage = random.Next(1, 20);
                            // if there is a 3rd instruction & the 3rd instruction is a weapon
                            // item, then multiply the damage by the damage multiplier of the
                            // weapon.
                            // e.g. allow 'attack dragon' or 'attack dragon sword'.
                            if (instructions.Count > 2)
                            {
                                string name = instructions[2];
                                Item? found = Inventory.Find(item => item.IsWeapon() && item.GetName() == name);
                                if (found != null)
                                {
                                    Console.WriteLine($"Attacking with {name}");
                                    damage *= found.GetDamageMultiplier();
                                }
                            }
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
                        // first, remove the creature from the room.
                        Location.RemoveCreature(deadCreature);
                        // does the creature drop an item on death?
                        Item? onDeathItem = deadCreature.OnDeathDrop();
                        if (onDeathItem != null)
                        {
                            Location.AddItem(onDeathItem);
                            Console.WriteLine($"The creature is dead, it drops a {onDeathItem.GetName()}");
                        }
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
                    if (Inventory.Count == 0)
                    {
                        Console.WriteLine("You aren't carrying anything.");
                    }
                    Console.WriteLine("You have the following items:");
                    Console.WriteLine(String.Join("\n", Inventory));
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
            // Handles case where we have items {apple, apple, apple} say
            // and the first apple is not edible. Code breaks, but we could have
            // found an edible apple if we had searched the whole list.
            Item? found = Inventory.Find(item => item.IsEdible() && item.GetName() == food);
            if (found != null)
            {
                Health += found.GetHeals();
                Inventory.Remove(found);
            }
            else
            {
                Console.Write($"Could not find an edible item of name: {food}");
            }
        }



    }
    
}
