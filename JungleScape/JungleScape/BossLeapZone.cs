using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public enum BossLeapDirection { Left, Right, UpLeft, UpRight, DownLeft, DownRight }

    public class BossLeapZone : GameObject
    {
        public List<BossLeapZone> linkedZones { get; }
        public BossLeapDirection direction { get; }

        public BossLeapZone(Rectangle hBox, Texture2D texture, BossLeapDirection direction) : base(hBox, texture)
        {
            this.direction = direction;
            linkedZones = new List<BossLeapZone>();
        }

        public void AddZones(List<BossLeapZone> zones)
        {
            linkedZones.AddRange(zones);
        }
    }
}
