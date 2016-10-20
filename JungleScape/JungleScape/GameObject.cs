using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public abstract class GameObject
    {
        // attributes
        public Rectangle hitBox;       // holds the rectangle of the hitbox
        public Texture2D sprite;

        // constructor
        public GameObject(Rectangle hBox, Texture2D texture)
        {
            sprite = texture;
            hitBox = hBox;
        }

        // method
        public bool DetectCollision(GameObject go)      // detects collision between this object and the object passed in.
        {
            if (this.hitBox.Intersects(go.hitBox))
            {
                return true;
            }
            else
                return false;
        }
    }
}
