using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_Hunt_the_Wumpus
{
    public class Map
    {
        int[,] Rooms;

        public Map()
        {
            // initialize map data structure
            Rooms = new int[20, 3] {
                {2,5,8},    // Room 1
                {1,3,10},   // Room 2
                {2,4,12},   // Room 3
                {3,5,14},   // Room 4
                {1,4,6},    // Room 5
                {5,7,15},   // Room 6
                {6,8,17},   // Room 7
                {1,7,9},    // Room 8
                {8,10,18},  // Room 9
                {2,9,11},   // Room 10
                {10,12,19}, // Room 11
                {3,11,13},  // Room 12
                {12,14,20}, // Room 13
                {4,13,15},  // Room 14
                {6,14,16},  // Room 15
                {15,17,20}, // Room 16
                {7,16,18},  // Room 17
                {9,17,19},  // Room 18
                {11,18,20}, // Room 19
                {13,16,19}  // Room 20
            };
        }

        public int[] GetAdjacent(int curRoom)
        {
            // return array of adjacent rooms given current room
            int[] output = new int[3];
            for (int x = 0; x < 3; x++)
            {
                output[x] = Rooms[curRoom-1, x];
            }
            return output;
        }

        public bool IsAdjacent(int curRoom, int destRoom)
        {
            bool retVal = false;
            int[] vector = GetAdjacent(curRoom);

            for (int x = 0; x < 3; x++)
            {
                if (vector[x] == destRoom)
                {
                    retVal = true;
                }
            }
            
            return retVal;
        }
    }
}
