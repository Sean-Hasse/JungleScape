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
        KeyboardState keyState;

        // constructor
        public Player(Rectangle hBox) : base(hBox)
        {
            keyState = new KeyboardState();
            speedX = 5;
            speedY = 5;
        }

        // methods
        public override void Move()
        {
            keyState = Keyboard.GetState();

            // use IsKeyDown to determine if a partuclar key is being pressed. Use 4 if statesments for wasd
            if (keyState.IsKeyDown(Keys.W))
            {
                hitBox.Y -= speedY; // temp jump button
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                hitBox.X -= speedX;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                hitBox.Y += speedY; // temp Drop Down button
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                hitBox.X += speedX;
            }
        }

        // Aim will determine which direction the player in inputting to aim in
        public string Aim()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
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
        public void FireArrow()     // currently this method will just create the arrow and set its speed, still need to make it move and make sure it's detecting for collisions and killing things
        {
            Arrow arrow;
            string direction = Aim();

            if(direction == "up")
            {
                arrow = new Arrow(0, 20, new Rectangle((hitBox.X), (hitBox.Y), 30, 5), this);
            }
            if(direction == "right")
            {
                arrow = new Arrow(20, 0, new Rectangle((hitBox.X), (hitBox.Y), 30, 5), this);
            }
            if (direction == "left") ;
            {
                arrow = new Arrow(-20, 0, new Rectangle((hitBox.X), (hitBox.Y), 30, 5), this);
            }
            if (direction == "diagonal right")
            {
                arrow = new Arrow(5, 5, new Rectangle((hitBox.X), (hitBox.Y), 30, 5), this);
            }
            if (direction == "diagonal left")
            {
                arrow = new Arrow(-5, 5, new Rectangle((hitBox.X), (hitBox.Y), 30, 5), this);
            }
        }
    }
}
