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
        public Arrow(int spdX, int spdY, Rectangle hBox, Player player1, Texture2D texture) : base(hBox, texture)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            direction = player1.Aim();
            hitBox = new Rectangle(player1.hitBox.Center, new Point(20, 5));
        }

        // methods
        // move method. 
        public override void Move()
        {
            if (direction == "up")
            {
                speedX = 0;
                speedY = 8;
            }
            if (direction == "right")
            {
                speedX = 8;
                speedY = 0;
            }
            if (direction == "left")
            {
                speedX = -8;
                speedY = 0;
            }
            if (direction == "diagonal right")
            {
                speedX = 4;
                speedY = 4;
            }
            if (direction == "diagonal left")
            {
                speedX = 4;
                speedY = 4;
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

        // move meant for Player class. Not used.
        public override void Move(List<GameObject> gObj)
        {
        }
    }
}
