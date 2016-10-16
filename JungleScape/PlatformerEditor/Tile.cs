using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    class Tile
    {
        public Rectangle bounds { get; set; }
        public Texture2D texture { get; set; }

        public Tile(Rectangle bounds, Texture2D texture)
        {
            this.bounds = bounds;
            this.texture = texture;
        }
    }
}
