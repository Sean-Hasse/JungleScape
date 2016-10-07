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
            if (keyState.IsKeyDown(Keys.W))
            {
                speedY = 10;
            }
            if (PlayerDetectCollision(leftSide, platforms))
            {
                if (keyState.IsKeyDown(Keys.A))
                {
                    hitBox.X -= speedX;
                }
            }

                
            if (keyState.IsKeyDown(Keys.D))
            {
                hitBox.X += speedX;
            }

            // fake physics
            //if(PlayerDetectCollision(bottomSide, ))
            hitBox.Y -= speedY;
            speedY--;

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
            throw new NotImplementedException();
        }
    }
}

