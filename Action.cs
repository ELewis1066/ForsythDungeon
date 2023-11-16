using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
 

    public interface IGameAction
    {
        abstract static void DoAction(List<string> instructions, Player player);
    }

    /// <summary>
    /// Look Command.
    /// </summary>
    public class Look : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Look.DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            Console.WriteLine(player.GetLocation().GetDescription());
        }
    }

    /// <summary>
    /// Health Command.
    /// </summary>
    public class Health : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Health.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.WriteLine($"You have {player.GetHealth()} health.");
        }
    }

    public class Move : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Move.DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            if (instructions.Count <= 1)
            {
                Console.WriteLine("Move where?");
            }
            else
            {
                DoMove(instructions[1], player);
            }
        }

        private static void DoMove(String direction, Player player)
        {
            List<String> exits = player.GetLocation().GetDirections();
            Boolean directionFound = false;

            for (int i = 0; i < exits.Count; i++)
            {
                if (direction == exits[i])
                {
                    directionFound = true;
                    if (!player.GetLocation().GetConnections()[i].GoThrough(player, direction))
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
    }

    public class Take : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Take.DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            string nameOfItem = instructions[1];
            if (player.GetLocation().ContainsName(nameOfItem))
            {
                Item removedItem = player.GetLocation().RemoveItem(nameOfItem);
                player.AddToInventory(removedItem);
            }
            else
            {
                Console.WriteLine($"Item {nameOfItem} does not exist in the location.");
            }
        }
    }

    public class Drop : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Drop.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string itemToDrop = instructions[1];
            Item droppedItem = null;

            foreach (Item item in player.GetInventory())
            {
                if (item.GetName() == itemToDrop)
                {
                    droppedItem = item;
                }
            }
            if (droppedItem != null)
            {
                player.RemoveFromInventory(droppedItem);
                player.GetLocation().AddItem(droppedItem);
            }
            else
            {
                Console.WriteLine("You don't have a " + itemToDrop);
            }
        }
    }


    public class Attack : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Attack.DoAction);
        private static Random random = new Random();
        public static void DoAction(List<string> instructions, Player player)
        {
            Creature deadCreature = null;
            foreach (Creature creature in player.GetLocation().GetCreatures())
            {
                if (creature.GetName() == instructions[1])
                {
                    int damage = random.Next(50, 200);
                    // if there is a 3rd instruction & the 3rd instruction is a weapon
                    // item, then multiply the damage by the damage multiplier of the
                    // weapon.
                    // e.g. allow 'attack dragon' or 'attack dragon sword'.
                    if (instructions.Count > 2)
                    {
                        string name = instructions[2];
                        Item? found = player.GetInventory().Find(item => item.IsWeapon() && item.GetName() == name);
                        if (found != null)
                        {
                            Console.WriteLine($"Attacking with {name}");
                            damage *= found.GetDamageMultiplier();
                            damage += player.GetDamageBous();
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
                        Console.WriteLine($"The {creature.GetName()} now has {creature.GetHealth()} health left ");

                        // Modify damage damage according to armour class.
                        int damageTaken = creature.GetAttackDamage();
                        if (player.ArmourWorn != null)
                        {
                            int modifer = player.ArmourWorn.ArmourClassModifier;
                            damageTaken = damageTaken * (100 - modifer) / 100;
                            Console.Write("Your armour saves some damage, but .. ");
                        }


                        Console.WriteLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                        player.TakeDamage(damageTaken);

                        if (player.IsDead()) return;
                    }
                }
            }
            if (deadCreature != null)
            {
                // first, remove the creature from the room.
                Console.Write("The creature is dead.");
                player.GetLocation().RemoveCreature(deadCreature);
                // does the creature drop an item on death?
                Item? onDeathItem = deadCreature.OnDeathDrop();
                if (onDeathItem != null)
                {
                    player.GetLocation().AddItem(onDeathItem);
                    Console.WriteLine($"It has dropped {onDeathItem.GetName()}");
                }
                int xpBonus = deadCreature.OnDeathGiveXp();
                player.AddToXp(xpBonus);
                Console.WriteLine($"You earned an XP bonus of {xpBonus}; Xp: {player.GetXp()}");
            }
        }
    }

    public class Cast : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Cast.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            if (instructions.Count < 3)
            {
                Console.WriteLine("Cast which spell at which target?");
            }
            else
            {
                string spell = instructions[1];
                string target = instructions[2];
                Creature creatureToRemove = null;

                if (player.GetSpellBook().ContainsKey(spell))
                {
                    foreach (Creature creature in player.GetLocation().GetCreatures())
                    {
                        if (creature.GetName() == target)
                        {
                            bool dead = creature.TakeSpellDamage(spell, player.GetSpellBook()[spell]);
                            if (dead)
                            {
                                Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                creatureToRemove = creature;
                            }
                            else
                            {
                                player.TakeDamage(creature.GetAttackDamage());
                            }
                        }
                    }
                    if (creatureToRemove != null)
                    {
                        player.GetLocation().RemoveCreature(creatureToRemove);
                    }
                }
                else
                {
                    Console.WriteLine("You don't know that spell!");
                }
            }
        }
    }

    public class Examine : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Examine.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            if (instructions.Count <= 1)
            {
                Console.WriteLine("Examine what?");
            }
            else
            {
                var inventory = player.GetInventory();
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (instructions[1] == inventory[i].GetName())
                    {
                        Console.WriteLine(inventory[i].GetDescription());
                    }
                }
            }
        }
    }

    public class Eat : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Eat.DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            if (instructions.Count <= 1)
            {
                Console.WriteLine("Eat what?");
            }
            else
            {
                DoEat(instructions[1], player);
            }
        }

        private static void DoEat(string food, Player player)
        {
            // Handles case where we have items {apple, apple, apple} say
            // and the first apple is not edible. Code breaks, but we could have
            // found an edible apple if we had searched the whole list.
            Item? found = player.GetInventory().Find(item => item.IsEdible() && item.GetName() == food);
            if (found != null)
            {
                player.Heal(found.GetHeals());
                player.GetInventory().Remove(found);
            }
            else
            {
                Console.Write($"Could not find an edible item of name: {food}");
            }
        }

    }

    public class Inventory : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Inventory.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            if (player.GetInventory().Count == 0)
            {
                Console.WriteLine("You aren't carrying anything.");
            }
            else
            {
                Console.WriteLine("You have the following items:");
                Console.WriteLine(String.Join("\n", player.GetInventory()));
            }
            if (player.ArmourWorn != null)
            {
                Console.WriteLine($"You are wearning {player.ArmourWorn.GetName()}");
            }
            else
            {
                Console.WriteLine("You are wearing no armour.");
            }
        }
    }

    public class Stats : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Stats.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.WriteLine($"Health: {player.GetHealth()}");
            Console.WriteLine($"Xp: {player.GetXp()}");
        }
    }

    public class Quit : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Quit.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            player.SetHealth(0);
        }
    }

    public class Wear : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Wear.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            player.WearArmour(instructions[1]);
        }
    }

    public class Remove : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Remove.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            player.RemoveArmour();
        }
    }

    public class Command
    {
        
        private static Dictionary<string, Action<List<string>, Player>> actions
                = new Dictionary<string, Action<List<string>, Player>>
                {
                    {"l", Look.Instance },
                    {"look", Look.Instance },
                    {"h", Health.Instance },
                    {"health", Health.Instance },
                    {"m", Move.Instance },
                    {"move", Move.Instance },
                    {"go", Move.Instance },
                    {"t", Take.Instance },
                    {"take", Take.Instance },
                    {"g", Take.Instance },
                    {"get", Take.Instance },
                    {"d", Drop.Instance},
                    {"drop", Drop.Instance },
                    {"a", Attack.Instance},
                    {"attack", Attack.Instance },
                    {"c", Cast.Instance },
                    {"cast", Cast.Instance },
                    {"examine", Examine.Instance },
                    {"eat", Eat.Instance },
                    {"i", Inventory.Instance },
                    {"inventory", Inventory.Instance},
                    {"s", Stats.Instance },
                    {"stats", Stats.Instance},
                    {"quit", Quit.Instance },
                    {"q", Quit.Instance },
                    {"w", Wear.Instance },
                    {"wear", Wear.Instance},
                    {"r", Remove.Instance},
                    {"remove", Remove.Instance}
                };

        public static bool Execute(List<string> instructions, Player player)
        {
            if (actions.ContainsKey(instructions[0]))
            {
                actions[instructions[0]](instructions, player);
            }
            else
            {
                Console.WriteLine($"Sorry, I don't know how to do {instructions[0]}");
            }

            return player.IsDead();
        }
       
    }


}
