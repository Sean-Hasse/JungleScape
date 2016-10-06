using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JungleScape
{
    class Arrow : GameObject
    {
        // attributes
        int speedX;
        int speedY;

        // constructor
        public Arrow(int spdX, int spdY, Rectangle hBox, Player player1) : base(hBox)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            string direction = player1.Aim();
            hitBox = new Rectangle();
        }

        // methods
        public void ArrowHit(GameObject collisionObj)
        {
            if(DetectCollision(collisionObj))
            {
                if()
            }
        }
    }
}
