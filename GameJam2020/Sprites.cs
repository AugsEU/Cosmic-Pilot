using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam2020
{
    class Sprites
    {
        public static Pixel[][] ShipSpriteData = {
            new Pixel[] { new Pixel('_', ConsoleColor.Green) },
            new Pixel[] { new Pixel('O', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('<', ConsoleColor.Magenta),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('/', ConsoleColor.Green) },
            new Pixel[] { new Pixel(' ', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green), new Pixel('|', ConsoleColor.Green) }
        };

        public static Pixel[][] DualShipSpriteData = {
            new Pixel[] { new Pixel('_', ConsoleColor.Green) },
            new Pixel[] { new Pixel('O', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('<', ConsoleColor.Magenta),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('/', ConsoleColor.Green) },
            new Pixel[] { new Pixel(' ', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green), new Pixel('|', ConsoleColor.Green) },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { },
            new Pixel[] { new Pixel('_', ConsoleColor.Green) },
            new Pixel[] { new Pixel('O', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('O', ConsoleColor.Cyan), new Pixel('#', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green) },
            new Pixel[] { new Pixel('<', ConsoleColor.Magenta),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green),new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('#', ConsoleColor.Green), new Pixel('/', ConsoleColor.Green) },
            new Pixel[] { new Pixel(' ', ConsoleColor.Green), new Pixel('\\', ConsoleColor.Green), new Pixel('|', ConsoleColor.Green) }
        };

        public static Pixel[][] EnergySpriteData =
        {
            new Pixel[] { new Pixel('<', ConsoleColor.DarkMagenta), new Pixel('=', ConsoleColor.Magenta), new Pixel('=', ConsoleColor.Magenta), new Pixel('>', ConsoleColor.DarkMagenta) }
        };

        public static Pixel[][] MissileSpriteData =
        {
            new Pixel[] { new Pixel('<', ConsoleColor.DarkMagenta), new Pixel('=', ConsoleColor.Magenta), new Pixel('=', ConsoleColor.Magenta), new Pixel('<', ConsoleColor.Magenta) }
        };
    }
}

