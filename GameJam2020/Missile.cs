using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam2020
{
    class Missile : MovingSprite
    {
        public Missile(int inX, int inY) : base(Sprites.MissileSpriteData, inX, inY, 2)
        {

        }
    }
}
