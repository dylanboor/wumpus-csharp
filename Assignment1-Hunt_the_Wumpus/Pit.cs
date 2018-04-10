using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class Pit : Hazard
    {
        bool Debug;
        public Pit(int startRoom, bool debug)
        {
            StartingRoom = startRoom;
            CurrentRoom = startRoom;
            EncounterMessage = "YYYIIIIEEEE . . . fell in a pit";
            WarningMessage = "I feel a draft.";
            Debug = debug;
            if (Debug == true) { Console.WriteLine("debug:Pit created with starting room " + StartingRoom); }
        }

        public override void InteractWithPlayer()   // You fall in the pit and die
        {
            Console.WriteLine(EncounterMessage);
        }

        public override void ResetActor()
        {
            CurrentRoom = StartingRoom;
        }
    }
}
