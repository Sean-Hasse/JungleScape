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
        const int MAX_FALL_SPEED = -13;
        double timerArrow = 0;
        double timerJump = 0;
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
            speedX = 8;
            speedY = 0;
        }

        // methods
        public override void Move(List<GameObject> gameObjs)
        {
            // set rectangles for collsion detection
            leftSide = new Rectangle(hitBox.X, hitBox.Y, 1, hitBox.Height);
            rightSide = new Rectangle((hitBox.X + hitBox.Width), hitBox.Y, 1, hitBox.Height);
            topSide = new Rectangle(hitBox.X, hitBox.Y, hitBox.Width - 25, 1);
            bottomSide = new Rectangle(hitBox.X, (hitBox.Y + hitBox.Height), hitBox.Width - 25, 1);
            List<GameObject> platforms = new List<GameObject>();
            List<GameObject> enemies = new List<GameObject>();

            // populate the platforms and enemies lists
            foreach (GameObject gObj in gameObjs)
            {
                if (gObj is Environment)
                    platforms.Add(gObj);
                if (gObj is Enemy)
                    enemies.Add(gObj);
            }

            keyState = Keyboard.GetState();
            timerJump++;

            // use IsKeyDown to determine if a partuclar key is being pressed. Use 4 if statesments for wasd
            // if the top of the player isn't intersecting any platforms, and the bottom of the player is intersecting the platform, run jump logic
            if (PlayerDetectCollision(bottomSide, platforms))
            {
                // first, set speedy to 0, player should no be moving in y direction when on a platform with no key press. Also set y to level with the ground
                speedY = 0;
                PlayerResetY(bottomSide, platforms, "bottom");

                // check if the player is colliding with a platform above them
                if (!PlayerDetectCollision(topSide, platforms))
                {
                    // Allow jump if these conditions are met.
                    if (keyState.IsKeyDown(Keys.W) && timerJump >= 20)
                    {
                        // move the player up and start gravity
                        speedY = 15;
                        hitBox.Y -= speedY;
                        speedY--;

                        // reset jump timer
                        timerJump = 0;
                    }
                }
            }
            else
            {
                hitBox.Y -= speedY;
                speedY--;
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
            
            // If player hits their head on a wall, make them stop moving up.
            if(PlayerDetectCollision(topSide, platforms) && speedY >= 0)
            {
                speedY = 0;
                PlayerResetY(topSide, platforms, "top");
            }

            // Maximum falling speed. If exceeded, resets the falling speed to the maximum. Ensures not continuous accelleration
            if (speedY < MAX_FALL_SPEED)
            {
                speedY = MAX_FALL_SPEED;
            }

            // Check for enemy Collision. If true, take damage.
            foreach(GameObject enemy in enemies)
            {
                if(DetectCollision(enemy))
                {
                    TakeDamage(this);
                }
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

        // FireArrow method will create an arrow with speed based on the direction passed in by Aim. Requires the image for the arrow be passed in.
        public void FireArrow(Texture2D arrowImage, List<GameObject> objects)
        {
            // get what direction the player is aiming in
            string direction = Aim();

            // increase the timer each update
            timerArrow++;

            // check to see if the player has pressed spacebar to fire
            if (keyState.IsKeyDown(Keys.Space) && timerArrow >= 60)
            {
                // reset the timer
                timerArrow = 0;

                if (direction == "up")
                {
                    // creates an arrow, 0 horizontal speed, 8 verticle, starts in player center with dimesnions 20x5, and uses the passed in image
                    Arrow arrow = new Arrow(0, -8, new Rectangle(hitBox.Center, new Point(20, 5)), arrowImage);

                    // make the arrow move 
                    arrow.Move(objects);

                    // reset timer
                    timerArrow = 0;
                }
                if (direction == "right")
                {
                    Arrow arrow = new Arrow(12, 0, new Rectangle(hitBox.Center, new Point(20, 5)), arrowImage);
                    arrow.Move(objects);
                    timerArrow = 0;
                }
                if (direction == "left")
                {
                    Arrow arrow = new Arrow(-12, 0, new Rectangle(hitBox.Center, new Point(20, 5)), arrowImage);
                    arrow.Move(objects);
                    timerArrow = 0;
                }
                if (direction == "diagonal right")
                {
                    Arrow arrow = new Arrow(6, -6, new Rectangle(hitBox.Center, new Point(20, 5)), arrowImage);
                    arrow.Move(objects);
                    timerArrow = 0;
                }
                if (direction == "diagonal left")
                {
                    Arrow arrow = new Arrow(-6, -6, new Rectangle(hitBox.Center, new Point(20, 5)), arrowImage);
                    arrow.Move(objects);
                    timerArrow = 0;
                }
            }

            // code to stop rapid fire arrows here
            timerArrow++;
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
                }
                return false;
            }
            else
                return false;
        }

        // method for setting the Y position = to the platform being hit. Stops character from sinking.
        private void PlayerResetY(Rectangle side, List<GameObject> platforms, string stringName)
        {
            if (platforms.Count != 0)
            {
                foreach (GameObject platform in platforms)
                {
                    if (stringName == "bottom")
                    {
                        if (side.Intersects(platform.hitBox))
                        {
                            // set the Y value of the player hitbox equal to the top of the platform
                            hitBox.Y = platform.hitBox.Y - hitBox.Height;
                        }
                    }
                    if(stringName == "top")
                    {
                        if (side.Intersects(platform.hitBox))
                            // set the Y value of the player hitbox equal to the bottom of the platform
                            hitBox.Y = platform.hitBox.Y + hitBox.Height;
                    }
                }
                
            }
        }

        // Original Move. Not being used.
        public override void Move()
        {
        }
    }
}

