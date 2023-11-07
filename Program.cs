using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace Dungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game dungeon = new Game();
            dungeon.PlayGame();
        }
    }

}
