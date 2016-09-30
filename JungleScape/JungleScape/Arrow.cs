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
        public Arrow(int spdX, int spdY, Rectangle hBox) : base(hBox)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            string direction = player1.Aim(keyState);
            hitBox = new Rectangle();
        }
    }
}
