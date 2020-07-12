using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam2020
{
    class ASCIISprite
    {
        public Pixel[][] SpriteData;
        public int X;
        public int Y;

        public int Width;
        public int Height;

        public ASCIISprite(Pixel[][] _SpriteData)
        {
            SpriteData = _SpriteData;
            X = Y = 0;
            Height = SpriteData.Length;
            Width = 0;
            for(int i = 0; i < SpriteData.Length; i++)
            {
                if(SpriteData[i].Length > Width)
                {
                    Width = SpriteData[i].Length;
                }
            }
        }

        public bool IsOnSprite(int inX, int inY)
        {
            //check if it's in outside the bounding box
            if (inY < Y || inY >= Y + Height) return false;
            if (inX < X || inX >= X + Width) return false;

            //if it isn't then get indices
            int YIdx = inY - Y;
            int XIdx = inX - X;

            if (SpriteData[YIdx].Length > XIdx) return true;

            return false;
        }

        public bool IsCollidingWith(Pixel[,] Screen, char PixelType)//checks if a given pixel type is colliding with the ship
        {
            int ScreenWidth = Screen.GetLength(0) - 1;
            int ScreenHeight = Screen.GetLength(1) - 1;
            for (int Sy = 0; Sy< SpriteData.Length;Sy++)
            {
                for(int Sx = 0; Sx < SpriteData[Sy].Length; Sx++)
                {
                    int ScreenX = Math.Min(X + Sx, ScreenWidth);
                    int ScreenY = Math.Min(Y + Sy, ScreenHeight);
                    if (Screen[ScreenX,ScreenY].Character == PixelType) return true;
                }
            }
            return false;
        }

        public Pixel GetPixel(int inX, int inY)
        {
            int YIdx = inY - Y;
            int XIdx = inX - X;

            return SpriteData[YIdx][XIdx];
        }

        public void GiveNewSprite(Pixel[][] NewSprite)
        {
            SpriteData = NewSprite;
            Height = SpriteData.Length;
            Width = 0;
            for (int i = 0; i < SpriteData.Length; i++)
            {
                if (SpriteData[i].Length > Width)
                {
                    Width = SpriteData[i].Length;
                }
            }
        }
    }
}


