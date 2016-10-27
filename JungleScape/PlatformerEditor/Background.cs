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
        //attributes
        private Vector2 screenPos, origin, textureSize;
        private Texture2D mytexture;
        private int screenHeight;
        private int screenWidth;  
        
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytexture = backgroundTexture;
            screenHeight = 1920;
            screenWidth = 1080;
            //set the orgin that we are drawing from

            //the top edge
            origin = new Vector2(0, 0);

            // Set the screen position to the center of the screen.
            screenPos = new Vector2(screenWidth, screenHeight);
            
            //offset to draw the second texture
            textureSize = new Vector2(0, mytexture.Width);

        }

        public void Update( float deltaX)
        {
            //The X value is kept no larger than the texture width
            screenPos.X += deltaX;
            screenPos.X = screenPos.X % mytexture.Width;

        }

        public void Draw( SpriteBatch batch)
        {
            //Draw the texture if it's still on the screen
            if (screenPos.X < screenWidth)
            {
                batch.Draw(mytexture, screenPos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }

            //Draw the texture a second time behind the first one

            //Create the scrolling illusion
            //subtracts the texture width from the screen position using the texturesize vector to create the illusion of a loop
            batch.Draw(mytexture, screenPos - textureSize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}

    