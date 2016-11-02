using PlatformerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class LeapZoneTile : Tile
    {
        /// <summary>
        /// id reference for this tile
        /// </summary>
        public int id { get; }

        /// <summary>
        /// list of zone ids this zone is linked to
        /// </summary>
        public List<int> linkedZones { get; }

        public LeapZoneTile(Rectangle bounds, Texture2D texture, ObjectType type, List<int> linkedZones, int id) : base(bounds, texture, type)
        {
            this.id = id;
            this.linkedZones = linkedZones;
        }
    }
}
