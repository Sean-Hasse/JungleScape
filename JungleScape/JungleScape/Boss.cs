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
        BossLeapZone currentZone;
        Player player1;
        double leapTimer;
        const int MAX_FALL_SPEED = -11;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, BossLeapZone startZone, List<BossLeapZone> lList, Player p) : base(hBox, env, texture, hp)
        {
            currentZone = startZone;
            leapList = lList;
            gObjs = env;

            speedX = 4;
            leapTimer = 0;
            player1 = p;
        }

        // methods

        // Move method takes the list of Game Objects and finds the player, and has the boss move towards them.
        public override void Move()
        {
            
            if (player1.hitBox.X > hitBox.X)
                hitBox.X += speedX;
            else if (player1.hitBox.X < hitBox.X)
                hitBox.X -= speedX;

            // update the timer
            leapTimer++;

            if (leapTimer >= 30)
                Pounce();

            // implement gravity for the boss
            foreach (GameObject platforms in gObjs)
            {
                if(platforms.DetectCollision(this))
                {
                    // if the boss is on a platform, stop it from falling
                    speedY = 0;

                    // call a "ResetBossY" method
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
        }

        // method for boss to move from platoform to platform towards the Player. 
        private void Pounce()
        {
            int listID = currentZone.id;

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
                    // get the boss to leap to it

                    /* so here's how I want to do this: The Boss compares its position to the position of the LeapZone it's targeting
                     * If the difference in x is negative or positive will affect the speed. The Y comparison (less, equal, greater) will then inform which jump it will perform*/
                    int xCompare = zone.hitBox.X - hitBox.X;    // will be negative if the boss is to the right of the leap zone
                    int yCompare = zone.hitBox.Y - hitBox.Y;    // will be negative if the boss is below the leap zone


                    if (yCompare < 0)
                    {
                        while (!DetectCollision(zone))  // keeps the boss moving until it lands in the zone it's targeted
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
                            }
                        }
                    }
                    if (yCompare == 0)
                    {
                        while (!DetectCollision(zone))
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
                            }
                        }
                    }
                    if(yCompare > 0)
                    {
                        while (!DetectCollision(zone))
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
                            }
                        }
                    }
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
