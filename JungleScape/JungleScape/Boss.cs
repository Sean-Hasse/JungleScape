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
        bool isPouncing;
        const int MAX_FALL_SPEED = 11;
        bool hasLanded;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, List<BossLeapZone> lList) : base(hBox, env, texture, hp)
        {
            leapList = lList;
            gObjs = env;

            speedX = 0;
            speedY = 0;
            leapTimer = 0;
            isPouncing = false;
            hasLanded = true;
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

            // update the timer
            leapTimer++;

            //update x values
            hitBox.X += speedX;
            
            //update y values
            hitBox.Y -= speedY;
            if (isPouncing)
                speedY--;

            bool ledgeCheck = BossCheckLedges();

            // move properly along edges
            if (!isPouncing && ledgeCheck && speedX > 0)
                speedX = 4;
            else if (!isPouncing && ledgeCheck && speedX <= 0)
                speedX = -4;
            else if (!isPouncing && !ledgeCheck && hasLanded)
                speedX = -speedX;
            else if(!isPouncing && !ledgeCheck && !hasLanded)
            {
                if (speedX > 0)
                    speedX = 4;
                else
                    speedX = -4;

                if (ledgeCheck)
                    hasLanded = true;
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


            if (speedY >= MAX_FALL_SPEED)
                speedY = MAX_FALL_SPEED;

            if (leapTimer >= 60 && !isPouncing)
            {
                Pounce();
                leapTimer = 0;
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
                    isPouncing = true;
                    hasLanded = false;

                    int xPos = hitBox.X;                //start x
                    int yPos = hitBox.Bottom;           //start y
                    int xZone = zone.hitBox.X;          //target x
                    int yZone = zone.hitBox.Bottom;     //target y
                    
                    int xDistance = xZone - xPos;    // will be negative if the boss is to the right of the leap zone
                    int yDistance = yZone - yPos;    // will be negative if the boss is below the leap zone

                    // kinematic equation stuff
                    int pounceSpeedX;

                    if (xDistance < 0)
                        pounceSpeedX = -10;
                    else if (xDistance > 0)
                        pounceSpeedX = 10;
                    else
                        pounceSpeedX = 0;

                    //float time = (2*(xDistance)/pounceSpeedX);
                    //pounceSpeedY = ((yDistance - ((time * time)/2))/ time);

                    if (yDistance <= 0)
                    {
                        //speedY = (int)pounceSpeedY;
                        speedY = (int)(3 * Math.Sqrt(((xDistance * xDistance) / (pounceSpeedX * pounceSpeedX)) - (2 * yDistance)));
                    }
                    else if (yDistance > 0)
                    {

                    }

                    speedX = pounceSpeedX;
                    break;
                }
            }
        }

        public bool BossCheckLedges()
        {
            // how I want to do this: make 2 new Rectangles on the Left and Right edge of the enemy 1 pixel taller than it, and when one of them isn't colliding, reverse the speed
            Rectangle leftRect = new Rectangle(hitBox.X + 1, hitBox.Y, 2, hitBox.Height + 5);       // creates a 1 pixel wide rectangle in the top left, and extends 1 pixel past the bottom of the enemy
            Rectangle rightRect = new Rectangle(hitBox.X + hitBox.Width - 1, hitBox.Y, 2, hitBox.Height + 5);       // creates a the same type of rectange in the top right

            List<Environment> ledges = new List<Environment>();
            foreach (GameObject obj in gObjs.OfType<Environment>())
            {
                if (obj is Environment)
                    ledges.Add((Environment)obj);
            }

            // check all platforms for intersections on either side individually. Then, if there are a total of 2 intersections (1 left, 1 right), return true. Else, false.
            int onLedge = 0;

            foreach (Environment ledge in ledges)
            {
                if (rightRect.Intersects(ledge.hitBox))
                    onLedge++;
                if (leftRect.Intersects(ledge.hitBox))
                    onLedge++;
            }

            // return true if both sides are on the ledge
            if (onLedge == 2)
                return true;
            else
                return false;
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
