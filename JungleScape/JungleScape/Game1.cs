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
    public enum GameState
    {
        Menu,
        Instructions,
        Story,
        Options,
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

        Map levelMap;
        Dictionary<ObjectType, Texture2D> textures;
        List<Texture2D> playerTextures;
        public static GameState myState;
        GameState previousGameState;
        int menuIndex;
        int pauseIndex;
        int gameOverIndex;
        int optionsIndex;
        KeyboardState previousKbState;
        KeyboardState kbState;
        KeyboardState aimState;
        SpriteFont testFont;
        SpriteFont testFont2;
        MapEditor editor;
        Texture2D background;
        public static int desiredBBWidth = 1920;
        public static int desiredBBHeight = 1080;

        public Player playerCamRef;

        //Texture2D background;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            //change the screen resolution
            graphics.PreferredBackBufferWidth = desiredBBWidth;
            graphics.PreferredBackBufferHeight = desiredBBHeight;
            this.Window.AllowUserResizing = true;
        }

        /* possible fix to updating the screen res
        internal void ChangeClientBounds(Rectangle clientBounds)
        {
            updateClientBounds = true;
            this.clientBounds = clientBounds;
        }
        */
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
            playerTextures = new List<Texture2D>();
            textures = new Dictionary<ObjectType, Texture2D>();
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

            Texture2D basePlayer = Content.Load<Texture2D>("BasicPlayer0");

            //all initial object textures
            textures.Add(ObjectType.TopBrick, Content.Load<Texture2D>("PlatformerBrick"));
            textures.Add(ObjectType.Player, basePlayer);
            textures.Add(ObjectType.Enemy, Content.Load<Texture2D>("SpiderEnemy"));
            textures.Add(ObjectType.PlainBrick, Content.Load<Texture2D>("PlainPlatformerBrick"));

            //fonts
            testFont = Content.Load<SpriteFont>("testFont");
            testFont2 = Content.Load<SpriteFont>("testFont2");

            //list of player sprites
            playerTextures.Add(basePlayer);
            playerTextures.Add(Content.Load<Texture2D>("BasicPlayer45"));
            playerTextures.Add(Content.Load<Texture2D>("BasicPlayer90"));
            background = Content.Load<Texture2D>("BasicBackground");

            //load the map and initialize the camera player reference object
            levelMap.loadMap(textures);
            playerCamRef = new Player(new Rectangle(desiredBBWidth / 2, desiredBBHeight / 2, 0, 0), null);
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

            if(myState != GameState.Game)
            {
                playerCamRef = new Player(new Rectangle(desiredBBWidth/2, desiredBBHeight/2, 0, 0), null);
            }

            switch (myState)
            {
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        menuIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        menuIndex -= 1;

                    if (menuIndex >= 5)
                        menuIndex = 0;
                    else if (menuIndex < 0)
                        menuIndex = 4;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 0)
                        myState = GameState.Story;
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 1)
                    {
                        previousGameState = GameState.Menu;
                        myState = GameState.Instructions;
                    }
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 2)
                    {
                        myState = GameState.Editor;
                    }
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && menuIndex == 3)
                        myState = GameState.Options;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && menuIndex == 4)
                        Exit();

                    break;

                case GameState.Instructions:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) || SingleKeyPress(Keys.Back, kbState, previousKbState))
                        if (previousGameState == GameState.Menu)
                            myState = GameState.Menu;
                        else
                            myState = GameState.Pause;
                    break;


                case GameState.Story:
                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState))
                        myState = GameState.Game;
                    break;

                case GameState.Options:
                    playerCamRef = new Player(new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), null);

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState))
                        myState = GameState.Menu;
                    break;


                case GameState.Game:
                    playerCamRef = levelMap.findPlayer();
                    foreach (Character chara in levelMap.objectMap.OfType<Character>())
                    {
                        
                        if (chara is Player)
                        {
                            Player player1 = (Player)chara;
                            player1.Move(levelMap.objectMap);
                            player1.FireArrow(textures[ObjectType.Player], levelMap.objectMap);
                        }
                        else
                            chara.Move();

                        aimState = Keyboard.GetState();

                    }
                    

                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Pause;
                    else if (SingleKeyPress(Keys.G, kbState, previousKbState))
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
                    {
                        myState = GameState.Menu;
                        Initialize();
                    }

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
                    {
                        Initialize();
                        myState = GameState.Game;
                    }
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
            spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,levelMap.cam.translation(playerCamRef));
            
            switch (myState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(testFont2, "JungleScape", new Vector2(10, 0), Color.White);
                    spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 150), Color.White);
                    spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 225), Color.White);
                    spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 300), Color.White);
                    spriteBatch.DrawString(testFont, "Options", new Vector2(20, 375), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 450), Color.White);

                    if (menuIndex == 0)
                        spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 150), Color.Yellow);
                    else if (menuIndex == 1)
                        spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 225), Color.Yellow);
                    else if (menuIndex == 2)
                        spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 300), Color.Yellow);
                    else if (menuIndex == 3)
                        spriteBatch.DrawString(testFont, "Options", new Vector2(20, 375), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 450), Color.Yellow);
                    break;

                case GameState.Options:
                    spriteBatch.DrawString(testFont, "Set Your Screen Resolution ", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "1024 x 768", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "1280 x 1024", new Vector2(0, 225), Color.White);
                    spriteBatch.DrawString(testFont, "1600 x 900", new Vector2(0, 300), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Menu", new Vector2(0, 450), Color.White);

                    if (pauseIndex == 0)
                        spriteBatch.DrawString(testFont, "1024 x 768", new Vector2(0, 150), Color.Yellow);
                    else if (pauseIndex == 1)
                        spriteBatch.DrawString(testFont, "1280 x 1024", new Vector2(0, 225), Color.Yellow);
                    else if (pauseIndex == 2)
                        spriteBatch.DrawString(testFont, "1600 x 900", new Vector2(0, 300), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Exit Menu", new Vector2(0, 450), Color.Yellow);

                    break;

                case GameState.Instructions:
                    spriteBatch.DrawString(testFont, "How To Play The Game:", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "Move and Shoot:", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Press the W key to jump.", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Press the A key to move left.", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "Press the D key to move right.", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Combine the keys to move and jump at the same time in one direction.", new Vector2(0, 300), Color.White);
                    spriteBatch.DrawString(testFont, "Press the spacebar to shoot at an enemy spider.", new Vector2(0, 350), Color.White);
                    spriteBatch.DrawString(testFont, "To Win:", new Vector2(0, 450), Color.White);
                    spriteBatch.DrawString(testFont, "Reach the end of the level without dying, there are no checkpoints.", new Vector2(0, 500), Color.White);
                    spriteBatch.DrawString(testFont, "Be prepared to fight the boss at the end of the game.", new Vector2(0, 550), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 600), Color.White);
                    break;

                case GameState.Story:
                    spriteBatch.DrawString(testFont, "JungleScape Storyline:", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "The player was going to a professional archery competition in South America.", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Suddenly his plane crashes in the middle of wild, unexplored jungle.", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "The player has to survive huge bugs and wild animals.", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "All while solving puzzles to find his way out.", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Creators:", new Vector2(0, 350), Color.White);
                    spriteBatch.DrawString(testFont, "Project Manager: Alexia Bernardo", new Vector2(0, 400), Color.White);
                    spriteBatch.DrawString(testFont, "Architect: Sean Hasse", new Vector2(0, 450), Color.White);
                    spriteBatch.DrawString(testFont, "Game Designer: Brady Murren", new Vector2(0, 500), Color.White);
                    spriteBatch.DrawString(testFont, "Interface Designer: Max Kaiser", new Vector2(0, 550), Color.White);
                    spriteBatch.DrawString(testFont, "hit ENTER to advance to game", new Vector2(0, 650), Color.White);
                    break;

                case GameState.Editor:
                    spriteBatch.DrawString(testFont, "How To Use The Map Editor:", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "Begin by starting a new instance of the editor to view the screen.", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "Place the game objects into the game by left clicking on your mouse.", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "Scroll through the options by clicking on the up and down keys.", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Overwrite your mistake by clicking a different object into place.", new Vector2(0, 350), Color.White);
                    spriteBatch.DrawString(testFont, "The default option is 'nothing' and will delete anything you do not want.", new Vector2(0, 400), Color.White);
                    spriteBatch.DrawString(testFont, "Do keep in mind in order to play you must include a player.", new Vector2(0, 500), Color.White);
                    spriteBatch.DrawString(testFont, "When you are done press enter, and run the game. Enjoy!", new Vector2(0, 600), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 700), Color.White);
                    break;

                case GameState.Game:
                    spriteBatch.Draw(background, new Rectangle(0,0,desiredBBWidth,desiredBBHeight), Color.White);
                    levelMap.drawMap(spriteBatch, playerTextures, kbState);
                    spriteBatch.DrawString(testFont, "This is a Game Screen", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'G' key to initiate game over", new Vector2(0, 50), Color.White);
                    spriteBatch.DrawString(testFont, "hit 'P' key to pause", new Vector2(0, 100), Color.White);
                    
                    break;

                case GameState.Pause:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.Gray);
                    levelMap.drawMap(spriteBatch, playerTextures, kbState);
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
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.Red);
                    spriteBatch.DrawString(testFont2, "Oops", new Vector2(280, 10), Color.White);
                    spriteBatch.DrawString(testFont, "Try Again?", new Vector2(270, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(240, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Quit Game", new Vector2(285, 350), Color.White);

                    if (gameOverIndex == 0)
                        spriteBatch.DrawString(testFont, "Try Again?", new Vector2(270, 150), Color.Yellow);
                    else if (gameOverIndex == 1)
                        spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(240, 250), Color.Yellow);
                    else if (gameOverIndex == 2)
                        spriteBatch.DrawString(testFont, "Quit Game", new Vector2(285, 350), Color.Yellow);

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
