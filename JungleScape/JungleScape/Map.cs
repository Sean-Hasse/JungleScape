using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

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
        public void loadMap(Dictionary<ObjectType, Texture2D> textures)
        {
            objectMap.Clear();

            /*
            //add environment blocks
            for (int i=0; i<10; i++)
            {
                //block sprite is at first index of texure list
                objectMap.Add(new Environment(new Rectangle(i * GRID_SCALE * 2, GRID_SCALE * 8, GRID_SCALE * 2, GRID_SCALE), textures.ElementAt(0)));
            }

            //add player
            //player sprite is at first index of texure list
            objectMap.Add(new Player(new Rectangle(GRID_SCALE * 2, GRID_SCALE * 6, (int)(GRID_SCALE * 1.5), GRID_SCALE * 2), textures.ElementAt(1)));

            //add enemy
            //use player sprite for now
            objectMap.Add(new Enemy(new Rectangle(GRID_SCALE * 8, (int)(GRID_SCALE * 6.5), (GRID_SCALE * 2), (int)(GRID_SCALE * 1.5)), objectMap, textures.ElementAt(2)));
            */

            StreamReader jinput = new StreamReader("../../../../Content/level.json");
            List<Tile> inputList = JsonConvert.DeserializeObject<List<Tile>>(jinput.ReadToEnd());
            jinput.Close();

            foreach(Tile tile in inputList)
            {
                switch (tile.type)
                {
                    case ObjectType.TopBrick:
                        objectMap.Add(new Environment(tile.bounds, textures.ElementAt(0)));
                        break;
                    case ObjectType.PlainBrick:
                        objectMap.Add(new Environment(tile.bounds, textures.ElementAt(3)));
                        break;
                    case ObjectType.Player:
                        objectMap.Add(new Player(tile.bounds, textures.ElementAt(1)));
                        break;
                    case ObjectType.Enemy:
                        objectMap.Add(new Enemy(tile.bounds, objectMap, textures.ElementAt(2)));
                        break;
                }
            }
        }

        public void drawMap(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in objectMap)
            {
                if (obj is Character)
                {
                    
                }
                spriteBatch.Draw(obj.sprite, obj.hitBox, Color.White);
            }
        }
    }
}
