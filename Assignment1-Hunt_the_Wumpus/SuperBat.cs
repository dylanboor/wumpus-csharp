using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class SuperBat : Hazard
    {
        bool Debug;
        public SuperBat(int startRoom, bool debug)
        {
            StartingRoom = startRoom;
            CurrentRoom = startRoom;
            EncounterMessage = "Zap--Super Bat snatch! Elsewhereville for you!";
            WarningMessage = "Bats nearby"; // just a string so we don't need setWarningMessage method
            Debug = debug;
            if (Debug == true) { Console.WriteLine("debug:Bats created with starting room " + StartingRoom); }
        }

        public override void InteractWithPlayer()   // You will be warped to another room
        {
            Console.WriteLine(EncounterMessage);
        }

        public override void ResetActor()
        {
            CurrentRoom = StartingRoom;
        }
    }
}
