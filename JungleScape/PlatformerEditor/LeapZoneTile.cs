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

        /// <summary>
        /// makes a new LeapZoneTile object
        /// </summary>
        /// <param name="bounds">Rectangle boundign box</param>
        /// <param name="texture">Texture2D of the sprite</param>
        /// <param name="type">can be ignored; will always be BossLeapZone</param>
        /// <param name="linkedZones">List of ID integers for linked zones</param>
        /// <param name="id">ID integer for this zone</param>
        public LeapZoneTile(Rectangle bounds, Texture2D texture, ObjectType type, int id) : base(bounds, texture, type)
        {
            this.type = ObjectType.BossLeapZone;
            this.id = id;
            linkedZones = new List<int>();
        }

        public void linkZone(LeapZoneTile tile)
        {
            linkedZones.Add(tile.id);
        }

        public string linkedZonesString()
        {
            string s = "";

            foreach(int i in linkedZones)
            {
                s += i + " ";
            }
            return s;
        }
    }
}
