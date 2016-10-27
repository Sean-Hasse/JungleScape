using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class Boss : Character
    {
        public Boss(Rectangle hBox, Texture2D texture, int hp) : base(hBox, texture, hp)
        {
            speedX = 4;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override void Move(List<GameObject> gObj)
        {
            foreach(Player p in gObj.OfType<Player>())
            {
                if (p.hitBox.X > hitBox.X)
                    hitBox.X += speedX;
                else if (p.hitBox.X < hitBox.X)
                    hitBox.X -= speedX;
            }
            
            
        }
    }
}
