using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleScape
{
    public class Camera
    {
        public Vector2 position { get; set; }

        public Camera(Player player)
        {
            position = new Vector2(Game1.desiredBBWidth / 2 - player.hitBox.X, Game1.desiredBBHeight / 2 - player.hitBox.Y);
        }

        public Matrix translation(Player player)
        {
            position = new Vector2(player.hitBox.X - Game1.desiredBBWidth / 2, player.hitBox.Y - Game1.desiredBBHeight / 2);

            return Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
