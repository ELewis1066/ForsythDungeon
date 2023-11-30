using Dungeon.Players;

namespace Dungeon
{
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
                    {"remove", Remove.Instance},
                    {"unlock", Unlock.Instance }
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
