using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Input;
using PlatformerEditor;

namespace JungleScape
{
    public class Map
    {
        public List<GameObject> objectMap { get; set; }
        public static int GRID_SCALE = 50;
        public Camera cam;
        public List<BossLeapZone> leapZoneMap { get; set; }

        JsonSerializerSettings settings;

        public Map()
        {
            objectMap = new List<GameObject>();
            leapZoneMap = new List<BossLeapZone>();
            settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        }

        /// <summary>
        /// Loads GameObjects into the object map.
        /// Loads objects from an external Json file created from the map editor.
        /// </summary>
        public void loadMap(Dictionary<ObjectType, Texture2D> textures)
        {
            objectMap.Clear();

            StreamReader jinput = new StreamReader("../../../../Content/level.json");
            List<Tile> inputList = JsonConvert.DeserializeObject<List<Tile>>(jinput.ReadToEnd(), settings);
            jinput.Close();

            foreach(Tile tile in inputList)
            {
                switch (tile.type)
                {
                    case ObjectType.TopBrick:
                        objectMap.Add(new Environment(tile.bounds, textures[ObjectType.TopBrick]));
                        break;
                    case ObjectType.PlainBrick:
                        objectMap.Add(new Environment(tile.bounds, textures[ObjectType.PlainBrick]));
                        break;
                    case ObjectType.Player:
                        objectMap.Add(new Player(tile.bounds, textures[ObjectType.Player], 3));
                        break;
                    case ObjectType.Enemy:
                        objectMap.Add(new Enemy(tile.bounds, objectMap, textures[ObjectType.Enemy], 2));
                        break;
                    case ObjectType.Boss:
                        objectMap.Add(new Boss(tile.bounds, objectMap, textures[ObjectType.Boss], 10, leapZoneMap));
                        break;
                    case ObjectType.BossLeapZone:
                        //Leap Zone tiles will be LeapZoneTile type instead of a normal tile, and a cast is needed to get extra attributes.
                        LeapZoneTile zoneTile = (LeapZoneTile)tile;
                        BossLeapZone zone = new BossLeapZone(zoneTile.bounds, textures[ObjectType.BossLeapZone], zoneTile.id, zoneTile.linkedZones);
                        objectMap.Add(zone);
                        leapZoneMap.Add(zone);
                        break;
                }
            }
            cam = new Camera(findPlayer());
        }

        public void drawMap(SpriteBatch spriteBatch, List<Texture2D> textures, KeyboardState kbState )
        {
            foreach (GameObject obj in objectMap)
            {
                //If map object is the player, it takes the keyboard state and draws where the player is currently aiming
                if (obj is Player)
                {
                    
                    if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Up) && kbState.IsKeyDown(Keys.Right))
                        spriteBatch.Draw(textures[1], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    else if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Up) && kbState.IsKeyDown(Keys.Left))
                        spriteBatch.Draw(textures[1], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Up))
                        spriteBatch.Draw(textures[2], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    else if (kbState.IsKeyDown(Keys.Right) && kbState.IsKeyDown(Keys.A))
                        spriteBatch.Draw(textures[0], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    else if (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Up))
                        spriteBatch.Draw(textures[1], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (kbState.IsKeyDown(Keys.Left))
                        spriteBatch.Draw(textures[0], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (kbState.IsKeyDown(Keys.A))
                        spriteBatch.Draw(textures[0], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (kbState.IsKeyDown(Keys.Up) && kbState.IsKeyDown(Keys.Right))
                        spriteBatch.Draw(textures[1], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    else if (kbState.IsKeyDown(Keys.Up))
                        spriteBatch.Draw(textures[2], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(textures[0], obj.hitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                }
                else
                    spriteBatch.Draw(obj.sprite, obj.hitBox, Color.White);
            }
        }

        /// <summary>
        /// Finds the player in the game map.
        /// </summary>
        /// <returns>Player object</returns>
        public Player findPlayer()
        {
            Player pref = new Player(new Rectangle(), null, 0);
            return (Player)objectMap.Find(p => p.GetType() == pref.GetType());
        }
    }
}
