using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon.Players;

namespace Dungeon
{
    public class Connection
    {
        private Room RoomFrom;
        private Room RoomTo;
        private String Direction;

        public static void MakeConnection(Room roomFrom, Room roomTo, String direction)
        {
           

            string opposite = string.Empty;
            switch (direction)
            {
                case "north":
                    opposite = "south";
                    break;
                
                case "south":
                    opposite = "north";
                    break;

                case "west":
                    opposite = "east";
                    break;

                case "east":
                    opposite = "west";
                    break;
                default:
                    throw new Exception("Unknown direction building room.");
            }

            Connection from = new Connection(roomFrom, roomTo, direction);
            Connection to = new Connection(roomTo, roomFrom, opposite);
            roomFrom.AddConnection(from);
            roomTo.AddConnection(to);
        }

        private Connection(Room roomFrom, Room roomTo, String direction)
        {
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            Direction = direction;
        }
        public Boolean GoThrough(Player player, String direction)
        {
            if ((player.GetLocation() == RoomFrom) && (direction == Direction))
            {
                player.SetLocation(RoomTo);
                return true;
            }
            else
            {
                return false;
            }
        }
        public String GetDirection()
        {
            return Direction;
        }
    }
}
