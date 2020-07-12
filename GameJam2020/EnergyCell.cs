using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam2020
{
    class EnergyCell : MovingSprite
    {
        public EnergyCell(int inX, int inY) : base(Sprites.EnergySpriteData, inX, inY, 1)
        {

        }
    }
}
