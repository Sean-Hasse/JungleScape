using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JungleScape
{
    public class Enemy : Character
    {
        // attributes
        int speed;      // for measuring the speed the enemy will move. Gets passed into Character.Move()

        // constructor
        public Enemy(Point loc, Rectangle hBox, int spd) : base(loc, hBox)
        {
            speed = spd;
        }

        // methods
        public void checkLedges(GameObject ledge)
        {
            // how I want to do this: make 2 new Rectangles on the Left and Right edge of the enemy 1 pixel taller than it, and when one of them isn't colliding, reverse the speed
            Rectangle leftRect = new Rectangle(location.X, location.Y, 1, hitBox.Height + 1);       // creates a 1 pixel wide rectangle in the top left, and extends 1 pixel past the bottom of the enemy
            Rectangle rightRect = new Rectangle(location.X + hitBox.Width, location.Y, 1, hitBox.Height + 1);       // creates a the same type of rectange in the top right

            if(!leftRect.Intersects(ledge.hitBox) || !rightRect.Intersects(ledge.hitBox))
            {
                speed = -speed;
            }

        }
    }
}
