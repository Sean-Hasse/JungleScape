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
        List<GameObject> gameObjs;
        List<BossLeapZone> leapList;
        BossLeapZone currentZone;
        double leapTimer;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, BossLeapZone startZone, List<BossLeapZone> lList) : base(hBox, env, texture, hp)
        {
            currentZone = startZone;
            leapList = lList;
            gameObjs = env;
            speedX = 4;
            leapTimer = 0;
        }

        // methods

        // Move method takes the list of Game Objects and finds the player, and has the boss move towards them.
        public override void Move()
        {
            foreach(Player p in gameObjs.OfType<Player>())
            {
                if (p.hitBox.X > hitBox.X)
                    hitBox.X += speedX;
                else if (p.hitBox.X < hitBox.X)
                    hitBox.X -= speedX;
            }

            // update the timer
            leapTimer++;

            if (leapTimer >= 30)
                Pounce();
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
            foreach (Player p in gameObjs)
            {
                foreach (BossLeapZone zone in jumpZones)
                {
                    if (zone.DetectCollision(p))
                        return;
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
