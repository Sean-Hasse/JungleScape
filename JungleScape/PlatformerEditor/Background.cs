//Code from http://www.jgallant.com/how-to-create-a-parallax-effect-in-monogame/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
namespace PlatformerEditor
{
    class Background
    {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Zoom;              //Zoom level of our image

        private Viewport Viewport;      //Our game viewport

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle
        {
            get { return new Rectangle((int)(Offset.X), (int)(Offset.Y), (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom));}
        }

        public Background(Texture2D texture, Vector2 speed, float zoom)
        {
            Texture = texture;
            Offset = Vector2.Zero;
            Speed = speed;
            Zoom = zoom;
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            //Update our offset
            Offset += distance;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Y), Rectangle, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }
}

    