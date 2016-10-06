using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JungleScape
{
    class Arrow : Character
    {
        // attributes

        // constructor
        public Arrow(int spdX, int spdY, Rectangle hBox, Player player1) : base(hBox)
        {

            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();
            string direction = player1.Aim();
            hitBox = new Rectangle();
        }

        // methods

        public override void Move()
        {
            ;
        }

        public void ArrowHit(GameObject collisionObj)
        {
            
        }
    }
}
