using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public class Map
    {
        public List<GameObject> objectMap { get; set; }

        public Map()
        {
            objectMap = new List<GameObject>();
        }

        /// <summary>
        /// Loads GameObjects into the object map.
        /// For now it creates a hard-coded map, but will eventually
        /// load objects from an external Json file created from the map editor.
        /// </summary>
        public void loadMap()
        {
            objectMap.Clear();
            for (int i=0; i<10; i++)
            {
                objectMap.Add(new Environment(new Rectangle(new Point(i * 100, 400), new Point(100,50))));
            }
        }

        public void loadSprites()
        {

        }

        public void drawMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (GameObject obj in objectMap)
            {
                spriteBatch.Draw(obj.sprite, obj.hitBox, Color.White);
            }
            spriteBatch.End();
        }
    }
}
