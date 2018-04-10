using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    class Game
    {
        static string INTRO = "===================\nWelcome to HUNT THE WUMPUS\n\nThe Wumpus lives in a cave of 20 rooms. Each room has 3 tunnels leading to other rooms.\nBottomless pits will kill you.\nSuperBats will warp you randomly.\nThe Wumpus is not bothered by the hazards and is usually asleep.\n===================\n";
        bool Debug;
        Map map;
        Player player;
        Wumpus wumpus;
        SuperBat superBats1;
        SuperBat superBats2;
        Pit pit1;
        Pit pit2;

        bool ResetSpawns = true;
        bool CloseGame = false;
        bool GameInProgress = true;
        Random randomObj = new Random();

        public Game(bool debug)
        {
            Debug = debug;
            while (CloseGame != true)
            {
                InitializeGame();   // Places 1 wumpus, 2 superbats, and 2 bottomless pits on the map

                while (GameInProgress == true)  // Turn loop
                {
                    Console.WriteLine();
                    if (wumpus.IsAwake == true) { wumpus.Move(); }  // Move the wumpus if awake

                    CheckForAdjacentHazards();  // Check neighboring rooms for hazards

                    /*Don't need this section unless you think it's funny to die if you spawn in a pit.
                    CheckForHazardCollision();  // Check for whether the player started their turn on a hazard
                    if (GameInProgress == false) { break; } // If you lost the game already, break out of the loop*/

                    Console.WriteLine("You are in room " + player.CurrentRoom); // Display current room

                    // Display Adjacent Rooms
                    int[] adj = map.GetAdjacent(player.CurrentRoom);
                    Console.WriteLine("Tunnels lead to {0} {1} {2}", adj[0], adj[1], adj[2]);

                    string input = UserInput(); // Perform user input for Moving, Shooting, or quitting
                    if (input.Equals("M"))
                    {
                        player.Move();   // Move the player
                        CheckForHazardCollision();  // Check for whether the player has hit a hazard
                    }
                    else if (input.Equals("S")) 
                    {
                        CheckHits(player.Shoot(HandleShotInput()));    // After shoot is input completely, pass input to player Shoot(), then check each room for hits:

                        if (GameInProgress == true && wumpus.IsAwake == false)    // Shooting an arrow wakes the wumpus
                        {
                            wumpus.Wake();   // Wumpus wakes up and moves when you shoot an arrow anywhere on the map
                            wumpus.Move();
                            if (player.CurrentRoom == wumpus.CurrentRoom)
                            {
                                WumpusInYourRoom(); // case: He could end up in your room after you shoot, killing you
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public void InitializeGame()
        {
            GameInProgress = true;
            map = new Map();
            Console.WriteLine(INTRO);

            if (ResetSpawns == true)
            {
                // Make new player, wumpus, 2 superbats, and 2 pits in random, different rooms
                int playerSpawn = randomObj.Next(19) + 1;

                int wumpusSpawn = randomObj.Next(19) + 1;
                while (wumpusSpawn == playerSpawn) {
                    wumpusSpawn = randomObj.Next(19) + 1;
                }

                int superBats1Spawn = randomObj.Next(19) + 1;
                while (superBats1Spawn == playerSpawn || superBats1Spawn == wumpusSpawn) {
                    superBats1Spawn = randomObj.Next(19) + 1;
                }

                int superBats2Spawn = randomObj.Next(19) + 1;
                while (superBats2Spawn == playerSpawn || superBats2Spawn == wumpusSpawn || superBats2Spawn == superBats1Spawn)
                {
                    superBats2Spawn = randomObj.Next(19) + 1;
                }

                int pit1Spawn = randomObj.Next(19) + 1;
                while (pit1Spawn == playerSpawn || pit1Spawn == wumpusSpawn || pit1Spawn == superBats1Spawn || pit1Spawn == superBats2Spawn) {
                    pit1Spawn = randomObj.Next(19) + 1;
                }

                int pit2Spawn = randomObj.Next(19) + 1;
                while (pit2Spawn == playerSpawn || pit2Spawn == wumpusSpawn || pit2Spawn == superBats1Spawn || pit2Spawn == superBats2Spawn || pit2Spawn == pit1Spawn) {
                    pit2Spawn = randomObj.Next(19) + 1;
                }

                player = new Player(playerSpawn, map, Debug);   // player and wumpus objects need to look at the map to shoot and move
                wumpus = new Wumpus(wumpusSpawn, map, Debug);
                superBats1 = new SuperBat(superBats1Spawn, Debug);
                superBats2 = new SuperBat(superBats2Spawn, Debug);
                pit1 = new Pit(pit1Spawn, Debug);
                pit2 = new Pit(pit2Spawn, Debug);
            }
            else if (ResetSpawns == false)
            {
                // Don't make new objects, just reset to original rooms and initial game state
                player.ResetActor();
                wumpus.ResetActor();
                superBats1.ResetActor();
                superBats2.ResetActor();
                pit1.ResetActor();
                pit2.ResetActor();

                if (Debug) {
                    Console.WriteLine("debug:Player reset to starting room " + player.CurrentRoom);
                    Console.WriteLine("debug:Wumpus reset to starting room " + wumpus.CurrentRoom);
                    Console.WriteLine("debug:SuperBats reset to starting room " + superBats1.CurrentRoom);
                    Console.WriteLine("debug:SuperBats reset to starting room " + superBats2.CurrentRoom);
                    Console.WriteLine("debug:Pit1 reset to starting room " + pit1.CurrentRoom);
                    Console.WriteLine("debug:Pit2 reset to starting room " + pit2.CurrentRoom);
                }
            }
        }

        public string UserInput()
        {
            string inputString = string.Empty;
            string firstLetter = string.Empty;

            while (!(firstLetter.Equals("M") || firstLetter.Equals("S") || firstLetter.Equals("Q")))
            {
                Console.WriteLine("M for move, S for shoot, Q for quit");

                inputString = Console.ReadLine();
                firstLetter = inputString.First().ToString().ToUpper(); // Uses Linq to get 1st letter

                if (firstLetter.Equals("Q"))
                {
                    if (QuitGamePrompt() == true)
                    {
                        QuitGame();
                    }
                    else
                    {
                        firstLetter = string.Empty; // Reset input value and go back into loop
                    }    
                }
            }
            return firstLetter;
        }

        public int HandleShotInput()
        {
            string inputString = string.Empty;
            int numShots = 0;
            // Loop for entering number of shots
            while (numShots < 1 || numShots > 5)
            {
                Console.Write("Number of rooms to shoot(1-5)? ");
                inputString = Console.ReadLine();

                // If can't parse input to an integer, display error. Otherwise it parses inputString into numShots
                if (!Int32.TryParse(inputString, out numShots))
                {
                    Console.WriteLine("That's not a room number!");
                }
                else if (numShots < 1 || numShots > 5)
                {
                    Console.WriteLine("Please enter a number between 1 and 5.");
                }
            }
            return numShots;
        }

        public void CheckHits(int[] shots)
        {
            if (shots.Contains(wumpus.CurrentRoom))
            {
                wumpus.ShotByArrow();   // Displays message
                WinGame();
            }
            else if (player.Arrows == 0)
            {
                Console.WriteLine("You're out of arrows! You can't survive!");
                LoseGame();
            }
            else if (shots.Contains(player.CurrentRoom))
            {
                Console.WriteLine("Ouch! Shot yourself!!");
                LoseGame();
            }
            else
            {
                Console.WriteLine("Missed!");
            }
        }

        public void CheckForAdjacentHazards()
        {
            int[] vector = map.GetAdjacent(player.CurrentRoom);
            bool wumpusNear = false;
            bool superBatsNear = false;
            bool pitNear = false;

            for (int x = 0; x < 3; x++)
            {
                if (vector[x] == wumpus.CurrentRoom)
                {
                    wumpusNear = true;
                }
                if ((vector[x] == superBats1.CurrentRoom) || (vector[x] == superBats2.CurrentRoom))
                {
                    superBatsNear = true;
                }
                if ((vector[x] == pit1.CurrentRoom) || (vector[x] == pit2.CurrentRoom))
                {
                    pitNear = true;
                }
            }
            // Output warning messages if near hazards
            if (wumpusNear == true) { wumpus.WarnPlayer(); }
            if (superBatsNear == true) { superBats1.WarnPlayer(); }
            if (pitNear == true) { pit1.WarnPlayer(); }
        }

        public void CheckForHazardCollision()
        {
            if (player.CurrentRoom == wumpus.CurrentRoom)
            {
                WumpusInYourRoom(); // Wumpus wakes up and moves if asleep; if he's still in your room, you die
            }
            else if (player.CurrentRoom == superBats1.CurrentRoom)
            {
                superBats1.InteractWithPlayer(); // Display superbats message
                int x = randomObj.Next(19) + 1;
                player.WarpTo(x);   // Warp player to random room x
                CheckForHazardCollision();
            }
            else if (player.CurrentRoom == superBats2.CurrentRoom)
            {
                superBats2.InteractWithPlayer(); // Display superbats message
                int x = randomObj.Next(19) + 1;
                player.WarpTo(x);   // Warp player to random room x
                CheckForHazardCollision();
            }
            else if (player.CurrentRoom == pit1.CurrentRoom)
            {
                pit1.InteractWithPlayer();  // Display pit message
                LoseGame();
            }
            else if (player.CurrentRoom == pit2.CurrentRoom)
            {
                pit2.InteractWithPlayer();  // Display pit message
                LoseGame();
            }
        }

        public void WumpusInYourRoom()
        {
            if (wumpus.IsAwake == false)   // If Wumpus was asleep, he wakes up and tries to move
            {
                Console.WriteLine("...Oops! Bumped a Wumpus.");
                wumpus.Wake();
                wumpus.Move();
            }

            if (player.CurrentRoom == wumpus.CurrentRoom)   // If he's still in the same room as you
            {
                wumpus.InteractWithPlayer();
                LoseGame();
            }
        }

        public void WinGame()
        {
            Console.WriteLine("\nHeeheehee - the Wumpus'll getcha next time!!");
            GameInProgress = false;
            EndGamePrompt();
        }

        public void LoseGame()
        {
            Console.WriteLine("\nGAME OVER");
            GameInProgress = false;
            EndGamePrompt();
        }

        public void EndGamePrompt()
        {
            string inputString = string.Empty;
            string firstLetter = string.Empty;

            while (!(firstLetter.Equals("1") || firstLetter.Equals("2") || firstLetter.Equals("Q")))
            {
                Console.WriteLine("Play again with:\n1. Random New Spawns\n2. The same Spawn Locations\nQ to quit");

                inputString = Console.ReadLine();
                firstLetter = inputString.First().ToString().ToUpper(); // Uses Linq to get 1st letter

                if (firstLetter.Equals("1"))
                {
                    if (Debug == true) { Console.WriteLine("debug:Set spawns to random."); }
                    ResetSpawns = true;
                }
                else if (firstLetter.Equals("2"))
                {
                    if (Debug == true) { Console.WriteLine("debug:Set spawns to non-random."); }
                    ResetSpawns = false;
                }
                else if (firstLetter.Equals("Q"))
                {
                    if (QuitGamePrompt() == true) { QuitGame(); }
                    else { firstLetter = string.Empty; } // Reset input value and go back into loop
                }
            }
        }

        public void QuitGame()
        {
            GameInProgress = false;
            CloseGame = true;
            //Environment.Exit(0);
        }

        public bool QuitGamePrompt()
        {
            bool retVal = false;
            string inputString = string.Empty;
            string firstLetter = string.Empty;

            while (!(firstLetter.Equals("Y") || firstLetter.Equals("N")))
            {
                Console.WriteLine("Are you sure you want to quit? Y or N");
                inputString = Console.ReadLine();
                firstLetter = inputString.First().ToString().ToUpper(); // Uses Linq to get 1st letter

                if (firstLetter.Equals("Y")) { retVal = true; }
                else if (firstLetter.Equals("N")) { retVal = false; }
            }
            return retVal;
        }
    }
}
