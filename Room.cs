using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Creatures;
using Dungeon.Items;

namespace Dungeon
{
    public class Room
    {
        // Make this readable, GetDescription should be moved to 'ToString'.
        public String Description { get; set; }

        private List<Item> Contents = new List<Item>();
        private List<Creature> Creatures = new List<Creature>();
        private List<Connection> Connections = new List<Connection>();


        public Room(String description)
        {
            Description = description;
        }

        
        public void AddItem(Item item)
        {
            Contents.Add(item);
        }
        public void AddCreature(Creature creature)
        {
            Creatures.Add(creature);
        }

        public List<Creature> GetCreatures()
        {
            return Creatures;
        }
        public void RemoveCreature(Creature creature)
        {
            Creatures.Remove(creature);
        }

        public bool ContainsName(String name)
        {
            foreach (Item item in Contents)
            {
                if (item.GetName().Equals(name)) return true;
            }
            return false;
        }
        public Item GetItem(String name)
        {
            foreach (Item item in Contents)
            {
                if (item.GetName() == name)
                {
                    return item;
                }
            }

            return null;
        }

        public Item RemoveItem(String name)
        {
            foreach (Item item in Contents)
            {
                if (item.GetName() == name)
                {
                    Contents.Remove(item);
                    return item;
                }
            }

            return null;
        }
        public void AddConnection(Connection connection)
        {
            Connections.Add(connection);
        }
        public List<String> GetDirections()
        {
            // This returns the directions for each Connection in index order according to the Connections List
            List<String> directions = new List<String>();

            foreach (Connection connection in Connections)
            {
                directions.Add(connection.GetDirection());
            }

            return directions;
        }

        public List<Connection> GetConnections()
        {
            return Connections;
        }
        public String GetDescription()
        {
            String items = "\n";
            if (Contents.Count > 0)
            {
                if (Contents.Count == 1)
                {
                    items += $"You can see the following item: {Contents[0].GetName()}.";
                }
                else
                {
                    items += $"You can see the following items: {Contents[0].GetName()}";
                    for (int i = 1; i < Contents.Count - 1; i++)
                    {
                        items += ", " + Contents[i].GetName();
                    }
                    items += $" and {Contents[Contents.Count - 1].GetName()}.";
                }
            }
            else
            {
                items = "";
            }

            String creatures = "\n";
            if (Creatures.Count > 0)
            {
                if (Creatures.Count == 1)
                {
                    creatures += $"You can see the following creature: {Creatures[0].GetName()}.";
                }
                else
                {
                    creatures += $"You can see the following creatures: {Creatures[0].GetName()}";
                    for (int i = 1; i < Creatures.Count - 1; i++)
                    {
                        creatures += ", " + Creatures[i].GetName();
                    }
                    creatures += $" and {Creatures[Creatures.Count - 1].GetName()}.";
                }
            }
            else
            {
                creatures = "";
            }

            String exits = "\n";
            if (Connections.Count > 0)
            {
                if (Connections.Count == 1)
                {
                    exits += $"There is an exit to the {Connections[0].GetDirection()}.";
                }
                else
                {
                    exits += $"There are exits to the {Connections[0].GetDirection()}";
                    for (int i = 1; i < Connections.Count - 1; i++)
                    {
                        exits += ", " + Connections[i].GetDirection();
                    }
                    exits += $" and {Connections[Connections.Count - 1].GetDirection()}.";
                }
            }
            else
            {
                exits += "There are no visible exits.";
            }

            return Description + items + creatures + exits;
        }
        public List<Item> GetContents()
        {
            return Contents;
        }
        public void SetDescription(String description)
        {
            Description = description;
        }
    }
}
