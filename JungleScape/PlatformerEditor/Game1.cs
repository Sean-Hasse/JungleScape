using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        Texture2D brick;
        int currentX;
        int lvlX;
        List<Tile> platforms;
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        MouseState previousMState;
        const int GRID_SIZE = 50;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
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
            currentX = 0;
            lvlX = 0;
            platforms = new List<Tile>();
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
            brick = Content.Load<Texture2D>("PlatformerBrick");
            // TODO: use this.Content to load your game content here
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

            if (SingleMouseClick())
            {
                platforms.Add(new Tile(new Rectangle(getGridCoord(Mouse.GetState().X, Mouse.GetState().Y), new Point(GRID_SIZE, GRID_SIZE)), brick));
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
            spriteBatch.Draw(background, new Rectangle(-currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(background, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            spriteBatch.Draw(background, new Rectangle(2 * GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(brick, new Rectangle(getGridCoord(Mouse.GetState().X, Mouse.GetState().Y), new Point(GRID_SIZE, GRID_SIZE)), Color.White);
            foreach (var item in platforms)
            {
                spriteBatch.Draw(item.texture, item.bounds, Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool SingleKeyPress(Keys key)
        {
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }
            else
                return false;
        }

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
        public Point getGridCoord(int givenX, int givenY)
        {
            int x = (int)Math.Round((double)(givenX / GRID_SIZE)) * GRID_SIZE;
            int y = (int)Math.Round((double)(givenY / GRID_SIZE)) * GRID_SIZE;
            if (x > givenX)
                x -= GRID_SIZE;
            if (y > givenY)
                y -= GRID_SIZE;

            return new Point(x, y);
        }
    }
}
