using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class Wumpus : Hazard
    {
        bool Debug;
        public bool IsAwake { get; set; }
        Random randomObj = new Random();
        Map map;

        public Wumpus(int startRoom, Map mapObj, bool debug)
        {
            StartingRoom = startRoom;
            CurrentRoom = startRoom;
            EncounterMessage = "The Wumpus killed you!";
            WarningMessage = "I smell a Wumpus.";
            IsAwake = false;    // Wumpus starts asleep
            map = mapObj;
            Debug = debug;
            if (Debug == true) { Console.WriteLine("debug:Wumpus created with starting room " + StartingRoom); }
        }

        public override void InteractWithPlayer()   // Wumpus kills you. Game Over
        {
            Console.WriteLine(EncounterMessage);
        }

        public override void ResetActor()
        {
            CurrentRoom = StartingRoom;
            IsAwake = false;
        }

        public void ShotByArrow()
        {
            Console.WriteLine("Aha! You got the Wumpus!\n");  // Dies, you win the game.
        }

        public void Move()
        {
            if (Debug == true) { Console.WriteLine("debug:Wumpus is trying to move"); }
            int x = randomObj.Next(4);  // generates 0, 1, 2, or 3
            int[] vector = map.GetAdjacent(CurrentRoom); // Gets adjacent rooms to wumpus

            if (x < 3)  // 0, 1, or 2 = 75% chance
            {
                CurrentRoom = vector[x]; // Move to 1st, 2nd, or 3rd adjacent room
                if (Debug == true) { Console.WriteLine("debug:Wumpus moved to " + CurrentRoom); }
            }
            else   // Wumpus stays still 25% of the time (if a 3 is rolled, 1/4 chance)
            {
                if (Debug == true) { Console.WriteLine("debug:Wumpus didn't move anywhere"); }
            }
        }

        public void Wake()
        {
            IsAwake = true;
            if (Debug == true) { Console.WriteLine("debug:Wumpus just woke up"); }
        }


    }
}
