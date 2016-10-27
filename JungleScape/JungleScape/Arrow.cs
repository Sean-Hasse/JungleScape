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
    public class Arrow : Character
    {
        // attributes
        public string direction;
        double timerDmg;

        // properties
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        // constructor
        public Arrow(int spdX, int spdY, Rectangle hBox, Texture2D texture, int hp, double timer) : base(hBox, texture, hp)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            speedX = spdX;
            speedY = spdY;
            timerDmg = timer;
        }

        // methods
        // move method. 
        public override void Move(List<GameObject> gObj)
        {
            // move the arrow according to the speed.
            hitBox.X += speedX;
            hitBox.Y += speedY;

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
            }
        }

        // Draw method, for drawing the arrow after it is fired
        public void Draw(SpriteBatch sb)
        {
            // Draw the arrow rotated differently for which way the player aims
            if (direction == "diagonal right")
                sb.Draw(sprite, hitBox, null, Color.White, (float)Math.PI * 7/4, Vector2.Zero, SpriteEffects.None, 0f);
            if(direction == "diagonal left")
                sb.Draw(sprite, hitBox, null, Color.White, (float)Math.PI * 5/4, Vector2.Zero, SpriteEffects.None, 0f);
            if(direction == "up")
                sb.Draw(sprite, hitBox, null, Color.White, (float)Math.PI * 3/2, Vector2.Zero, SpriteEffects.None, 0f);
            if (direction == "right")
                sb.Draw(sprite, hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            if(direction == "left")
                sb.Draw(sprite, hitBox, null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0f);
        }

        // Standard Move(). Not used.
        public override void Move()
        {
        }
        
    }
}
