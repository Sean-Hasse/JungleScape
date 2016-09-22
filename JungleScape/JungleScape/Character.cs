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
        public bool alive;      // bool to determine if the character is still alive
        Point location;
        
        // constructor
        public Character(Point loc, Rectangle hBox) : base(loc, hBox)      // takes in Location and HitBox for the new Character
        {
            alive = true;       // making a character automatically sets them as "alive"
            location = loc;
        }

        // methods
        public void Move(int speed)      // basic movment. Change location based on input
        {
             
        }
    }
}       
