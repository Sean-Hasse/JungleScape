using LevelEditor;
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
        Editor,
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
        GameState previousGameState;
        int menuIndex;
        int pauseIndex;
        int gameOverIndex;
        KeyboardState previousKbState;
        KeyboardState kbState;
        KeyboardState aimState;
        SpriteFont testFont;
        SpriteFont testFont2;
        MapEditor editor;
        Texture2D player45;
        Texture2D player90;
        

        //Texture2D background;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //change the screen resolution
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
            levelMap = new Map();
            editor = new MapEditor();
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
            textures.Add(Content.Load<Texture2D>("PlatformerBrick"));
            textures.Add(Content.Load<Texture2D>("BasicPlayer0"));
            textures.Add(Content.Load<Texture2D>("SpiderEnemy"));
            textures.Add(Content.Load<Texture2D>("PlainPlatformerBrick"));
            testFont = Content.Load<SpriteFont>("testFont");
            testFont2 = Content.Load<SpriteFont>("testFont2");
            textures.Add(Content.Load<Texture2D>("BasicPlayer45"));
            textures.Add(Content.Load<Texture2D>("BasicPlayer90"));
            //background = Content.Load<Texture2D>("BasicBackground");

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

                    if (menuIndex >= 4)
                        menuIndex = 0;
                    else if (menuIndex < 0)
                        menuIndex = 3;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 0)
                        myState = GameState.Game;
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 1)
                    {
                        previousGameState = GameState.Menu;
                        myState = GameState.Instructions;
                    }
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 2)
                    {
                        myState = GameState.Editor;
                    }
                        
                    else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && menuIndex == 3)
                        Exit();

                    break;

                case GameState.Instructions:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) || SingleKeyPress(Keys.Back, kbState, previousKbState))
                        if (previousGameState == GameState.Menu)
                            myState = GameState.Menu;
                        else
                            myState = GameState.Pause;
                    break;

                case GameState.Game:
                    foreach (Character chara in levelMap.objectMap.OfType<Character>())
                    {
                        
                        if (chara is Player)
                        {
                            Player player1 = (Player)chara;
                            player1.Move(levelMap.objectMap);
                            player1.FireArrow(textures[1], levelMap.objectMap);
                        }
                        else
                            chara.Move();

                        aimState = Keyboard.GetState();

                    }

                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Pause;
                    else if (Keyboard.GetState().IsKeyDown(Keys.G))
                        myState = GameState.GameOver;
                    break;

                case GameState.Editor:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) || SingleKeyPress(Keys.Back, kbState, previousKbState))
                        if (previousGameState == GameState.Menu)
                            myState = GameState.Menu;
                        else
                            myState = GameState.Pause;
                    break;

                case GameState.Pause:
                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Game;

                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        pauseIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        pauseIndex -= 1;

                    if (pauseIndex >= 3)
                        pauseIndex = 0;
                    else if (pauseIndex < 0)
                        pauseIndex = 2;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && pauseIndex == 0)
                        myState = GameState.Game;
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && pauseIndex == 1)
                    {
                        previousGameState = GameState.Pause;
                        myState = GameState.Instructions;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && pauseIndex == 2)
                        myState = GameState.Menu;

                    break;

                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        gameOverIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        gameOverIndex -= 1;

                    if (gameOverIndex >= 3)
                        gameOverIndex = 0;
                    else if (gameOverIndex < 0)
                        gameOverIndex = 2;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && gameOverIndex == 0)
                        myState = GameState.Game;
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && gameOverIndex == 1)
                        myState = GameState.Menu;
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && gameOverIndex == 2)
                        Exit();

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
                    spriteBatch.DrawString(testFont2, "JungleScape", new Vector2(10, 0), Color.White);
                    spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 150), Color.White);
                    spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 225), Color.White);
                    spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 300), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 375), Color.White);

                    if (menuIndex == 0)
                        spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 150), Color.Yellow);
                    else if (menuIndex == 1)
                        spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 225), Color.Yellow);
                    else if (menuIndex == 2)
                        spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 300), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 375), Color.Yellow);
                    break;

                case GameState.Instructions:
                    spriteBatch.DrawString(testFont, "These are Instructions:", new Vector2(0, 0), Color.White);
                    //spriteBatch.DrawString(testFont, "Move and Shoot:", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Press the A key to move left.", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Press the D key to move right.", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Press the W key to move to jump.", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "Press the spacebar to shoot.", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Can't shoot right now. :(", new Vector2(0, 300), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 400), Color.White);
                    break;

                case GameState.Editor:
                    spriteBatch.DrawString(testFont, "How To Use The Map Editor:", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "Create new instance of the editor.", new Vector2(0, 50), Color.White);
                    spriteBatch.DrawString(testFont, "Left click to place game objects.", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Press up/down keys to pick options.", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Left click on 'nothing' to delete.", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "You must include a player.", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Once you are done press 'Enter'.", new Vector2(0, 300), Color.White);
                    spriteBatch.DrawString(testFont, "Run the game again.", new Vector2(0, 350), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 400), Color.White);
                    break;

                case GameState.Game:
                    levelMap.drawMap(spriteBatch, textures, aimState);
                    spriteBatch.DrawString(testFont, "This is a Game Screen", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'G' key to initiate game over", new Vector2(0, 50), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'P' key to pause", new Vector2(0, 100), Color.White);
                    //spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Pause:
                    spriteBatch.DrawString(testFont2, "Paused", new Vector2(230, 10), Color.White);
                    spriteBatch.DrawString(testFont, "Resume Game", new Vector2(230, 150), Color.White);
                    spriteBatch.DrawString(testFont, "How do I Play Again?", new Vector2(150, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Return to Menu", new Vector2(225, 350), Color.White);

                    if (pauseIndex == 0)
                        spriteBatch.DrawString(testFont, "Resume Game", new Vector2(230, 150), Color.Yellow);
                    else if (pauseIndex == 1)
                        spriteBatch.DrawString(testFont, "How do I Play Again?", new Vector2(150, 250), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Return to Menu", new Vector2(225, 350), Color.Yellow);

                    break;

                case GameState.GameOver:
                    spriteBatch.DrawString(testFont2, "Oops", new Vector2(280, 10), Color.White);
                    spriteBatch.DrawString(testFont, "Try Again?", new Vector2(270, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(240, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Forget It.", new Vector2(285, 350), Color.White);

                    if (gameOverIndex == 0)
                        spriteBatch.DrawString(testFont, "Try Again?", new Vector2(270, 150), Color.Yellow);
                    else if (gameOverIndex == 1)
                        spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(240, 250), Color.Yellow);
                    else if (gameOverIndex == 2)
                        spriteBatch.DrawString(testFont, "Forget It.", new Vector2(285, 350), Color.Yellow);

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
