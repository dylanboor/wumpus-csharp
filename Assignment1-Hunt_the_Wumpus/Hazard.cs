using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    // Abstract so we can never make a generic Hazard
    public abstract class Hazard : Actor
    {
        public string EncounterMessage { get; set; }
        public string WarningMessage { get; set; }
        
        // Virtual so we can override it
        public virtual void InteractWithPlayer(){}  // Displays encounter message when player is in the same room as the hazard

        public void WarnPlayer()
        {
            Console.WriteLine(WarningMessage);
        }
    }
}
