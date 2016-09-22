using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JungleScape
{
    public abstract class GameObject
    {
        // attributes
        Point location;              // holds the y value of the position
        Rectangle hitBox;

        // constructor
        public GameObject()
        {
        }
    }
}
