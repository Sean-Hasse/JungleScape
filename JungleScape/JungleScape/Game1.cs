using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace JungleScape
{
    /// <summary>
    /// Available GameStates
    /// </summary>
    enum GameState
    {
        Menu,
        Instructions,
        Game,
        Pause,
        GameOver
    };
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // attempting to make an enemy
        Enemy spider;
        // call spider in LoadContent

        Map levelMap;
        List<Texture2D> textures;
        GameState myState;
        int menuIndex;
        KeyboardState previousKbState;
        KeyboardState kbState;
        SpriteFont testFont;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            levelMap = new Map();
            textures = new List<Texture2D>();
            myState = GameState.Menu;
            menuIndex = 0;

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

            // TODO: use this.Content to load your game content here

            // initialize and call spider here
            //spider = new Enemy(new Point(0, 0), new Rectangle(0, 0, 20, 20), 5);
            //spider.Move(spider.speed, 0);

            //order of adding textures is important for map loading
            textures.Add(Content.Load<Texture2D>("firstPlatformerBrick"));
            textures.Add(Content.Load<Texture2D>("BasicPlayer"));

            testFont = Content.Load<SpriteFont>("testFont");

            levelMap.loadMap(textures);
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

            // TODO: Add your update logic here
            
            previousKbState = kbState;
            kbState = Keyboard.GetState();
            switch (myState)
            {
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        menuIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        menuIndex -= 1;

                    if (menuIndex >= 3)
                        menuIndex = 0;
                    else if (menuIndex < 0)
                        menuIndex = 2;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 0)
                        myState = GameState.Game;
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 1)
                        myState = GameState.Instructions;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && menuIndex == 2)
                        Exit();

                    break;

                case GameState.Instructions:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) || SingleKeyPress(Keys.Back, kbState, previousKbState))
                        myState = GameState.Menu;
                    break;

                case GameState.Game:
                    foreach (Character chara in levelMap.objectMap.OfType<Character>())
                    {
                    chara.Move();
                    }

                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Pause;
                    else if (Keyboard.GetState().IsKeyDown(Keys.G))
                        myState = GameState.GameOver;
                    break;

                case GameState.Pause:
                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Game;
                    break;

                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState))
                    {
                        myState = GameState.Menu;
                        menuIndex = 0;
                    }
                       
                    break;

                default:
                    break;
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
            spriteBatch.Begin();

            switch (myState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(testFont, "This is a Menu", new Vector2(0, 0), Color.White);
                    if (menuIndex == 0)
                        spriteBatch.DrawString(testFont, "Start Game is selected", new Vector2(0, 50), Color.White);
                    else if (menuIndex == 1)
                        spriteBatch.DrawString(testFont, "Instructions are selected", new Vector2(0, 50), Color.White);
                    else
                        spriteBatch.DrawString(testFont, "exit game is selected", new Vector2(0, 50), Color.White);
                    break;

                case GameState.Instructions:
                    spriteBatch.DrawString(testFont, "These are Instructions", new Vector2(0, 0), Color.White);
                    break;

                case GameState.Game:
                    levelMap.drawMap(spriteBatch);
                    spriteBatch.DrawString(testFont, "This is a Game Screen", new Vector2(0, 0), Color.White);
                    break;

                case GameState.Pause:
                    spriteBatch.DrawString(testFont, "This is a pause menu", new Vector2(0, 0), Color.White);
                    break;

                case GameState.GameOver:
                    spriteBatch.DrawString(testFont, "Game over man, game over.", new Vector2(0, 0), Color.White);
                    break;

                default:
                    break;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public bool SingleKeyPress(Keys key, KeyboardState kbState, KeyboardState previousKbState)
        {
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
