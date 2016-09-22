using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleScape
{
    public abstract class GameObject
    {
        // attributes
        public int xPos;              // holds the x value of the position
        int yPos;              // holds the y value of the position

        // constructor
        public GameObject(int x, int y)
        {
            xPos = x;
            yPos = y;
        }
    }
}
