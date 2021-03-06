﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class BossLeapZone : GameObject
    {
        /// <summary>
        /// id reference for this tile
        /// </summary>
        public int id { get; }

        /// <summary>
        /// list of zone ids this zone is linked to
        /// </summary>
        public List<int> linkedZones { get; }

        public BossLeapZone(Rectangle hBox, Texture2D texture, int id, List<int> linkedZones) : base(hBox, texture)
        {
            this.id = id;
            this.linkedZones = linkedZones;
        }
    }
}
