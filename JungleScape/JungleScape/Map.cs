using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
        public void loadmap()
        {
            objectMap.Clear();
            objectMap.Add(new Environment(new Point(100, 100), new Rectangle()));
        }
    }
}
