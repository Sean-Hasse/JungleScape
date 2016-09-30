using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JungleScape
{
    public class Player : Character
    {
        // attributes

        // constructor
        public Player(Rectangle hBox) : base(hBox)
        {

        }

        // methods
        public void PlayerMove()
        {
            KeyboardState keyState = new KeyboardState();
            keyState = Keyboard.GetState();

            // use IsKeyDown to determine if a partuclar key is being pressed. Use 4 if statesments for wasd
            if (keyState.IsKeyDown(Keys.W))
            {
                Move(0,1);      // temp jump button
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                Move(-1,0);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                Move(0,0);      // temp Drop Down button
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                Move(1,0);
            }
        }

        // Aim will determine which direction the player in inputting to aim in
        public string Aim(KeyboardState keyState)
        {
            if(keyState.IsKeyDown(Keys.Up))
                return "up";
            if (keyState.IsKeyDown(Keys.Right))
                return "right";
            if (keyState.IsKeyDown(Keys.Left))
                return "left";
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Right))
                return "diagonal right";
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Left))
                return "diagonal left";
            else
                return "";
        }

        // FireArrow will fire an arrow
        public void FireArrow()
        {
            
        }
    }
}
