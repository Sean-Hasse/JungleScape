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
        public static int GRID_SCALE = 50;

        public Map()
        {
            objectMap = new List<GameObject>();
        }

        /// <summary>
        /// Loads GameObjects into the object map.
        /// For now it creates a hard-coded map, but will eventually
        /// load objects from an external Json file created from the map editor.
        /// </summary>
        public void loadMap(List<Texture2D> textures)
        {
            objectMap.Clear();

            //add environment blocks
            for (int i=0; i<10; i++)
            {
                Environment env = new Environment(new Rectangle(i * GRID_SCALE * 2, GRID_SCALE * 8, GRID_SCALE * 2, GRID_SCALE));
                env.sprite = textures.ElementAt(0); //block sprite is at first index of texure list
                objectMap.Add(env);
            }

            //add player
            Player p = new Player(new Rectangle(GRID_SCALE * 2, GRID_SCALE * 6, (int)(GRID_SCALE * 1.5), GRID_SCALE * 2));
            p.sprite = textures.ElementAt(1); //player sprite is at first index of texure list
            objectMap.Add(p);

            //add enemy
            Enemy e = new Enemy(new Rectangle(GRID_SCALE * 8, GRID_SCALE * 6, (int)(GRID_SCALE * 1.5), GRID_SCALE * 2), objectMap.ElementAt(2)); //get 3rd block
            e.sprite = textures.ElementAt(1);  //use player sprite for now
            objectMap.Add(e);
        }

        public void drawMap(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in objectMap)
            {
                spriteBatch.Draw(obj.sprite, obj.hitBox, Color.White);
            }
        }
    }
}
