using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam2020
{
    class MovingSprite : ASCIISprite
    {
        int Speed;
        public MovingSprite(Pixel[][] _SpriteData, int inX, int inY, int _Speed) : base(_SpriteData)
        {
            SpriteData = _SpriteData;
            X = inX;
            Y = inY;
            Speed = _Speed;
        }

        public void Update()
        {
            X -= Speed;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MovingSprite sprite = (MovingSprite)obj;
                return (X == sprite.X) && (Y == sprite.Y);
            }
        }
    }
}
