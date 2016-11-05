using JungleScape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlatformerEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        SpriteFont font;
        private Background myBackground;
        List<Tile> tiles;
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        MouseState previousMState;
        const int GRID_SIZE = 50;
        Dictionary<ObjectType, Texture2D> tileDict;
        ObjectType currentType;
        public static int desiredBBWidth = 1600;
        public static int desiredBBHeight = 900;

        public static int backgroundWidth = 3800;
        public static int backgroundHeight = 2160;

        public int xPos = 0;
        public int yPos = 0;
        int lvlY;
        int lvlX;
        private static int camSpeed = 10;
        int currentID;

        bool linkState;
        LeapZoneTile linkTile;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            //change the screen reolution
            graphics.PreferredBackBufferWidth = desiredBBWidth;
            graphics.PreferredBackBufferHeight = desiredBBHeight;
            this.Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            lvlX = 0;
            lvlY = 0;
            currentID = 0;
            linkState = false;
            tiles = new List<Tile>();

            tileDict = new Dictionary<ObjectType, Texture2D>();
            currentType = ObjectType.Delete;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //scrolling background
            myBackground = new Background();
            background = Content.Load<Texture2D>("BasicBackground");
            //myBackground.Load(GraphicsDevice, background);

            tileDict.Add(ObjectType.TopBrick, Content.Load<Texture2D>("PlatformerBrick"));
            tileDict.Add(ObjectType.PlainBrick, Content.Load<Texture2D>("PlainPlatformerBrick"));
            tileDict.Add(ObjectType.Player, Content.Load<Texture2D>("BasicPlayer0"));
            tileDict.Add(ObjectType.Enemy, Content.Load<Texture2D>("SpiderEnemy"));
            tileDict.Add(ObjectType.Boss, Content.Load<Texture2D>("Boss Enemy0"));

            font = Content.Load<SpriteFont>("testFont");
            // TODO: use this.Content to load your game content here
            loadCurrentMap();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //define keyboard and mouse states
            previousKbState = kbState;
            kbState = Keyboard.GetState();
            previousMState = mState;
            mState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Get directiona vector based on keyboard input
            if (kbState.IsKeyDown(Keys.W))
            {
                yPos -= camSpeed;
                lvlY -= camSpeed;
            }
      
            else if (kbState.IsKeyDown(Keys.S))
            {
                yPos += camSpeed;
                lvlY += camSpeed;
            }
            
            if (kbState.IsKeyDown(Keys.A))
            {
                xPos -= camSpeed;
                lvlX -= camSpeed;
            }

            else if (kbState.IsKeyDown(Keys.D))
            {
                xPos += camSpeed;
                lvlX += camSpeed;
            }

            //scrolling background
            // The time since Update was called last.
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            //myBackground.Update(elapsed * 100);

            //change currently selected object type with up/down arrow keys
            //list order loops in order of coded case sequence
            switch (currentType)
            {
                case ObjectType.Delete:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Link;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.TopBrick;
                    break;
                case ObjectType.TopBrick:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Delete;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.PlainBrick;
                    break;
                case ObjectType.PlainBrick:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.TopBrick;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.Player;
                    break;
                case ObjectType.Player:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.PlainBrick;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.Enemy;
                    break;
                case ObjectType.Enemy:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Player;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.Boss;
                    break;
                case ObjectType.Boss:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Enemy;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.BossLeapZone;
                    break;
                case ObjectType.BossLeapZone:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Boss;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.Link;
                    break;
                case ObjectType.Link:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.BossLeapZone;
                    if (SingleKeyPress(Keys.Down))
                        currentType = ObjectType.Delete;
                    break;
                default:
                    currentType = ObjectType.Delete;
                    break;
            }

            //on single mouse click, add a new tile object to the selected location
            //if there is a tile already there, delete it before adding the new one
            if (SingleMouseClick())
            {
                //current coordinates of mouse click
                Point currentCoord = getGridCoord(Mouse.GetState().X + lvlX, Mouse.GetState().Y + lvlY);
                foreach(Tile tile in tiles)
                {
                    Point checkPoint = getGridCoord(tile.bounds.X, tile.bounds.Y);
                    if (currentCoord == checkPoint && !linkState)
                    {
                        tiles.Remove(tile);
                        break;
                    }
                }

                if (!linkState)
                {
                    //add object to list of tiles
                    if (currentType == ObjectType.Player)
                        tiles.Add(new Tile(new Rectangle(new Point(currentCoord.X, currentCoord.Y - GRID_SIZE), new Point(GRID_SIZE, GRID_SIZE * 2)), tileDict[currentType], currentType));

                    else if (currentType == ObjectType.Enemy)
                        tiles.Add(new Tile(new Rectangle(new Point(currentCoord.X, currentCoord.Y - GRID_SIZE / 2), new Point((int)(GRID_SIZE * 1.5), (int)(GRID_SIZE * 1.5))), tileDict[currentType], currentType));

                    else if (currentType == ObjectType.Boss)
                        tiles.Add(new Tile(new Rectangle(new Point(currentCoord.X, currentCoord.Y - GRID_SIZE / 2), new Point(GRID_SIZE * 4, (int)(GRID_SIZE * 1.5))), tileDict[currentType], currentType));

                    else if (currentType == ObjectType.BossLeapZone)
                    {
                        currentID++;
                        tiles.Add(new LeapZoneTile(new Rectangle(new Point(currentCoord.X, currentCoord.Y), new Point(GRID_SIZE * 4, GRID_SIZE)), tileDict[ObjectType.PlainBrick], currentType, currentID));
                    }

                    else if (currentType == ObjectType.Link)
                    {
                        linkState = true;
                        foreach (LeapZoneTile tile in tiles.OfType<LeapZoneTile>())
                        {
                            Point checkPoint = getGridCoord(tile.bounds.X, tile.bounds.Y);
                            if (currentCoord == checkPoint)
                            {
                                linkTile = tile;
                                break;
                            }
                        }
                    }


                    else if (currentType != ObjectType.Delete)
                        tiles.Add(new Tile(new Rectangle(currentCoord, new Point(GRID_SIZE, GRID_SIZE)), tileDict[currentType], currentType));
                }
                else
                {
                    foreach (LeapZoneTile tile in tiles.OfType<LeapZoneTile>())
                    {
                        Point checkPoint = getGridCoord(tile.bounds.X, tile.bounds.Y);
                        if (currentCoord == checkPoint)
                        {
                            ((LeapZoneTile)(tiles.Find(t => t == linkTile))).linkedZones.Add(tile.id);
                            tile.linkedZones.Add(linkTile.id);
                        }
                    }
                }
                
                
            }

            //saves current list of Tiles into JungleScape's Content folder
            if (SingleKeyPress(Keys.Enter))
            {
                StreamWriter joutput = new StreamWriter("../../../../../JungleScape/Content/level.json");
                joutput.WriteLine(JsonConvert.SerializeObject(tiles));
                joutput.Close();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, myBackground.translation(xPos + desiredBBWidth / 2, yPos + desiredBBHeight / 2));

            //draws backgrounds
            //myBackground.Draw(spriteBatch);
            //spriteBatch.Draw(background, new Rectangle(-lvlX, 0, backgroundWidth, backgroundWidth), Color.White);
            //spriteBatch.Draw(background, new Rectangle(-lvlX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            //spriteBatch.Draw(background, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - lvlX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            //spriteBatch.Draw(background, new Rectangle(2 * GraphicsDevice.Viewport.Width - lvlX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            //draw all the tiles in the list of tiles (the current map)
            foreach (var item in tiles)
            {
                spriteBatch.Draw(item.texture, item.bounds, Color.White);
                if(item is LeapZoneTile)
                {
                    LeapZoneTile thisTile = (LeapZoneTile)item;
                    spriteBatch.DrawString(font, thisTile.id.ToString(), new Vector2(item.bounds.X, item.bounds.Y), Color.Purple);
                }
                    
            }
                

            Point gridCoord = getGridCoord(Mouse.GetState().X + lvlX, Mouse.GetState().Y + lvlY);
            //draw hovering tile at the cursor
            if (currentType == ObjectType.Player)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(getGridCoord(Mouse.GetState().X + lvlX, Mouse.GetState().Y - GRID_SIZE + lvlY), new Point(GRID_SIZE, GRID_SIZE * 2)), Color.White);

            else if(currentType == ObjectType.Enemy)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(gridCoord, new Point((int)(GRID_SIZE * 1.5), (int)(GRID_SIZE * 1.5))), Color.White);

            else if(currentType == ObjectType.Boss)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(gridCoord, new Point(GRID_SIZE * 4, (int)(GRID_SIZE * 1.5))), Color.White);

            else if(currentType == ObjectType.BossLeapZone)
                spriteBatch.Draw(tileDict[ObjectType.PlainBrick], new Rectangle(gridCoord, new Point(GRID_SIZE * 4, GRID_SIZE)), Color.White);

            else if(currentType == ObjectType.Link)
                spriteBatch.Draw(tileDict[ObjectType.Player], new Rectangle(gridCoord, new Point(GRID_SIZE, GRID_SIZE)), Color.White);

            else if(currentType != ObjectType.Delete)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(gridCoord, new Point(GRID_SIZE, GRID_SIZE)), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Checks for single key press.
        /// </summary>
        /// <param name="key">input key</param>
        /// <returns>boolean</returns>
        private bool SingleKeyPress(Keys key)
        {
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks for single mouse click.
        /// </summary>
        /// <returns>boolean</returns>
        private bool SingleMouseClick()
        {
            if (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Calculates the relative grid coordinates from given x/y values.
        /// </summary>
        private Point getGridCoord(int givenX, int givenY)
        {
            int x = (int)Math.Round((double)(givenX / GRID_SIZE)) * GRID_SIZE;
            int y = (int)Math.Round((double)(givenY / GRID_SIZE)) * GRID_SIZE;
            if (x > givenX)
                x -= GRID_SIZE;
            if (y > givenY)
                y -= GRID_SIZE;

            return new Point(x, y);
        }

        /// <summary>
        /// loads in the level.json file and saves the level data to the list of Tiles.
        /// </summary>
        private void loadCurrentMap()
        {
            StreamReader jinput = new StreamReader("../../../../../JungleScape/Content/level.json");
            tiles = JsonConvert.DeserializeObject<List<Tile>>(jinput.ReadToEnd());
            jinput.Close();

            foreach(Tile tile in tiles)
                tile.texture = tileDict[tile.type];
        }
    }
}
