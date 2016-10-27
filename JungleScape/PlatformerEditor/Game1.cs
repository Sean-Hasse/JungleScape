using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
        List<Background> Backgrounds;
        int currentX;
        int lvlX;
        List<Tile> tiles;
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        MouseState previousMState;
        const int GRID_SIZE = 50;
        Dictionary<ObjectType, Texture2D> tileDict;
        ObjectType currentType;
        private int desiredBBWidth = 1920;
        private int desiredBBHeight = 1080;

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
            currentX = 0;
            lvlX = 0;
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

            background = Content.Load<Texture2D>("BasicBackground");

            tileDict.Add(ObjectType.TopBrick, Content.Load<Texture2D>("PlatformerBrick"));
            tileDict.Add(ObjectType.PlainBrick, Content.Load<Texture2D>("PlainPlatformerBrick"));
            tileDict.Add(ObjectType.Player, Content.Load<Texture2D>("BasicPlayer0"));
            tileDict.Add(ObjectType.Enemy, Content.Load<Texture2D>("SpiderEnemy"));
            // TODO: use this.Content to load your game content here
            
            //Load the background images
            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"BasicBackground"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"BasicBackground"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"BasicBackground"), new Vector2(700, 700), 1.1f));

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

            //Get directional vector based on keyboard input
            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.W))
                direction = new Vector2(0, -1);
            else if (kbState.IsKeyDown(Keys.S))
                direction = new Vector2(0, 1);
            if (kbState.IsKeyDown(Keys.A))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.D))
                direction += new Vector2(1, 0);

            //Update backgrounds
            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, GraphicsDevice.Viewport);

            // TODO: Add your update logic here
            /* if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                currentX += 2;
                lvlX += 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                currentX -= 2;
                lvlX -= 2;
            }
            if (currentX < 0)
                currentX = 0;
            currentX %= GraphicsDevice.Viewport.Bounds.Width * 2;*/

            //change currently selected object type with up/down arrow keys
            //list order loops in order of coded case sequence
            switch (currentType)
            {
                case ObjectType.Delete:
                    if (SingleKeyPress(Keys.Up))
                        currentType = ObjectType.Enemy;
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
                Point currentCoord = getGridCoord(Mouse.GetState().X, Mouse.GetState().Y);
                foreach(Tile tile in tiles)
                {
                    Point checkPoint = new Point(tile.bounds.X, tile.bounds.Y);
                    if (checkPoint == currentCoord)
                    {
                        tiles.Remove(tile);
                        break;
                    }
                }

                if (currentType == ObjectType.Player)
                    tiles.Add(new Tile(new Rectangle(new Point(currentCoord.X, currentCoord.Y - GRID_SIZE), new Point(GRID_SIZE, GRID_SIZE * 2)), tileDict[currentType], currentType));

                else if (currentType == ObjectType.Enemy)
                    tiles.Add(new Tile(new Rectangle(new Point(currentCoord.X, currentCoord.Y - GRID_SIZE / 2), new Point((int)(GRID_SIZE * 1.5), (int)(GRID_SIZE * 1.5))), tileDict[currentType], currentType));

                else if (currentType != ObjectType.Delete)
                    tiles.Add(new Tile(new Rectangle(currentCoord, new Point(GRID_SIZE, GRID_SIZE)), tileDict[currentType], currentType));
                
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            //spriteBatch.Draw(background, new Rectangle(-currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            //spriteBatch.Draw(background, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            //spriteBatch.Draw(background, new Rectangle(2 * GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);

            foreach (var item in tiles)
                spriteBatch.Draw(item.texture, item.bounds, Color.White);

            if(currentType == ObjectType.Player)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(getGridCoord(Mouse.GetState().X, Mouse.GetState().Y - GRID_SIZE), new Point(GRID_SIZE, GRID_SIZE * 2)), Color.White);

            else if(currentType == ObjectType.Enemy)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(getGridCoord(Mouse.GetState().X, Mouse.GetState().Y), new Point((int)(GRID_SIZE * 1.5), (int)(GRID_SIZE * 1.5))), Color.White);

            else if(currentType != ObjectType.Delete)
                spriteBatch.Draw(tileDict[currentType], new Rectangle(getGridCoord(Mouse.GetState().X, Mouse.GetState().Y), new Point(GRID_SIZE, GRID_SIZE)), Color.White);

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
