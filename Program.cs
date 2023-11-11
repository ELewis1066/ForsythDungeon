using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Dungeon
{
    internal class Program
    {
        static ConsoleColor originalBackgroundColor;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            string startingScreen = @"
 ==============================================================
| ██████╗░██╗░░░██╗███╗░░██╗░██████╗░███████╗░█████╗░███╗░░██╗ |
| ██╔══██╗██║░░░██║████╗░██║██╔════╝░██╔════╝██╔══██╗████╗░██║ |
| ██║░░██║██║░░░██║██╔██╗██║██║░░██╗░█████╗░░██║░░██║██╔██╗██║ |
| ██║░░██║██║░░░██║██║╚████║██║░░╚██╗██╔══╝░░██║░░██║██║╚████║ |
| ██████╔╝╚██████╔╝██║░╚███║╚██████╔╝███████╗╚█████╔╝██║░╚███║ |
| ╚═════╝░░╚═════╝░╚═╝░░╚══╝░╚═════╝░╚══════╝░╚════╝░╚═╝░░╚══╝ |
 ==============================================================
";
            Console.WriteLine(startingScreen, Console.BackgroundColor, Console.ForegroundColor);
            Console.BackgroundColor = originalBackgroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Game dungeon = new Game();
            Console.WriteLine("\x1B[3m");
            dungeon.PlayGame();
            Console.WriteLine("\x1B[0m");
        }
    }
}
