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
        BossLeapZone currentZone;

        public Boss(Rectangle hBox, List<GameObject> env, Texture2D texture, int hp, BossLeapZone startZone) : base(hBox, env, texture, hp)
        {
            currentZone = startZone;
            gameObjs = env;
            speedX = 4;
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
        }

        // other Move method. Not implemented here
        public override void Move(List<GameObject> gObjs)
        {
            throw new NotImplementedException();
        }
    }
}
