using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JungleScape
{
    public class Character : GameObject
    {
        // attributes
        protected bool alive;         // bool to determine if the character is still alive
        protected float speedX;
        protected float speedY
        
        // constructor
        public Character(Point loc, Rectangle hBox) : base(loc, hBox)      // takes in Location and HitBox for the new Character
        {
            alive = true;       // making a character automatically sets them as "alive"
            speedX = 0;
            speedY = 0;
        }

        // methods
        public void Move(int speedx, int speedy)      // basic movment. Change location based on input
        {
            location.X = location.X + speedx;
            location.Y = location.Y + speedy;
        }
    }
}       
