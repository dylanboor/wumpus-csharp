using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            // Set Debug messages
            bool Debug = false;

            // Start the game
            Game game = new Game(Debug);
        }
    }
}
