using Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Game
    {
        private const int STARTING_HEALTH = 100;
        public void PlayGame()
        {
            String command = "";
            Boolean gameOver = false;

            // initialising the game
            Console.WriteLine("Welcome Message...");
            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.");

            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10);
            FoodItem redApple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10);

            Item stoneApple = new Item("apple", "a beautiful apple made of stone.");
            Item water = new Item("water", "Everian, the best!.");
            Item glass = new Item("glass", "glassy glass.");

            WeaponItem sword = new WeaponItem("sword", "a basic iron sword crafted by dwarves.", 2);
            DragonCreature dragon = new DragonCreature();

            /*
            startRoom.AddConnection(new Connection(startRoom, lavaRoom, "north"));
            lavaRoom.AddConnection(new Connection(lavaRoom, startRoom, "south"));
            */

            // Handles all the connection logic, makes sure that the rooms are 
            // connected properly.
            Connection.MakeConnection(startRoom, lavaRoom, "north"); 

            startRoom.AddItem(apple);
            startRoom.AddItem(glass);
            lavaRoom.AddCreature(dragon);
            lavaRoom.AddItem(sword);

            Player pc = new Player(STARTING_HEALTH);
            pc.SetLocation(startRoom);
            pc.AddItem(redApple);
            pc.AddItem(stoneApple);


            // play the game
            while (!gameOver)
            {
                Console.WriteLine();
                Console.Write("What would you like to do? ");
                command = Console.ReadLine();
                gameOver = pc.DoCommand(command);
            }

            // finish off nicely and close down
            Console.WriteLine("Thank you for playing Dungeon! See you again soon, brave dungeoneer.");

            Console.ReadLine();
        }
    }
}
