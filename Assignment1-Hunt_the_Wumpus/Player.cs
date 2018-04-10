using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class Player : Actor
    {
        bool Debug;
        public int Arrows { get; set; }
        Map map;
        Random randomObj = new Random();

        public Player(int startRoom, Map mapObj, bool debug)
        {
            StartingRoom = startRoom;
            CurrentRoom = startRoom;
            Arrows = 5;
            map = mapObj;
            Debug = debug;
            if (Debug == true) { Console.WriteLine("debug:Player created with starting room " + StartingRoom); }
        }

        public void Move()
        {
            //Console.WriteLine("debug:Player Moving mechanics...");
            int inputRoom = 0;
            string inputString = string.Empty;

            // Loop while inputRoom is not adjacent to the player's current room
            while (!map.IsAdjacent(CurrentRoom, inputRoom))
            {
                Console.WriteLine("Where to?");
                inputString = Console.ReadLine();

                // If can't parse input to an integer, display error. Otherwise it parses inputString into inputRoom
                if (!Int32.TryParse(inputString, out inputRoom))
                {
                    Console.WriteLine("That's not a room number!");
                }
                // Or if the input was an integer, but not a connected room:
                else if (!map.IsAdjacent(CurrentRoom, inputRoom))
                {
                    Console.WriteLine("There's no way to get there!");
                }
            }
            CurrentRoom = inputRoom;
        }

        public void WarpTo(int x)
        {
            CurrentRoom = x;
        }

        public int[] Shoot(int numShots) // Shoots crooked arrow, goes through 5 rooms
        {
            int inputRoom = 0;
            string inputString = string.Empty;
            int[] vector;
            int[] shots = new int[numShots];
            int rand;
            int ctr = 0;
            bool validInput = false;    // Flag needed to loop proper number of times

            while (ctr < numShots)
            {
                if (Debug == true)  // Debug output the shot rooms
                {
                    Console.Write("debug:Shot array = ");
                    for (int y = 0; y < numShots; y++) { Console.Write("{0} ", shots[y]); }
                    Console.WriteLine();
                }
                
                Console.Write("Where to shoot? ");
                inputString = Console.ReadLine();

                if (!Int32.TryParse(inputString, out inputRoom) || inputRoom < 1 || inputRoom > 20) // If room is not an int or out of range
                {
                    Console.WriteLine("That's not a room number!");
                    validInput = false;
                }
                else
                {
                    if (ctr == 0) // If it's the 1st input, take from CurrentRoom
                    {
                        if (inputRoom == CurrentRoom)
                        {
                            Console.WriteLine("Arrows aren't that crooked! Try another room.");
                            validInput = false;
                        }
                        else if (map.IsAdjacent(CurrentRoom, inputRoom))    // If your input room was adjacent to CurrentRoom
                        { 
                            if (Debug == true) { Console.WriteLine("debug:Your input was adjacent to the current room"); }

                            shots[ctr] = inputRoom;
                            validInput = true;
                        }
                        else
                        {
                            if (Debug == true) { Console.WriteLine("debug:Your input was NOT adjacent to the current room"); }

                            vector = map.GetAdjacent(CurrentRoom);   // Get adjacent rooms to CurrentRoom
                            rand = randomObj.Next(3);  // generates 0, 1, 2
                            shots[ctr] = vector[rand];   // Set the target to a random room adjacent to CurrentRoom
                            validInput = true;

                            if (Debug == true) { Console.WriteLine("debug:You instead shot " + shots[ctr]); }
                        }
                    }
                    else if (ctr <= 1) // If it's the 2nd input, take from the room that was last shot and restrict CurrentRoom
                    {
                        if (map.IsAdjacent(shots[ctr - 1], inputRoom)) {
                            if (Debug == true) { Console.WriteLine("debug:Your input was adjacent to the last shot room"); }

                            if (inputRoom == CurrentRoom) {
                                Console.WriteLine("Arrows aren't that crooked! Try another room.");
                                validInput = false;
                            }
                            else {
                                shots[ctr] = inputRoom;
                                validInput = true;
                            }
                        }
                        else {
                            if (Debug == true) { Console.WriteLine("debug:Your input was NOT adjacent to the last shot room"); }

                            vector = map.GetAdjacent(shots[ctr - 1]);   // Get adjacent rooms to the last shot
                            rand = randomObj.Next(3);  // generates 0, 1, 2
                            while (shots.Contains(vector[rand]) || vector[rand] == CurrentRoom) {   // generates 0, 1, 2 excluding rooms already in the arrow's path
                                rand = randomObj.Next(3);  
                            }
                            shots[ctr] = vector[rand];   // add the random path to shots
                            validInput = true;

                            if (Debug == true) { Console.WriteLine("debug:You instead shot " + shots[ctr]); }
                        }
                    }
                    else // If it's after the 2nd input, take from the room that was last shot, and prevent random values that were already in the arrow's path
                    {
                        if (map.IsAdjacent(shots[ctr - 1], inputRoom)){
                            if (Debug == true) { Console.WriteLine("debug:Your input was adjacent to the last shot room"); }

                            if (inputRoom == shots[ctr - 2]) {  // If the shot would go A to B to A
                                Console.WriteLine("Arrows aren't that crooked! Try another room.");
                                validInput = false;
                            }
                            else {
                                shots[ctr] = inputRoom;
                                validInput = true;
                            }
                        }
                        else {
                            if (Debug == true) { Console.WriteLine("debug:Your input was NOT adjacent to the last shot room"); }

                            vector = map.GetAdjacent(shots[ctr - 1]);   // Get adjacent rooms to the last shot
                            rand = randomObj.Next(3);  // generates 0, 1, 2
                            while (shots.Contains(vector[rand])) {  // generates 0, 1, 2 excluding rooms already in the arrow's path
                                rand = randomObj.Next(3);  // generates 0, 1, 2 until 
                            }
                            shots[ctr] = vector[rand];   // add the random path to shots
                            validInput = true;

                            if (Debug == true) { Console.WriteLine("debug:You instead shot " + shots[ctr]); }
                        }
                    }

                    if (validInput) { ctr++; }  // If the input was valid, only then increment the counter
                }
            }
            Arrows -= 1;
            return shots;
        }

        public override void ResetActor()
        {
            CurrentRoom = StartingRoom;
            Arrows = 5;
        }
    }
}
