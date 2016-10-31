using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleScape
{
    public enum ObjectType { Player, TopBrick, PlainBrick, Enemy, BossLeapZone }
    class Tile
    {
        public Rectangle bounds { get; set; }
        public ObjectType type { get; set; }
    }
}
