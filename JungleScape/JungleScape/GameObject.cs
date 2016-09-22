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
        public Point location;         // holds the location of the object
        public Rectangle hitBox;       // holds the rectangle of the hitbox

        // constructor
        public GameObject(Point loc, Rectangle hBox)
        {
            location = loc;
            hitBox = hBox;
        }

        // method
        public bool DetectCollision(GameObject go)      // detects collision between this object and the object passed in.
        {
            if (hitBox.Intersects(go.hitBox))
            {
                return true;
            }
            else
                return false;
        }
    }
}
