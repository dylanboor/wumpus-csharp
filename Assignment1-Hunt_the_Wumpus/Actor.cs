using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    // Abstract so we can never make a generic Actor
    public abstract class Actor
    {
        public int StartingRoom { get; set; }
        public int CurrentRoom { get; set; }

        // Virtual so we can override it
        public virtual void ResetActor(){}  // Performs action when player enters the same room as the Hazard 
    }
}
