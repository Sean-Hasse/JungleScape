using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JungleScape
{
    public abstract class Character : GameObject
    {
        // attributes
        protected bool alive;         // bool to determine if the character is still alive
        protected int speedX;
        protected int speedY;
        
        // constructor
        public Character(Rectangle hBox, Texture2D texture) : base(hBox, texture)      // takes in Location and HitBox for the new Character
        {
            alive = true;       // making a character automatically sets them as "alive"
            speedX = 0;
            speedY = 0;
        }

        // methods
        public abstract void Move();
        public abstract void Move(List<GameObject> gObj);

        // takeDamage, when called, sets alive to false for passed in character
        public void TakeDamage(Character char1)
        {
            char1.alive = false;
        }
    }
}       
