using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class Enemy : Character
    {
        GameObject linkedBlock;

        // constructor
        public Enemy(Rectangle hBox, GameObject env, Texture2D texture) : base(hBox, texture)
        {
            linkedBlock = env;
            speedX = -2;
            speedY = 0;
        }

        // methods
        // checkLedges turns the enemy around when they approach a ledge
        public bool CheckLedges(GameObject ledge)
        {
            // how I want to do this: make 2 new Rectangles on the Left and Right edge of the enemy 1 pixel taller than it, and when one of them isn't colliding, reverse the speed
            Rectangle leftRect = new Rectangle(hitBox.X, hitBox.Y, 1, hitBox.Height + 1);       // creates a 1 pixel wide rectangle in the top left, and extends 1 pixel past the bottom of the enemy
            Rectangle rightRect = new Rectangle(hitBox.X + hitBox.Width, hitBox.Y, 1, hitBox.Height + 1);       // creates a the same type of rectange in the top right

            if (!leftRect.Intersects(ledge.hitBox) || !rightRect.Intersects(ledge.hitBox))
            {
                return true;
            }
            else
                return false;
        }

        // calls  CheckLedges based on the ledge they were assigned to, and moves the enemy
        public override void Move()
        {
            if (CheckLedges(linkedBlock))
            {
                speedX = -speedX;
            }
            hitBox.X += speedX;
        }

        // Move meant for Player class. Not used.
        public override void Move(List<GameObject> gObj)
        {
            throw new NotImplementedException();
        }
    }
}
