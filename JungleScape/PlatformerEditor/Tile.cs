using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    enum ObjectType { Player, TopBrick, PlainBrick, Enemy }
    class Tile
    {
        public Rectangle bounds { get; set; }
        public Texture2D texture { get; set; }
        public ObjectType type { get; set; }

        public Tile(Rectangle bounds, Texture2D texture, ObjectType type)
        {
            this.bounds = bounds;
            this.texture = texture;
            this.type = type;
        }
    }
}
