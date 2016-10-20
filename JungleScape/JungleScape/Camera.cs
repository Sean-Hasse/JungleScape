using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleScape
{
    class Camera
    {
        public Vector2 position { get; set; }

        public Camera(Player player)
        {
            position = new Vector2(Game1.desiredBBWidth / 2 - player.hitBox.X, Game1.desiredBBHeight / 2 - player.hitBox.Y);
        }
    }
}
