﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEditor;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        Victory,
        GameOver
    }
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
        List<Texture2D> healthTextures;
        public static GameState myState;
        GameState previousGameState;
        int menuIndex;
        int pauseIndex;
        int gameOverIndex;
        int optionsIndex;
        int victoryIndex;
        KeyboardState previousKbState;
        KeyboardState kbState;
        KeyboardState aimState;
        SpriteFont testFont;
        SpriteFont testFont2;
        public Texture2D arrowImage;
        List<Arrow> arrows;
        //MapEditor editor;
        Texture2D background;
        Texture2D logo;
        
        public static int desiredBBWidth = 1600;
        public static int desiredBBHeight = 900;

        public static int backgroundWidth = 2500;
        public static int backgroundHeight = 1500;

        public Player playerCamRef;

        List<Enemy> enemies = new List<Enemy>();
        public static List<SoundEffect> soundEffects;
        public static List<Song> songs;

        private int gameMusicTimer;
        private int BossBattleTimer;
        private bool BossBattleHasStarted = false;

        //Texture2D background;

        public static void PlaySound(int n)
        {
            soundEffects[n].Play();
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            //change the screen resolution
            graphics.PreferredBackBufferWidth = desiredBBWidth;
            graphics.PreferredBackBufferHeight = desiredBBHeight;
            this.Window.AllowUserResizing = true;

            soundEffects = new List<SoundEffect>();
            songs = new List<Song>();
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
            //editor = new MapEditor();
            playerTextures = new List<Texture2D>();
            healthTextures = new List<Texture2D>();
            textures = new Dictionary<ObjectType, Texture2D>();
            myState = GameState.Menu;
            menuIndex = 0;
            BossBattleHasStarted = false;

            // create a list of arrows to be drawn
            arrows = new List<Arrow>();

            //set music timer back to 0 (see logic when playing songs)
            gameMusicTimer = 0;
            BossBattleTimer = 0;

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
            textures.Add(ObjectType.Boss, Content.Load<Texture2D>("Boss Enemy0"));
            //textures.Add(ObjectType.BossLeapZone, Content.Load<Texture2D>("ClearLeapZone"));
            textures.Add(ObjectType.Rock, Content.Load<Texture2D>("black_rock"));

            // load the sprite for the arrow
            arrowImage = Content.Load<Texture2D>("Arrow");

            //fonts
            testFont = Content.Load<SpriteFont>("testFont");
            testFont2 = Content.Load<SpriteFont>("testFont2");

            //list of player sprites
            playerTextures.Add(basePlayer);
            playerTextures.Add(Content.Load<Texture2D>("BasicPlayer45"));
            playerTextures.Add(Content.Load<Texture2D>("BasicPlayer90"));
            background = Content.Load<Texture2D>("BasicBackground");
            logo = Content.Load<Texture2D>("Jungle Escape Logo");

            //list of healthpoint bars
            healthTextures.Add(Content.Load<Texture2D>("HealthBarGreen"));
            healthTextures.Add(Content.Load<Texture2D>("HealthBarYel-Green"));
            healthTextures.Add(Content.Load<Texture2D>("HealthBarYellow"));
            healthTextures.Add(Content.Load<Texture2D>("HealthBarOrange"));
            healthTextures.Add(Content.Load<Texture2D>("HealthBarRed"));

            //load the map and initialize the camera player reference object
            levelMap.loadMap(textures);
            playerCamRef = new Player(new Rectangle(desiredBBWidth / 2, desiredBBHeight / 2, 0, 0), null, 0);

            // give the boss the player to follow
            foreach(Boss boss in levelMap.objectMap.OfType<Boss>())
            {
                boss.Player1 = levelMap.findPlayer();
            }

            //sound effects and music
            SoundEffect.MasterVolume = .5f;
            soundEffects.Add(Content.Load<SoundEffect>("bowFire1"));

            songs.Add(Content.Load<Song>("Game"));
            songs.Add(Content.Load<Song>("GameOver"));
            songs.Add(Content.Load<Song>("BossEpic"));
            songs.Add(Content.Load<Song>("menu1"));
            songs.Add(Content.Load<Song>("instinct"));
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
                playerCamRef = new Player(new Rectangle(desiredBBWidth/2, desiredBBHeight/2, 0, 0), null, 0);
            }

            switch (myState)
            {
                // Each gamestste checks for keypresses to navigate through a menu, and switch to the proper gamestate based on a current index
                case GameState.Menu:
                    if (gameMusicTimer == 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = .25f;
                        MediaPlayer.Play(songs[3]);
                        MediaPlayer.IsRepeating = true;
                    }
                    gameMusicTimer++;

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
                    {
                        Initialize();
                        myState = GameState.Game;
                    }
                    break;

                case GameState.Options:
                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        optionsIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        optionsIndex -= 1;

                    if (optionsIndex >= 5)
                        optionsIndex = 0;
                    else if (optionsIndex < 0)
                        optionsIndex = 4;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && optionsIndex == 0)
                    {
                        //change to 1024 x 768
                        desiredBBWidth = 1024;
                        desiredBBHeight = 768;
                        graphics.PreferredBackBufferWidth = desiredBBWidth;
                        graphics.PreferredBackBufferHeight = desiredBBHeight;
                        graphics.ApplyChanges();
                        myState = GameState.Menu;
                    }

                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && optionsIndex == 1)
                    {
                        //change to 1200 x 1024
                        desiredBBWidth = 1280;
                        desiredBBHeight = 1024;
                        graphics.PreferredBackBufferWidth = desiredBBWidth;
                        graphics.PreferredBackBufferHeight = desiredBBHeight;
                        graphics.ApplyChanges();
                        myState = GameState.Menu;
                    }
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && optionsIndex == 2)
                    {
                        //change to 1900 x 1080
                        desiredBBWidth = 1920;
                        desiredBBHeight = 1080;
                        graphics.PreferredBackBufferWidth = desiredBBWidth;
                        graphics.PreferredBackBufferHeight = desiredBBHeight;
                        graphics.ApplyChanges();
                        myState = GameState.Menu;
                    }

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && optionsIndex == 3)
                    {
                        //change back to the orginal
                        desiredBBWidth = 1600;
                        desiredBBHeight = 900;
                        graphics.PreferredBackBufferWidth = desiredBBWidth;
                        graphics.PreferredBackBufferHeight = desiredBBHeight;
                        graphics.ApplyChanges();
                        myState = GameState.Menu;
                    }

                    //go back to the main menu
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && optionsIndex == 4)
                        myState = GameState.Menu;
                    break;


                case GameState.Game:
                    //Play normal game theme
                    if (gameMusicTimer == 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = .25f;
                        MediaPlayer.Play(songs[0]);
                        MediaPlayer.IsRepeating = true;
                    }
                    gameMusicTimer++;

                    //If player crosses a leap zone, the boss battle has begun
                    foreach (BossLeapZone zone in levelMap.objectMap.OfType<BossLeapZone>())
                    {
                        if (playerCamRef.DetectCollision(zone))
                        {
                            BossBattleHasStarted = true;
                        }
                    }

                    //If the boss battle has started, plays the boss theme
                    if (BossBattleHasStarted)
                    {
                        if (BossBattleTimer == 0)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Volume = .5f;
                            MediaPlayer.Play(songs[4]);
                            MediaPlayer.IsRepeating = true;
                        }
                        BossBattleTimer++;
                    }


                        playerCamRef = levelMap.findPlayer();
                    foreach (Character chara in levelMap.objectMap.OfType<Character>())
                    {
                        
                        if (chara is Player)
                        {
                            Player player1 = (Player)chara;
                            player1.Move(levelMap.objectMap);

                            // populate the list of arrows with valid arrows to be drawn later
                            Arrow firedArrow = player1.FireArrow(arrowImage, levelMap.objectMap);

                            if (firedArrow != null)
                            {
                                arrows.Add(firedArrow);
                            }

                            foreach(Arrow arrow in arrows)
                            {
                                if (!arrow.alive)
                                {
                                    arrows.Remove(arrow);
                                    break;
                                }

                                // make the arrow move 
                                arrow.Move(levelMap.objectMap);
                            }
                        }
                        else
                            chara.Move();

                        aimState = Keyboard.GetState();

                    }

                    //check if any enemies have been killed and remove them
                    foreach(Enemy enemy in levelMap.objectMap.OfType<Enemy>())
                    {
                        if (!enemy.alive)
                        {
                            levelMap.objectMap.Remove(enemy);
                            break;
                        }
                    }

                    // let the player pause
                    if (SingleKeyPress(Keys.P, kbState, previousKbState) || SingleKeyPress(Keys.Escape, kbState, previousKbState))
                        myState = GameState.Pause;

                    // Change the gamestates based on the state of the map and check for player death
                    foreach (Character chara in levelMap.objectMap.OfType<Character>())
                    {
                        // populate the list of enemies with the current list of enemies in object map
                        if (chara is Enemy)
                            enemies.Add((Enemy)chara);

                        // create Player object
                        Player player1;
                        if (chara is Player)
                        {
                            player1 = (Player)chara;

                            if (player1.alive == false || player1.hitBox.Y >= backgroundHeight)
                            {
                                gameMusicTimer = 0;
                                myState = GameState.GameOver;
                            }   
                        }
                    }

                    // check if the new list of enemies is empty
                    if(enemies.Count == 0)
                    {
                        gameMusicTimer = 0;
                        myState = GameState.Victory;
                    }



                    // reset the enemy list to repopulate on the next update
                    enemies.Clear();

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

                    //play gameOver theme
                    if (gameMusicTimer == 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = .25f;
                        MediaPlayer.Play(songs[1]);
                        MediaPlayer.IsRepeating = true;
                    }
                    gameMusicTimer++;

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
                    {
                        gameMusicTimer = 0;
                        myState = GameState.Menu;
                    }
                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && gameOverIndex == 2)
                        Exit();

                    break;

                case GameState.Victory:
                    if (gameMusicTimer == 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = .5f;
                        MediaPlayer.Play(songs[2]);
                        MediaPlayer.IsRepeating = true;
                    }
                    gameMusicTimer++;

                    if (SingleKeyPress(Keys.Down, kbState, previousKbState))
                        victoryIndex += 1;
                    else if (SingleKeyPress(Keys.Up, kbState, previousKbState))
                        victoryIndex -= 1;

                    if (victoryIndex >= 2)
                        victoryIndex = 0;
                    else if (victoryIndex < 0)
                        victoryIndex = 1;

                    if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && victoryIndex == 0)
                        Initialize();

                    else if (SingleKeyPress(Keys.Enter, kbState, previousKbState) && victoryIndex == 1)
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,levelMap.cam.translation(playerCamRef));
            
            switch (myState)
            {
                case GameState.Menu:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    spriteBatch.Draw(logo, new Rectangle(0, 0, desiredBBWidth/6, desiredBBHeight/6), Color.White);
                    spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 160), Color.White);
                    spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 235), Color.White);
                    spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 310), Color.White);
                    spriteBatch.DrawString(testFont, "Options", new Vector2(20, 385), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 460), Color.White);

                    if (menuIndex == 0)
                        spriteBatch.DrawString(testFont, "Start Game", new Vector2(20, 160), Color.Yellow);
                    else if (menuIndex == 1)
                        spriteBatch.DrawString(testFont, "How to Play", new Vector2(20, 235), Color.Yellow);
                    else if (menuIndex == 2)
                        spriteBatch.DrawString(testFont, "How to Change Map", new Vector2(20, 310), Color.Yellow);
                    else if (menuIndex == 3)
                        spriteBatch.DrawString(testFont, "Options", new Vector2(20, 385), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 460), Color.Yellow);
                    break;

                case GameState.Options:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    spriteBatch.DrawString(testFont, "Set Your Screen Resolution ", new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(testFont, "1024 x 768", new Vector2(0, 100), Color.White);
                    spriteBatch.DrawString(testFont, "1280 x 1024", new Vector2(0, 150), Color.White);
                    spriteBatch.DrawString(testFont, "1920 x 1080", new Vector2(0, 200), Color.White);
                    spriteBatch.DrawString(testFont, "Original", new Vector2(0, 250), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Options Menu", new Vector2(0, 300), Color.White);

                    if (optionsIndex == 0)
                        spriteBatch.DrawString(testFont, "1024 x 768", new Vector2(0, 100), Color.Yellow);
                    else if (optionsIndex == 1)
                        spriteBatch.DrawString(testFont, "1280 x 1024", new Vector2(0, 150), Color.Yellow);
                    else if (optionsIndex == 2)
                        spriteBatch.DrawString(testFont, "1920 x 1080", new Vector2(0, 200), Color.Yellow);
                    else if (optionsIndex == 3)
                        spriteBatch.DrawString(testFont, "Original", new Vector2(0, 250), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Exit Options Menu", new Vector2(0, 300), Color.Yellow);

                    break;

                case GameState.Instructions:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    if (desiredBBHeight >= 1080)
                    {
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
                    }
                    else
                    {
                        spriteBatch.DrawString(testFont, "How To Play The Game:", new Vector2(0, 0), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Move and Shoot:", new Vector2(0, 100), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Press the W key to jump.", new Vector2(0, 150), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Press the A key to move left.", new Vector2(0, 200), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Press the D key to move right.", new Vector2(0, 250), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Combine the keys to move and jump at the same time in one direction.", new Vector2(0, 300), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Press the spacebar to shoot at an enemy spider.", new Vector2(0, 350), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "To Win:", new Vector2(0, 450), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Reach the end of the level without dying, there are no checkpoints.", new Vector2(0, 500), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Be prepared to fight the boss at the end of the game.", new Vector2(0, 550), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 600), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                    }
                    break;

                case GameState.Story:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    if (desiredBBHeight >= 1080)
                    {
                        spriteBatch.DrawString(testFont, "JungleScape Storyline:", new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(testFont, "The player was going to a professional archery competition in Rio.", new Vector2(0, 100), Color.White);
                        spriteBatch.DrawString(testFont, "Suddenly his plane crashes in the middle of wild, unexplored jungle.", new Vector2(0, 150), Color.White);
                        spriteBatch.DrawString(testFont, "The player has to survive huge bugs and wild animals.", new Vector2(0, 200), Color.White);
                        spriteBatch.DrawString(testFont, "All while solving puzzles to find his way out.", new Vector2(0, 250), Color.White);
                        spriteBatch.DrawString(testFont, "Creators:", new Vector2(0, 350), Color.White);
                        spriteBatch.DrawString(testFont, "Project Manager: Alexia Bernardo", new Vector2(0, 400), Color.White);
                        spriteBatch.DrawString(testFont, "Architect: Sean Hasse", new Vector2(0, 450), Color.White);
                        spriteBatch.DrawString(testFont, "Game Designer: Brady Murren", new Vector2(0, 500), Color.White);
                        spriteBatch.DrawString(testFont, "Interface Designer: Max Kaiser", new Vector2(0, 550), Color.White);
                        spriteBatch.DrawString(testFont, "hit ENTER to advance to game", new Vector2(0, 650), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(testFont, "JungleScape Storyline:", new Vector2(0, 0), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "The player was going to a professional archery competition in Rio.", new Vector2(0, 100), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Suddenly his plane crashes in the middle of wild, unexplored jungle.", new Vector2(0, 150), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "The player has to survive huge bugs and wild animals.", new Vector2(0, 200), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "All while solving puzzles to find his way out.", new Vector2(0, 250), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Creators:", new Vector2(0, 350), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Project Manager: Alexia Bernardo", new Vector2(0, 400), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Architect: Sean Hasse", new Vector2(0, 450), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Game Designer: Brady Murren", new Vector2(0, 500), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Interface Designer: Max Kaiser", new Vector2(0, 550), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "hit ENTER to advance to game", new Vector2(0, 650), Color.White, 0f, new Vector2(0, 0), .6f, SpriteEffects.None, 0f);
                    }
                    break;

                case GameState.Editor:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    if (desiredBBHeight >= 1080)
                    {
                        spriteBatch.DrawString(testFont, "How To Use The Map Editor:", new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(testFont, "Begin by starting a new instance of the editor to view the screen.", new Vector2(0, 100), Color.White);
                        spriteBatch.DrawString(testFont, "Place the game objects into the game by left clicking on your mouse.", new Vector2(0, 200), Color.White);
                        spriteBatch.DrawString(testFont, "Scroll through the options by clicking on the up and down keys.", new Vector2(0, 250), Color.White);
                        spriteBatch.DrawString(testFont, "Overwrite your mistake by clicking a different object into place.", new Vector2(0, 350), Color.White);
                        spriteBatch.DrawString(testFont, "The default option is 'nothing' and will delete anything you do not want.", new Vector2(0, 400), Color.White);
                        spriteBatch.DrawString(testFont, "Do keep in mind in order to play you must include a player.", new Vector2(0, 500), Color.White);
                        spriteBatch.DrawString(testFont, "When you are done press enter, and run the game. Enjoy!", new Vector2(0, 600), Color.White);
                        spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 700), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(testFont, "How To Use The Map Editor:", new Vector2(0, 0), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Begin by starting a new instance of the editor to view the screen.", new Vector2(0, 100), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Place the game objects into the game by left clicking on your mouse.", new Vector2(0, 200), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Scroll through the options by clicking on the up and down keys.", new Vector2(0, 250), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Overwrite your mistake by clicking a different object into place.", new Vector2(0, 350), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "The default option is 'nothing' and will delete anything you do not want.", new Vector2(0, 400), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "Do keep in mind in order to play you must include a player.", new Vector2(0, 500), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "When you are done press enter, and run the game. Enjoy!", new Vector2(0, 600), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(testFont, "hit 'Enter' to return", new Vector2(0, 700), Color.White, 0f, new Vector2(0, 0), .75f, SpriteEffects.None, 0f);
                    }
                    break;

                case GameState.Game:
                    spriteBatch.Draw(background, new Rectangle(0,0,backgroundWidth * 3 , backgroundHeight), Color.White);
                    levelMap.drawMap(spriteBatch, playerTextures, kbState);
                    //spriteBatch.DrawString(testFont, playerCamRef.healthPoints.ToString(), new Vector2(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 45), Color.Red);

                    //player health bars
                    switch (playerCamRef.healthPoints)
                    {
                        case 5:
                            spriteBatch.Draw(healthTextures[0], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100, 10), Color.White);
                            break;
                        case 4:
                            spriteBatch.Draw(healthTextures[1], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100, 10), Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(healthTextures[2], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100, 10), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(healthTextures[3], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100, 10), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(healthTextures[4], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100 , 10), Color.White);
                            break;
                        default:
                            spriteBatch.Draw(healthTextures[4], new Rectangle(playerCamRef.hitBox.X, playerCamRef.hitBox.Y - 20, 100, 10), Color.White);
                            break;
                    }

                    foreach (Enemy enemy in levelMap.objectMap.OfType<Enemy>())
                    {
                        //spriteBatch.DrawString(testFont, enemy.healthPoints.ToString(), new Vector2(enemy.hitBox.X, enemy.hitBox.Y - 45), Color.Red);
                        
                        //check to see if it's a boss
                        if(enemy is Boss)
                        {
                            switch (enemy.healthPoints)
                            {
                                case 10:
                                case 9:
                                    spriteBatch.Draw(healthTextures[0], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                                case 8:
                                case 7:
                                    spriteBatch.Draw(healthTextures[1], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                                case 6:
                                case 5:
                                    spriteBatch.Draw(healthTextures[2], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                                case 4:
                                case 3:
                                    spriteBatch.Draw(healthTextures[3], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                                case 2:
                                case 1:
                                    spriteBatch.Draw(healthTextures[4], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                                default:
                                    spriteBatch.Draw(healthTextures[4], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                    break;
                            }
                            continue;
                        }

                        //spider health bars
                        switch(enemy.healthPoints)
                        {
                            case 2:
                                spriteBatch.Draw(healthTextures[0], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                break;
                            case 1:
                                spriteBatch.Draw(healthTextures[4], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White); 
                                break;
                            default:
                                spriteBatch.Draw(healthTextures[4], new Rectangle(enemy.hitBox.X, enemy.hitBox.Y - 20, 100, 10), Color.White);
                                break;
                        }
                    }
                    
                    // draw the arrows inside of the list of arrows (the ones still valid)
                    foreach(Arrow arrow in arrows)
                    {
                        arrow.Draw(spriteBatch);
                    }
                    break;

                case GameState.Pause:
                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.Gray);
                    levelMap.drawMap(spriteBatch, playerTextures, kbState);
                    spriteBatch.DrawString(testFont2, "Paused", new Vector2((desiredBBWidth/2) - 50, (desiredBBHeight/2) - 400), Color.White);
                    spriteBatch.DrawString(testFont, "Resume Game", new Vector2((desiredBBWidth / 2) - 50, (desiredBBHeight / 2) - 250), Color.White);
                    spriteBatch.DrawString(testFont, "How do I Play Again?", new Vector2((desiredBBWidth / 2) - 120, (desiredBBHeight / 2) - 175), Color.White);
                    spriteBatch.DrawString(testFont, "Return to Menu", new Vector2((desiredBBWidth / 2) - 70, (desiredBBHeight / 2) - 100), Color.White);

                    if (pauseIndex == 0)
                        spriteBatch.DrawString(testFont, "Resume Game", new Vector2((desiredBBWidth / 2) - 50, (desiredBBHeight / 2) - 250), Color.Yellow);
                    else if (pauseIndex == 1)
                        spriteBatch.DrawString(testFont, "How do I Play Again?", new Vector2((desiredBBWidth / 2) - 120, (desiredBBHeight / 2) - 175), Color.Yellow);
                    else
                        spriteBatch.DrawString(testFont, "Return to Menu", new Vector2((desiredBBWidth / 2) - 70, (desiredBBHeight / 2) - 100), Color.Yellow);

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

                case GameState.Victory:

                    spriteBatch.Draw(background, new Rectangle(0, 0, desiredBBWidth, desiredBBHeight), Color.White);
                    spriteBatch.DrawString(testFont2, "Victory!", new Vector2(20, 10), Color.White);
                    spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(20, 150), Color.White);
                    spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 250), Color.White);

                    if (victoryIndex == 0)
                        spriteBatch.DrawString(testFont, "Back to Menu", new Vector2(20, 150), Color.Yellow);
                    else if (victoryIndex == 1)
                        spriteBatch.DrawString(testFont, "Exit Game", new Vector2(20, 250), Color.Yellow);

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
