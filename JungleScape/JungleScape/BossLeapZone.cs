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
        public Dictionary<BossLeapDirection, BossLeapZone> linkedZones { get; }

        public BossLeapZone(Rectangle hBox, Texture2D texture) : base(hBox, texture)
        {
            linkedZones = new Dictionary<BossLeapDirection, BossLeapZone>();
        }

        public void AddZone(BossLeapDirection direction, BossLeapZone zone)
        {
            linkedZones.Add(direction, zone);
        }
    }
}
