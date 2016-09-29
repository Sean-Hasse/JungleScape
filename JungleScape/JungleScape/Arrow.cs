using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JungleScape
{
    class Arrow 
    {
        // attributes
        int speed;
        Rectangle hitBox;

        // constructor
        public Arrow(int spd)
        {
            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            Player player1;
            string direction = player1.Aim(keyState);
            hitBox = new Rectangle()
        }
}
