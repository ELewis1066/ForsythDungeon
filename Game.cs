using Dungeon.Creatures;
using Dungeon.Items;
using Dungeon.Players;


namespace Dungeon
{
    public class Game
    {
        private const int STARTING_HEALTH = 750;
        public void PlayGame()
        {
            String command = "";
            Boolean gameOver = false;

            // initialising the game
            Console.WriteLine("Welcome Message...");
            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.");
            Room iceRoom = new Room("Winter has englufed you, you see protruding ice crystals hugging the room.");
            Room shadowRoom = new Room("You feel uneasy... like somethings watching you in this vanta black room");
            Room throneRoom = new Room("You are in the room touched by Midas himself, a grand throne lies in front of you with a mysterious figure sitting in it");

            FoodItem healthPotion = new FoodItem("healthPotion", "meticulously crafted by the finest apothecary", 175);
            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 15);
            FoodItem susApple = new FoodItem("susApple", "the apple rots with bugs embedded in it", -30);

            Item stoneApple = new Item("apple", "a beautiful apple made of stone.");
            Item water = new Item("water", "Everian, the best!.");
            Item glass = new Item("glass", "glassy glass.");

            WeaponItem sword = new WeaponItem("sword", "a basic iron sword crafted by dwarves.", 2);
            WeaponItem greatSword = new WeaponItem("greatSword", "a weapon crafted by the gaints only for the strong.", 3);

            ArmourItem chainMail = new ArmourItem("chainMail", "Basic Chain mail armour, crafted by royal blacksmiths.", 10);

            

            DragonCreature dragon = new DragonCreature();
            IceGolemCreature iceGolem = new IceGolemCreature();
            ShadowKnightCreature shadowKnight = new ShadowKnightCreature();
            CelestialHolyKingCreature celestialHolyKingCreature = new CelestialHolyKingCreature();

            /*
            startRoom.AddConnection(new Connection(startRoom, lavaRoom, "north"));
            lavaRoom.AddConnection(new Connection(lavaRoom, startRoom, "south"));
            */

            // Handles all the connection logic, makes sure that the rooms are 
            // connected properly.

            // starting room stuff
            startRoom.AddItem(apple);
            startRoom.AddItem(glass);

            // lava room stuff
            lavaRoom.AddCreature(dragon);
            lavaRoom.AddItem(susApple);
            lavaRoom.AddItem(sword);

            // ice room stuff
            iceRoom.AddCreature(iceGolem);
            iceRoom.AddItem(apple);
            iceRoom.AddItem(greatSword);

            // shadow room stuff
            shadowRoom.AddCreature(shadowKnight);

            // throne room
            throneRoom.AddCreature(celestialHolyKingCreature);
            throneRoom.AddItem(healthPotion);


            Player pc = new Player(STARTING_HEALTH);
            pc.AddToInventory(apple);
            pc.AddToInventory(stoneApple);

         
            List<Room> rooms = new List<Room>
            {
                 new Room("You are in a gloomy cellar.") ,
                 new Room("You are in a room with a hole in floor, it smells."),
                 new Room("You are in a dark dungeon."),
                 new Room("You are in a cellar with barrels of beer."),
                 new Room("You are in a small room with torches on the wall."),
                 lavaRoom,
                 iceRoom,
                 shadowRoom,
                 throneRoom,
                 startRoom // keep start room at the end.
            };

            List<Creature> orcs = new List<Creature>();
            for (int i = 0; i < 5; i++)
            {
                orcs.Add(new Creature($"orc_{i}", 150));

            }
            /* Randomly distribute the orcs the rooms? */
            Random random = new Random();
            foreach (Creature orc in orcs)
            {
                var randomRoom = rooms[random.Next(rooms.Count - 1)];
                randomRoom.AddCreature(orc);
            }

    
            PruferGenerator generator = new PruferGenerator();
            Room start = generator.Generate(rooms);
        
           
            //startRoom.AddItem(chainMail);
            KeyItem key1 = new KeyItem("goldKey", "A large gold key.", 123);
            ChestItem chest = new ChestItem("oldChest", "A large old chest", 123);
            chest.PutItemInChest(chainMail);

            startRoom.AddItem(chest);
            startRoom.AddItem(key1);

            pc.SetLocation(start);

            // play the game
            while (!gameOver)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("what would you like to do? ");

                Console.ForegroundColor = ConsoleColor.Red;
                command = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("\u001b[3m");


                // This shows that the game state is just the player class at the moment.
                // i.e. entire game state is managed by 'Player'.

                // So, we may have a new class
                // 
                // GameState that includes a Player object and NPCs etc..
                // to support 'turns'.
                gameOver = Command.Execute(command.Split(' ').ToList(), pc);
                Console.WriteLine(" \u001b[0m");
            }

            // finish off nicely and close down

            Console.ForegroundColor = ConsoleColor.Blue;
            string endingMessage = @"
 =================================================================================
 ████████╗██╗░░██╗░█████╗░███╗░░██╗██╗░░██╗░██████╗  ███████╗░█████╗░██████╗░                                  
 ╚══██╔══╝██║░░██║██╔══██╗████╗░██║██║░██╔╝██╔════╝  ██╔════╝██╔══██╗██╔══██╗                                  
 ░░░██║░░░███████║███████║██╔██╗██║█████═╝░╚█████╗░  █████╗░░██║░░██║██████╔╝                                  
 ░░░██║░░░██╔══██║██╔══██║██║╚████║██╔═██╗░░╚═══██╗  ██╔══╝░░██║░░██║██╔══██╗                                  
 ░░░██║░░░██║░░██║██║░░██║██║░╚███║██║░╚██╗██████╔╝  ██║░░░░░╚█████╔╝██║░░██║                                  
 ░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝╚═╝░░╚═╝╚═════╝░  ╚═╝░░░░░░╚════╝░╚═╝░░╚═╝                                  
                                                                                                                
 ██████╗░██╗░░░░░░█████╗░██╗░░░██╗██╗███╗░░██╗░██████╗░                                                         
 ██╔══██╗██║░░░░░██╔══██╗╚██╗░██╔╝██║████╗░██║██╔════╝░                                                         
 ██████╔╝██║░░░░░███████║░╚████╔╝░██║██╔██╗██║██║░░██╗░                                                        
 ██╔═══╝░██║░░░░░██╔══██║░░╚██╔╝░░██║██║╚████║██║░░╚██╗                                                         
 ██║░░░░░███████╗██║░░██║░░░██║░░░██║██║░╚███║╚██████╔╝                                                         
 ╚═╝░░░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░╚═╝╚═╝░░╚══╝░╚═════╝░                                                         
                                                                                                                
 ██████╗░██╗░░░██╗███╗░░██╗░██████╗░███████╗░█████╗░███╗░░██╗██╗                   
 ██╔══██╗██║░░░██║████╗░██║██╔════╝░██╔════╝██╔══██╗████╗░██║██║               
 ██║░░██║██║░░░██║██╔██╗██║██║░░██╗░█████╗░░██║░░██║██╔██╗██║██║                   
 ██║░░██║██║░░░██║██║╚████║██║░░╚██╗██╔══╝░░██║░░██║██║╚████║╚═╝                    
 ██████╔╝╚██████╔╝██║░╚███║╚██████╔╝███████╗╚█████╔╝██║░╚███║██╗                     
 ╚═════╝░░╚═════╝░╚═╝░░╚══╝░╚═════╝░╚══════╝░╚════╝░╚═╝░░╚══╝╚═╝                     
 ================================================================================
";
            Console.WriteLine(endingMessage);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.ReadLine();
        }
    }
}
