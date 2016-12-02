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
        Rectangle leftSide;
        Rectangle rightSide;
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
            leftSide = new Rectangle(hitBox.X, hitBox.Y, 1, hitBox.Height - 3);
            rightSide = new Rectangle((hitBox.X + hitBox.Width), hitBox.Y, 1, hitBox.Height - 3);

            // update the timer
            leapTimer++;

            //update x values
            hitBox.X += speedX;
            
            //update y values
            hitBox.Y -= speedY;
            if (isPouncing)
                speedY--;

            // move properly along edges
            if (!isPouncing && CheckLedges() && speedX > 0)
                speedX = 4;
            else if (!isPouncing && CheckLedges() && speedX <= 0)
                speedX = -4;
            else if (!isPouncing && !CheckLedges() && hasLanded)
                speedX = -speedX;
            else if(!isPouncing && !CheckLedges() && !hasLanded)
            {
                if (speedX > 0)
                    speedX = 4;
                else
                    speedX = -4;

                if (CheckLedges())
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

                    int time = 60;
                    double angle = Math.PI / 4;
                    int accel = 2;

                    // kinematic equation stuff
                    double pounceSpeedX = xDistance / (time * Math.Cos(angle));

                    //float time = (2*(xDistance)/pounceSpeedX);
                    //pounceSpeedY = ((yDistance - ((time * time)/2))/ time);

                    if (yDistance <= 0)
                    {
                        //speedY = (int)pounceSpeedY;
                        //speedY = (int)(3 * Math.Sqrt(((xDistance * xDistance) / (pounceSpeedX * pounceSpeedX)) - (2 * yDistance)));
                        speedY = (int)((yDistance + (0.5 * time * time * accel)) / (time * Math.Sin(angle)));
                    }

                    speedX = (int)pounceSpeedX;
                    break;
                }
            }
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
