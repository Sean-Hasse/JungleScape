using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    class Arrow : Character
    {
        // attributes

        // constructor
        public Arrow(int spdX, int spdY, Rectangle hBox, Texture2D texture) : base(hBox, texture)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            speedX = spdX;
            speedY = spdY;
        }

        // methods
        // move method. 
        public override void Move(List<GameObject> gObj)
        {
            // move the arrow according to the speed.
            hitBox.X += speedX;

            // check for collisions with the game objects, and execute logic based on what type of object is hit.
            foreach (GameObject gObject in gObj)
            {
                if (DetectCollision(gObject))
                {
                    if (gObject is Enemy)
                    {
                        speedX = 0;
                        speedY = 0;
                        TakeDamage((Enemy)gObject);

                        alive = false;
                    }

                    if (gObject is Environment)
                    {
                        speedX = 0;
                        speedY = 0;

                        alive = false;
                    }
                }
                else
                    speedY = 0;
            }
        }

        // Standard Move(). Not used.
        public override void Move()
        {
        }
        
    }
}
