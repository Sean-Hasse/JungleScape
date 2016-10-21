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
        string direction;

        // constructor
        public Arrow(int spdX, int spdY, Rectangle hBox, Texture2D texture) : base(hBox, texture)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
        }

        // methods
        // move method. 
        public override void Move(List<GameObject> gObj)
        {
            // move the arrow according to the speed.
            hitBox.X += speedX;
            hitBox.Y += speedY;

            // check for collisions with the game objects, and execute logic based on what type of object is hit.
            foreach(GameObject gObject in gObj)
            {
                if(DetectCollision(gObject))
                {
                    if(gObject is Enemy)
                    {
                        speedX = 0;
                        speedY = 0;
                        DealDamage((Enemy)gObject);

                        alive = false;
                    }

                    if(gObject is Environment)
                    {
                        speedX = 0;
                        speedY = 0;

                        alive = false;
                    }
                }
            }
        }

        // checks if there is a collision. Then checks if the collision is an enemy, dealing damage to them.
        public void ArrowHit(GameObject collisionObj)
        {
            if(DetectCollision(collisionObj))
            {
                if(collisionObj is Enemy)
                {
                    DealDamage((Enemy)collisionObj);
                }
            }
        }

        // Standard Move(). Not used.
        public override void Move()
        {
        }
        
    }
}
