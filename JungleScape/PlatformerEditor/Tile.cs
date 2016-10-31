using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    enum ObjectType { Player, TopBrick, PlainBrick, Enemy, Delete, BossLeapZone }
    class Tile
    {
        public Rectangle bounds { get; set; }

        [JsonIgnore]
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
