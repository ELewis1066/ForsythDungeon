using Dungeon.Players;
using Dungeon.Items;
using Dungeon.Creatures;


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
            if (instructions.Count == 2)
            {
                // Look for the item in the room.
                if (player.GetLocation().ContainsName(nameOfItem))
                {
                    Item item = player.GetLocation().GetItem(nameOfItem);
                    if (item.CanPickUp())
                    {
                        Item removedItem = player.GetLocation().RemoveItem(nameOfItem);
                        player.AddToInventory(removedItem);
                    }
                    else
                    {
                        Console.WriteLine("The item is too heavy to pick up.");
                    }
                }
                else
                {
                    Console.WriteLine($"Item {nameOfItem} does not exist in the location.");
                }
            }
            else if (instructions.Count == 3)
            {
                // Look for the item in a chest.
                // 1. Find the chest.
                // 2. If the Item is in the chest, remove the item from the chest and put into
                //    player inventory.
                string nameOfChest = instructions[2];
                Item chestItem = player.GetLocation().GetItem(nameOfChest);
                if (chestItem != null && !chestItem.CanPickUp())
                {
                    ChestItem chest = (ChestItem)chestItem;
                    Item? found = chest.ChestContents.Find(item => item.GetName() == nameOfItem);
                    if (found != null)
                    {
                        chest.ChestContents.Remove(found);
                        player.AddToInventory(found);
                        Console.WriteLine($"You have addded {nameOfItem} to your inventory.");
                    }
                    else
                    {
                        Console.WriteLine($"Could not find {nameOfItem} in the chest.");
                    }
                }
                else
                {
                    Console.WriteLine($"Sorry {nameOfChest} isn't a chest.");
                }

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

                SpellBookItem? spellBook = player.GetSpellBook();
                if (spellBook == null)
                {
                    Console.WriteLine("Sorry you don't have any spell books.");
                    return;
                }

                if (spellBook.ContainsSpell(spell))
                {
                    foreach (Creature creature in player.GetLocation().GetCreatures())
                    {
                        if (creature.GetName() == target)
                        {
                            bool dead = creature.TakeSpellDamage(spell, spellBook.GetDamage(spell));
                            if (dead)
                            {
                                Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                creatureToRemove = creature;
                            }
                            else
                            {
                                int damage = creature.GetAttackDamage();
                                Console.WriteLine($"The creature has attacked and caused {damage} damage");
                                Console.WriteLine($"The creature has {creature.GetHealth()} HP.");
                                player.TakeDamage(damage);
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

    public class Unlock : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Unlock.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            // unlock, chest, key
            string chestName = instructions[1];
            string keyName = instructions[2];
            // 1. find the key in your inventory.
            var keyResult = player.GetInventory().Find(x => x.GetName() == keyName);
            if (keyResult != null)
            {
                KeyItem keyItem = (KeyItem)keyResult;

                var chest = player.GetLocation().GetContents().Find(item => item.GetName() == chestName);

                if (chest != null)
                {
                    ChestItem chestItem = (ChestItem)chest;
                    if (chestItem.IsLocked)
                    {

                        chestItem.UnlockChest(keyItem);


                        if (!chestItem.IsLocked)
                        {
                            chestItem.DisplayContent();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The chest {chestName} is already unlocked.");
                    }
                }
                else
                {
                    Console.WriteLine($"No chest named {chestName} found in the location.");
                }
            }
            else
            {
                Console.WriteLine($"No key named {keyName} found in your inventory.");
            }

        }


    }
}
