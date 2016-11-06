using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class Boss : Enemy
    {
        // attributes
        List<BossLeapZone> leapList;
        List<GameObject> gObjs;
        Rectangle bottomSide;
        public BossLeapZone currentZone { get; set; }
        Player player1;
        double leapTimer;
        const int MAX_FALL_SPEED = -11;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, List<BossLeapZone> lList) : base(hBox, env, texture, hp)
        {
            leapList = lList;
            gObjs = env;

            speedX = 0;
            leapTimer = 0;
            bottomSide = new Rectangle(hitBox.X + 8, (hitBox.Y + hitBox.Height), hitBox.Width - 8, 1);
        }

        // methods
        // Move method finds the player,  has the boss move towards them,  calls the Pounce method when appropriate, .
        public override void Move()
        {
            // update the timer
            leapTimer++;


            // implement gravity for the boss
            foreach (GameObject platforms in gObjs)
            {
                if (platforms is Player)
                    player1 = (Player)platforms;
                /*
                if (player1.hitBox.X > hitBox.X && CheckLedges())
                    hitBox.X += speedX;
                else if (player1.hitBox.X < hitBox.X && CheckLedges())
                    hitBox.X -= speedX;
                */

                hitBox.X += speedX;

                if (platforms.DetectCollision(this))
                {
                    // if the boss is on a platform, stop it from falling
                    speedY = 0;

                    BossResetY();
                }
                else
                {
                    // have the boss fall if not on a platform
                    hitBox.Y -= speedY;
                    speedY--;
                }
            }

            // maximum falling speed. Probably won't be used but you never know.
            if (speedY < MAX_FALL_SPEED)
            {
                speedY = MAX_FALL_SPEED;
            }

            if (leapTimer >= 30)
                Pounce();
        }

        // Pounce method. Gets a list of BossLeapZones connected to the one it's on, 
        private void Pounce()
        {
            foreach(BossLeapZone zone in leapList)
            {
                if (DetectCollision(zone))
                {
                    currentZone = zone;
                    break;
                }
            }

            // create a list of the ID's of the zones the boss can jump to
            List<int> jumpZonesID = currentZone.linkedZones;
            List<BossLeapZone> jumpZones = new List<BossLeapZone>();

            // populate the jumpZones list with the BossLeapZones associated with the ID's in the JumpZonesID list
            foreach (int zone in jumpZonesID)
            {
                foreach (BossLeapZone lzone in leapList)
                {
                    if (zone == lzone.id)
                        jumpZones.Add(lzone);
                }
            }

            // check which jump zone the boss can jump to that the player is in
            foreach (BossLeapZone zone in jumpZones)
            {
                if (zone.DetectCollision(player1))
                {

                    int x0 = hitBox.X;  //start x
                    int y0 = hitBox.Y;  //start y
                    int x1 = zone.hitBox.X;  //target x
                    int y1 = zone.hitBox.Y;  //target y

                    //constant x speed in direction of zone
                    int dx = 1;
                    if (x1 - x0 > 0)
                        speedX = dx;
                    else if (x1 - x0 < 0)
                        speedX = -dx;
                    else if (x1 - x0 == 0)
                        speedX = 0;


                    /*
                    // get the boss to leap to it

                    //so here's how I want to do this: The Boss compares its position to the position of the LeapZone it's targeting
                    //If the difference in x is negative or positive will affect the speed. The Y comparison (less, equal, greater) will then inform which jump it will perform
                    
                    int xCompare = zone.hitBox.X - hitBox.X;    // will be negative if the boss is to the right of the leap zone
                    int yCompare = zone.hitBox.Y - hitBox.Y;    // will be negative if the boss is below the leap zone


                    if (yCompare < 0)
                    {
                        if (!DetectCollision(zone))  // keeps the boss moving until it lands in the zone it's targeted
                        {
                            if (xCompare < 0)
                                hitBox.X -= speedX;     // reverses the speed of the boss to make it move left if it's to the right of the leap zone
                            else
                                hitBox.X += speedX;

                            if (xCompare - hitBox.X <= 100)
                            {
                                // insert code for jumping to a platform above the boss
                                speedY = 18;
                                hitBox.Y -= speedY;
                                speedY--;

                                // reset the leap timer once pounce work has been done
                                leapTimer = 0;
                            }
                        }
                    }
                    if (yCompare == 0)
                    {
                        if (!DetectCollision(zone))
                        {
                            if (xCompare < 0)
                                hitBox.X -= speedX;
                            else
                                hitBox.X += speedX;

                            if (xCompare - hitBox.X <= 100)
                            {
                                // insert code for jumping to a platform on the same level as the boss
                                speedY = 18;
                                hitBox.Y -= speedY;
                                speedY--;

                                leapTimer = 0;
                            }
                        }
                    }
                    if(yCompare > 0)
                    {
                        if (!DetectCollision(zone))
                        {
                            if (xCompare < 0)
                                hitBox.X -= speedX;
                            else
                                hitBox.X += speedX;

                            if (xCompare - hitBox.X <= 100)
                            {
                                // insert code for jumping to a platform below the boss
                                speedY = 18;
                                hitBox.Y -= speedY;
                                speedY--;

                                leapTimer = 0;
                            }
                        }
                    }

                    */

                    // set the current leap zone to the zone the Boss is in
                    currentZone = zone;
                    break;
                }
                // if the boss does not detect the player in any leap zones it can move to, do nothing until next pounce
            }
        }

        // BossResetY does basically the same thing as the PlayerResetY
        public void BossResetY()
        {
            foreach(GameObject platform in gObjs)
            {
                if(platform.hitBox.Intersects(bottomSide))
                {
                    hitBox.Y = platform.hitBox.Y - hitBox.Height;
                }
            }
        }

        // other Move method. Not implemented here
        public override void Move(List<GameObject> gObjs)
        {
            throw new NotImplementedException();
        }
    }
}
