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
            hitBox = new Rectangle();
        }

        // methods
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

        public void ArrowHit(GameObject collisionObj)
        {
            
        }
    }
}
