﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleScape
{
    public class Character : GameObject
    {
        // attributes
        public bool alive;
        
        // constructor
        public Character(int x, int y) : base(x, y)
        {

        }

    }
}
