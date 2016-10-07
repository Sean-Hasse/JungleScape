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
    public class Player : Character
    {
        // attributes
        const int MAX_FALL_SPEED = -10;
        Rectangle leftSide;
        Rectangle rightSide;
        Rectangle topSide;
        Rectangle bottomSide;
        KeyboardState keyState;
        enum AimDirection { Forward, Diagonal, Up };
        AimDirection aimDirection;

        // constructor
        public Player(Rectangle hBox, Texture2D texture) : base(hBox, texture)
        {
            keyState = new KeyboardState();
            aimDirection = AimDirection.Forward;
            speedX = 5;
            speedY = -5;

            // set rectangles for collsion detection
            leftSide = new Rectangle(hitBox.Left, hitBox.Top, 1, hitBox.Height);
            rightSide = new Rectangle(hitBox.Right, hitBox.Top, -1, hitBox.Height);
            topSide = new Rectangle(hitBox.Left, hitBox.Top, hitBox.Width, 1);
            bottomSide = new Rectangle(hitBox.Left, hitBox.Bottom, hitBox.Width, -1);
        }

        // methods
        public override void Move(List<GameObject> platforms)
        {
            keyState = Keyboard.GetState();

            // use IsKeyDown to determine if a partuclar key is being pressed. Use 4 if statesments for wasd
            // if the top of the player isn't intersecting any platforms, and the bottom of the player is intersecting the platform, run jump logic
            if (!PlayerDetectCollision(topSide, platforms) && PlayerDetectCollision(bottomSide, platforms))
            {
                if (keyState.IsKeyDown(Keys.W))
                {
                    speedY = 10;
                    hitBox.Y -= speedY;
                    speedY--;
                }
            }

            // if the left side of the player does not instersect any platforms, move the player to the left.
            if (!PlayerDetectCollision(leftSide, platforms))
            {
                if (keyState.IsKeyDown(Keys.A))
                {
                    hitBox.X -= speedX;
                }
            }

            // if the right side of the player does not interesect any platforms, move the player to the right.
            if (!PlayerDetectCollision(rightSide, platforms))
            {
                if (keyState.IsKeyDown(Keys.D))
                {
                    hitBox.X += speedX;
                }
            }


            // fake physics
            // if the bottom side of the player does not intersect with any platforms, make them fall.
            if(!PlayerDetectCollision(bottomSide, platforms))
            {
                hitBox.Y -= speedY;
                speedY--;
            }

            if (speedY < MAX_FALL_SPEED)
            {
                speedY = MAX_FALL_SPEED;
            }
        }

        // Aim will determine which direction the player in inputting to aim in
        public string Aim()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
            {
                aimDirection = AimDirection.Up;
                return "up";
            }      
            if (keyState.IsKeyDown(Keys.Right))
            {
                aimDirection = AimDirection.Forward;
                return "right";
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                aimDirection = AimDirection.Forward;
                return "left";
            }
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Right))
            {
                aimDirection = AimDirection.Diagonal;
                return "diagonal right";
            }
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Left))
            {
                aimDirection = AimDirection.Diagonal;
                return "diagonal left";
            }
            else
                return null;
        }

        // specialized detect collision for each side of the player.
        private bool PlayerDetectCollision(Rectangle side, List<GameObject> platforms)
        {
            if (platforms.Count != 0)
            {
                foreach (GameObject platform in platforms)
                {
                    if (side.Intersects(platform.hitBox))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            else
                return false;
        }

        // Original Move. Not being used.
        public override void Move()
        {
        }
    }
}

