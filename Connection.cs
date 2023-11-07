using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class Connection
    {
        private Room RoomFrom;
        private Room RoomTo;
        private String Direction;

        public Connection(Room roomFrom, Room roomTo, String direction)
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
