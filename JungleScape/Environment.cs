using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    class Environment : GameObject
    {
        public Environment(Rectangle hBox, Texture2D texture) : base(hBox, texture)
        {
        }
    }
}
