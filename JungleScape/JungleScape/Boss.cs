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
        Rectangle leftRect;
        Rectangle rightRect;
        public BossLeapZone currentZone { get; set; }
        Player player1;
        double leapTimer;
        bool isPouncing;
        bool needsAdjusted;
        const int MAX_FALL_SPEED = 11;
        int accel = 3;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, List<BossLeapZone> lList) : base(hBox, env, texture, hp)
        {
            leapList = lList;
            gObjs = env;

            speedX = 4;
            speedY = 0;
            leapTimer = 0;
            isPouncing = false;
            needsAdjusted = false;
        }

        // propterty
        public Player Player1
        {
            get { return player1; }
            set { player1 = value; }
        }

        // methods
        // Move method finds the player,  has the boss move towards them,  calls the Pounce method when appropriate, .
        public override void Move()
        {
            // create hitboxes for messing with movement
            bottomSide = new Rectangle(hitBox.X + 8, (hitBox.Y + hitBox.Height), hitBox.Width - 16, 1);
            // how I want to do this: make 2 new Rectangles on the Left and Right edge of the enemy 1 pixel taller than it, and when one of them isn't colliding, reverse the speed
            leftRect = new Rectangle(hitBox.X + 1, hitBox.Y, 2, hitBox.Height + 5);       // creates a 1 pixel wide rectangle in the top left, and extends 1 pixel past the bottom of the enemy
            rightRect = new Rectangle(hitBox.X + hitBox.Width - 1, hitBox.Y, 2, hitBox.Height + 5);       // creates a the same type of rectange in the top right

            // update the timer
            leapTimer++;

            //update x values
            hitBox.X += speedX;
            
            //update y values
            hitBox.Y -= speedY;
            if (isPouncing)
                speedY -= accel;
            else
            {
                if (CheckLedges())
                {
                    needsAdjusted = false;
                    if (speedX > 0)
                        speedX = 4;
                    else
                        speedX = -4;
                }

                if (needsAdjusted)
                {
                    List<bool> ledgeCheck = BossCheckLedges();

                    // move properly along edges
                    if (!ledgeCheck[0])
                        speedX = -4;
                    else if (!ledgeCheck[1])
                        speedX = 4;
                }
                else
                {
                    if (!CheckLedges())
                        speedX = -speedX;
                }
            }


            foreach (Environment platform in gObjs.OfType<Environment>())
            {
                if (!isPouncing) {
                    if (bottomSide.Intersects(platform.hitBox))
                    {
                        speedY = 0;
                        BossResetY(platform);
                    }
                }
            }


            //if (speedY <= MAX_FALL_SPEED)
                //speedY = MAX_FALL_SPEED;

            if (leapTimer >= 60 && !isPouncing)
            {
                Pounce();
                
            }
            foreach (BossLeapZone zone in gObjs.OfType<BossLeapZone>())
            {
                if (isPouncing && zone != currentZone && DetectCollision(zone))
                {
                    isPouncing = false;
                    break;
                }
            }
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
                    leapTimer = 0;
                    isPouncing = true;
                    needsAdjusted = true;

                    int xPos = hitBox.X;                //start x
                    int yPos = hitBox.Bottom;           //start y
                    int xZone = zone.hitBox.X;          //target x
                    int yZone = zone.hitBox.Bottom;     //target y
                    
                    int xDistance = xZone - xPos;    // will be negative if the boss is to the right of the leap zone
                    int yDistance = yZone - yPos;    // will be negative if the boss is below the leap zone

                    int time = 60;
                    double angle = Math.PI / 3;

                    // kinematic equation stuff
                    double pounceSpeedX = xDistance / (time * Math.Cos(angle));

                    //float time = (2*(xDistance)/pounceSpeedX);
                    //pounceSpeedY = ((yDistance - ((time * time)/2))/ time);

                    if (yDistance <= 175)
                    {
                        speedY = (int)((yDistance + (0.5 * time * time)) / (time * Math.Sin(angle)));
                    }

                    speedX = (int)pounceSpeedX;
                    break;
                }
            }
        }

        public List<bool> BossCheckLedges()
        {
            // check all platforms for intersections on either side individually and return true/false values for both sides
            List<bool> LRCheck = new List<bool>(2);
            LRCheck.Add(false);
            LRCheck.Add(false);

            foreach (Environment obj in gObjs.OfType<Environment>())
            {
                if (rightRect.Intersects(obj.hitBox))
                    LRCheck[0] = true;
                if (leftRect.Intersects(obj.hitBox))
                    LRCheck[1] = true;
            }

            return LRCheck;
        }

        // BossResetY does basically the same thing as the PlayerResetY
        public void BossResetY(GameObject platform)
        {
            if(platform.hitBox.Intersects(bottomSide))
            {
                hitBox.Y = platform.hitBox.Y - hitBox.Height;
            }
        }

        public void BossResetXLeft(GameObject platform)
        {
            if (platform.hitBox.Intersects(bottomSide))
            {
                hitBox.X = platform.hitBox.X + platform.hitBox.Width;
            }
        }

        public void BossResetXRight(GameObject platform)
        {
            if (platform.hitBox.Intersects(bottomSide))
            {
                hitBox.X = platform.hitBox.X - hitBox.Width;
            }
        }

        // other Move method. Not implemented here
        public override void Move(List<GameObject> gObjs)
        {
            throw new NotImplementedException();
        }
    }
}
