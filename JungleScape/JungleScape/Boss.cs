﻿using System;
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

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, List<BossLeapZone> lList) : base(hBox, env, texture, hp)
        {
            leapList = lList;
            gObjs = env;

            speedX = 0;
            speedY = 0;
            leapTimer = 0;
            bottomSide = new Rectangle(hitBox.X + 8, (hitBox.Y + hitBox.Height), hitBox.Width - 16, 1);
            leftSide = new Rectangle(hitBox.X, hitBox.Y, 1, hitBox.Height - 3);
            rightSide = new Rectangle((hitBox.X + hitBox.Width), hitBox.Y, 1, hitBox.Height - 3);
            isPouncing = false;
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
            // update the timer
            leapTimer++;

            //update x values
            hitBox.X += speedX;
            bottomSide.X += speedX;
            leftSide.X += speedX;
            rightSide.X += speedX;
            
            //update y values
            hitBox.Y -= speedY;
            bottomSide.Y -= speedY;
            leftSide.Y -= speedY;
            rightSide.Y -= speedY;
            speedY--;
            
            foreach(Environment platform in gObjs.OfType<Environment>())
            {
                if(bottomSide.Intersects(platform.hitBox) && !isPouncing)
                {
                    speedY = 0;
                    BossResetY(platform);
                }

                if(rightSide.Intersects(platform.hitBox) && !isPouncing)
                {
                    speedX = 0;
                    BossResetXRight(platform);
                }

                if(leftSide.Intersects(platform.hitBox) && !isPouncing)
                {
                    speedX = 0;
                    BossResetXLeft(platform);
                }
            }

            if(speedY != 0)
                isPouncing = true;  // doesn't get set to false yet

            if (speedY >= MAX_FALL_SPEED)
                speedY = MAX_FALL_SPEED;
            
            if (leapTimer >= 30 && !isPouncing)
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

                    int xPos = hitBox.X;  //start x
                    int yPos = hitBox.Bottom;  //start y
                    int xZone = zone.hitBox.X;  //target x
                    int yZone = zone.hitBox.Bottom;  //target y
                    
                    int xDistance = xZone - xPos;    // will be negative if the boss is to the right of the leap zone
                    int yDistance = yZone - yPos;    // will be negative if the boss is below the leap zone

                    // kinematic equation stuff
                    int pounceSpeedX = 10;
                    float time = (2*(xDistance)/pounceSpeedX);
                    float pounceSpeedY = ((yDistance - ((time * time)/2))/ time);

                    if (yDistance <= 0)
                    {
                        speedY = (int)pounceSpeedY;
                    }

                    speedX = -pounceSpeedX;
                }
                // if the boss does not detect the player in any leap zones it can move to, do nothing until next pounce
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
