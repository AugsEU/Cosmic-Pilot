using System;
using System.Threading;
using System.IO;
using System.Windows.Input;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GameJam2020
{

    enum GameStage : short
    {
        TITLE = 0,
        PLAY = 1,
        GAMEOVER = 2,
        WIN = 3
    }

    public struct Pixel
    {
        public char Character;
        public ConsoleColor TextColour;

        public Pixel(char MyChar, ConsoleColor Colour)
        {
            Character = MyChar;
            TextColour = Colour;
        }

        public bool IsSame(Pixel Pix)
        {
            return (Pix.Character == Character) && (Pix.TextColour == TextColour);
        }
    }

    class Program
    {
        const int CON_WIDTH = 150;
        const int CON_HEIGHT = 30;
        const int CON_TYPE = 8;//Spacing left for user typing
        const int FRAME_INTERVAL = 600;

        static Random RNG;
        static WMPLib.WindowsMediaPlayer wplayer;



        static Pixel[,] WorldMap;

        static Pixel[,] CurrentScreen;
        static GameStage CurrentState;

        static int CurrentOffset;

        static int ChaosFactor;

        static ASCIISprite Ship;
        static List<MovingSprite> EnergyCells;
        static List<MovingSprite> Missiles;

        static void Main(string[] args)
        {
            wplayer = new WMPLib.WindowsMediaPlayer();
            TitleScreen();
        }

        private static void GameLoop()
        {
            while (CurrentState == GameStage.PLAY)
            {
                //Input
                ConsoleKeyInfo CurrentKey = GetConsoleKey();
                switch (CurrentKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        Ship.Y--;
                        break;
                    case ConsoleKey.DownArrow:
                        Ship.Y++;
                        break;
                }

                //Physics
                //Random movement
                int HeightAboveCentre = Ship.Y + 2 - CON_HEIGHT / 2;
                int DistFromCentre = (int)Math.Round((decimal)(Math.Abs(Ship.Y + 2 - CON_HEIGHT / 2) / 4)) + 2;

                int RandomMovement = RNG.Next(DistFromCentre) - 1;
                if (RandomMovement >= 1) RandomMovement = 1;

                if (RNG.Next(Math.Max(ChaosFactor,1)) != 0)
                {
                    if (Ship.Y >= HeightAboveCentre)
                    {
                        Ship.Y += RandomMovement;

                    }
                    else
                    {
                        Ship.Y -= RandomMovement;
                    }
                }

                //Update objects
                SpawnSprites();
                UpdateAllSprites();

                //Draw new frame
                CurrentOffset++;
                LoadWorldMapOffset(CurrentOffset);
                DrawScreen(CurrentScreen);
                DrawHUD();
                CaptainCommands();

                CheckGameEnd();


                System.Threading.Thread.Sleep(FRAME_INTERVAL);
            }

            wplayer.controls.stop();

            switch (CurrentState)
            {
                case GameStage.GAMEOVER:
                    EndMainGame();
                    break;
                case GameStage.WIN:
                    WinGame();
                    break;
            }
            
        }

        private static void CaptainCommands()
        {
            if (CurrentOffset < 7)
            {
                Console.WriteLine("[Haddock] Captain Haddock here... do you read me, pilot?\n OVER");
                Ship.Y = 14;
            }
            else if (CurrentOffset < 24)
            {
                Console.WriteLine("[Haddock] This old banger barely survived that attack, but now we've got bigger fish to fry! We're headed straight towards an asteroid field.\n If you don't manage to gain control of the ship, we're all gonna die. \n Now listen, let me hand you over to the cheif engineer Haggard.\n OVER");
                Ship.Y = 14;
            }
            else if (CurrentOffset < 36)
            {
                Console.WriteLine("[Haggard] OH LAWD ITS COMING. THE ENGINES ARE SCUFFED. OH LAWD OH LAWD.\n You are supposed to use UP ARROW and DOWN ARROW to move but I DON'T KNOW IF THIS IS GONNA WORK!?! ITS COMING BOY.");
            }
            else if (CurrentOffset < 40)
            {
                Console.WriteLine("[Haggrid] Harry, you must listen. The only way out is to hit the top of the screen. Harry ple...a...s.e...");
            }
            else if (CurrentOffset < 50)
            {
                Console.WriteLine("[Haggard] DO NOT LISTEN TO HAGGRID. HE IS AN EVIL MAN!!!");
                Console.WriteLine("[Haddock] Uhh.. Yes... I believe he is intercepting our communications.\n OVER");
            }
            else if (CurrentOffset < 70)
            {
                Console.WriteLine("[Haddock] Rocks are up ahead. Dodge the red Xs.\n OVER");
                Console.WriteLine("[Haggard] OH LAWD THE ROCKS ARE COMING.");
            }
            else if (CurrentOffset < 100)
            {
                if (Ship.Y > 12)
                {
                    Console.WriteLine("[Haddock] Cadet, are you sure it's a good idea to try and go between the rocks?\n OVER");
                }
                else
                {
                    Console.WriteLine("[Haddock] Yes, I think going above the rocks is a good idea.\n OVER");
                }
            }
            else if (CurrentOffset < 120)
            {
                Console.WriteLine("[Haggrid] This is your captain speaking, I have some new information regarding the rocks.\n We believe that hitting one could help stabilise the sh..ip...");
            }
            else if (CurrentOffset < 140)
            {
                Console.WriteLine("[Haddock] Cadet, radiation is up ahead. I think Haggard will explain more.\n OVER");
                Console.WriteLine("[Haggard] Yes SIR. The radiation is harmless to us, UNLIKE THE DEADLY ROCKS. But it migh? mess wi?h our?communica?ions.");
                ChaosFactor = 10;
            }
            else if (CurrentOffset < 180)
            {
                Console.WriteLine("[Hagg??d] OH Y?S ?????? PLEAS? COLLEC? T?E MO?VIN? PA?TS \n OVER");
                Console.WriteLine("[Hagg??d] OH L??D M?SS?LES ARE COMI??. Dodg? t?e mis?iles.");
            }
            else if (CurrentOffset < 200)
            {
                Console.WriteLine("[Hadd???] We h?ve ne? info. T?e stationary capule? are en?rgy c?lls. If collect them we can stabilise th?s shi? and get out of here. \n OVER");
            }
            else if (CurrentOffset < 225)
            {
                Console.WriteLine("[???dock] We ne?d th?se energy cells if we have any hope of getting out of here. \n OVER");
            }
            else if (CurrentOffset < 240)
            {
                Console.WriteLine("[???????] ?h??an??w????e ???lly??? ??e thi?? o??i? h??e.\n ???R");
            }
            else if (CurrentOffset < 275)
            {
                Console.WriteLine("[???dock] Haggard, I see a strange wall in the distance. Please report. \n OVER");
                Console.WriteLine("[Haggard] I DON'T KNOW MAN! Looks like some kind of quantum rift. ");
            }
            else if (CurrentOffset < 290)
            {
                Console.WriteLine("[Haddock] Haggard! Please report. What's going on? \n OVER");
                Console.WriteLine("[Haggard] Captain! It looks like a ship from an alternate timeline has merged into our world.");
            }
            else if (CurrentOffset < 310)
            {
                Console.WriteLine("[Haddock] Haggard! Uhh... English please? \n OVER");
                Console.WriteLine("[Haggard] HAHAHAHAHA VERY GOOD JOKE CAPTAIN.\n It means that if either ship gets harmed we ALL DIE!");
                Console.WriteLine("[Haggrid] I think it means that the we can also hit the missiles safely.");
            }
            else if (CurrentOffset < 325)
            {
                Console.WriteLine("[Haggard] You know, if we get our engine status to 30/30 we might just be able to get out of here.");
                Console.WriteLine("[Haddock] Cadet, make it so! \n OVER");
            }
            else if (CurrentOffset < 340)
            {
                Console.WriteLine("[Haddock] Watch out! Missile incoming below! \n OVER");
            }
            else if (CurrentOffset < 360)
            {
                Console.WriteLine("[Haddock] We're getting reports that the other ship's captain is one hansome devil. Please confirm. \n OVER");
                Console.WriteLine("[Haddock] Affirmative. We have been getting similar reports on our side. \n OVER");
            }
            else if (CurrentOffset < 370)
            {
                Console.WriteLine("[Haggrid] I wish there was a second me...");
            }
            else if (CurrentOffset < 400)
            {
                Console.WriteLine("[Haggard] Remember cadet, once engines are up to 30/30 we can get out of here.");
            }
            else if (CurrentOffset < 430)
            {
                Console.WriteLine("[Haddock] This is a tight fit, but I know you can do it cadet! \n OVER");
            }
            else if (CurrentOffset < 490)
            {
                Console.WriteLine("[Haddock] Cadet, this is our last chance! We are about to hit the asteroid field! \n OVER");
                Console.WriteLine("[Haggard] OH LAWDY LAWD!");
            }
            else if (CurrentOffset < 500)
            {
                Console.WriteLine("[Haddock] This is it... ");
                Console.WriteLine("[Haggard] !!!!!!!!!!!!!!!!!!!!!!");
            }
        }

        private static void UpdateAllSprites()
        {
            for(int i = 0; i < Missiles.Count();i++)
            {
                Missiles[i].Update();

                if (Missiles[i].X + Missiles[i].Width < 0)
                {
                    Missiles.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            for (int i = 0; i < EnergyCells.Count(); i++)
            {
                EnergyCells[i].Update();
                if(Ship.IsOnSprite(EnergyCells[i].X, EnergyCells[i].Y))
                {
                    ChaosFactor -= 2;
                    EnergyCells.RemoveAt(i);
                    i--;
                    continue;
                }
                if (EnergyCells[i].X + EnergyCells[i].Width < 0)
                {
                    EnergyCells.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        private static void SpawnSprites()
        {
            switch(CurrentOffset)
            {
                case 90:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 24));
                    break;
                case 110:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 6));
                    break;
                case 125:
                    Missiles.Add(new Missile(CON_WIDTH, 15));
                    break;
                case 155:
                    Missiles.Add(new Missile(CON_WIDTH, 23));
                    break;
                case 185:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 22));
                    break;
                case 190:
                    Missiles.Add(new Missile(CON_WIDTH, 15));
                    break;
                case 240:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 10));
                    break;
                case 250:
                    Missiles.Add(new Missile(CON_WIDTH, 3));
                    break;
                case 273:
                    Ship.GiveNewSprite(Sprites.DualShipSpriteData);//ship split
                    Ship.Y = 5;
                    break;
                case 288:
                    Missiles.Add(new Missile(CON_WIDTH, 23));
                    break;
                case 320:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 7));
                    break;
                case 325:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 24));
                    break;
                case 361:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 12));
                    break;
                case 367:
                    EnergyCells.Add(new EnergyCell(CON_WIDTH, 15));
                    break;
                case 370:
                    Missiles.Add(new Missile(CON_WIDTH, 15));
                    break;

            }
        }

        private static void CheckGameEnd()
        {
            //Check game end
            if (Ship.Y < -1 || Ship.Y > CON_HEIGHT - Ship.Height + 1) CurrentState = GameStage.GAMEOVER;//Went off screen
            if (Ship.IsCollidingWith(CurrentScreen, 'X')) CurrentState = GameStage.GAMEOVER;//hit a rock

            foreach (Missile Miss in Missiles)
            {
                if (Ship.IsOnSprite(Miss.X, Miss.Y))
                {
                    CurrentState = GameStage.GAMEOVER;
                }
            }

            if (ChaosFactor <= 0) CurrentState = GameStage.WIN;
        }

        static void TitleScreen()
        {
            CurrentState = GameStage.TITLE;
            InitScreen();
            DrawTitleScreen();
            Console.ReadLine();
            StartMainGame();
        }

        static void StartMainGame()
        {
            CurrentState = GameStage.PLAY;

            wplayer.URL = "FiveFour.wav";
            wplayer.controls.play();

            RNG = new Random();

            CurrentScreen = new Pixel[CON_WIDTH, CON_HEIGHT];
            CurrentOffset = 0;

            LoadWorld();
            LoadWorldMapOffset(CurrentOffset);

            ChaosFactor = 20;//High factor is bad

            Ship = new ASCIISprite(Sprites.ShipSpriteData);
            Ship.X = 35;
            Ship.Y = 14;

            EnergyCells = new List<MovingSprite>();
            Missiles = new List<MovingSprite>();

            GameLoop();
        }

        static void EndMainGame()
        {
            InitScreen();
            DrawGameOver();
            wplayer.URL = "Fail.wav";
            wplayer.controls.play();
            System.Threading.Thread.Sleep(6000);
            wplayer.controls.stop();
            TitleScreen();
        }

        static void WinGame()
        {
            InitScreen();
            DrawWinScreen();
            wplayer.URL = "Win.wav";
            wplayer.controls.play();
            System.Threading.Thread.Sleep(10000);
            wplayer.controls.stop();
            TitleScreen();
        }

        private static ConsoleKeyInfo GetConsoleKey()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            while (Console.KeyAvailable)
            {
                cki = Console.ReadKey();
            }

            return cki;
        }

        static void LoadWorldMapOffset(int OffSet)
        {
            for(int x = 0; x < CON_WIDTH; x++)
            {
                for(int y = 0; y < CON_HEIGHT; y++)
                {
                    Pixel MyPixel;

                    if ((x + OffSet) < WorldMap.GetLength(0) && y < WorldMap.GetLength(1))
                        MyPixel = WorldMap[x + OffSet, y];
                    else
                        MyPixel = new Pixel('X', ConsoleColor.Red);

                    CurrentScreen[x, y] = MyPixel; 
                }
            }
        }

        private static void LoadWorld()
        {
            string MyDirectory = Directory.GetCurrentDirectory();
            string[] FileLines = File.ReadAllLines(MyDirectory + "\\World.txt");

            int WorldWidth = FileLines[0].Length;
            int WorldHeight = FileLines.Length;
            WorldMap = new Pixel[WorldWidth, WorldHeight];

            for (int y = 0; y < FileLines.Length;y++)
            {
                for(int x = 0; x < FileLines[y].Length;x++)
                {
                    char PixelChar = (char)FileLines[y][x];

                    ConsoleColor SpecialColor = ConsoleColor.White;
                    switch(PixelChar)
                    {
                        case 'X':
                            SpecialColor = ConsoleColor.Red;//Rock fragments
                            break;
                        case '~':
                            SpecialColor = ConsoleColor.Cyan;
                            break;
                        case '<':
                            SpecialColor = ConsoleColor.Yellow;
                            break;
                        case '>':
                            SpecialColor = ConsoleColor.Blue;
                            break;
                    }
                    WorldMap[x, y] = new Pixel(PixelChar, SpecialColor);
                }
            }
        }

        private static void DrawHUD()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Engine status: " + (10 + (20 - ChaosFactor)).ToString() + "/30" );
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void DrawScreen(Pixel[,] Screen)
        {
            InitScreen();

            for (int y = 0; y < Screen.GetLength(1); y++)
            {
                string Line = "";
                ConsoleColor PrevColour = ConsoleColor.White;
                for (int x = 0; x < Screen.GetLength(0); x++)
                {
                    Pixel SpritePixel = DrawAllSprites(x, y);
                    Pixel PixelToDraw = Screen[x, y];
                    if ((byte)SpritePixel.Character != 0x00) PixelToDraw = SpritePixel;

                    if (PixelToDraw.TextColour == PrevColour)
                    {
                        Line = Line + (PixelToDraw.Character);
                    }
                    else
                    {
                        Console.ForegroundColor = PrevColour;
                        Console.Write(Line);
                        Line = PixelToDraw.Character.ToString();
                        PrevColour = PixelToDraw.TextColour;
                    }
                }
                Console.ForegroundColor = PrevColour;
                Console.Write(Line);
                Console.WriteLine();
            }

            //Draw divider
            Console.ForegroundColor = ConsoleColor.White;
            string Divider = "";

            for (int x = 0; x < Screen.GetLength(0); x++)
            {
                Divider = Divider + "=";
            }
            Console.WriteLine(Divider);

        }

        static Pixel DrawAllSprites(int x, int y)
        {
            if(Ship.IsOnSprite(x,y))
            {
                return Ship.GetPixel(x, y);
            }
            
            foreach(Missile Miss in Missiles)
            {
                if(Miss.IsOnSprite(x,y))
                {
                    return Miss.GetPixel(x, y);
                }
            }

            foreach (EnergyCell Energy in EnergyCells)
            {
                if (Energy.IsOnSprite(x, y))
                {
                    return Energy.GetPixel(x, y);
                }
            }

            return new Pixel((char)0x00, ConsoleColor.Black);
        }

        private static void InitScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.SetWindowSize(CON_WIDTH+1, CON_HEIGHT + CON_TYPE);
            Console.SetBufferSize(CON_WIDTH+1, CON_HEIGHT + CON_TYPE);
        }

        static void DrawGameOver()
        {
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                   /$$$$$$         /$$$$$$        /$$      /$$       /$$$$$$$$                                                      ");
            Console.WriteLine("                                  /$$__  $$       /$$__  $$      | $$$    /$$$      | $$_____/                                                      ");
            Console.WriteLine("                                 | $$  \\__/      | $$  \\ $$      | $$$$  /$$$$      | $$                                                          ");
            Console.WriteLine("                                 | $$ /$$$$      | $$$$$$$$      | $$ $$/$$ $$      | $$$$$                                                         ");
            Console.WriteLine("                                 | $$|_  $$      | $$__  $$      | $$  $$$| $$      | $$__/                                                         ");
            Console.WriteLine("                                 | $$  \\ $$      | $$  | $$      | $$\\  $ | $$      | $$                                                          ");
            Console.WriteLine("                                 |  $$$$$$/      | $$  | $$      | $$ \\/  | $$      | $$$$$$$$                                                     ");
            Console.WriteLine("                                  \\______/       |__/  |__/      |__/     |__/      |________/                                                     ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                         /$$$$$$        /$$    /$$       /$$$$$$$$       /$$$$$$$                                                   ");
            Console.WriteLine("                                        /$$__  $$      | $$   | $$      | $$_____/      | $$__  $$                                                  ");
            Console.WriteLine("                                       | $$  \\ $$      | $$   | $$      | $$            | $$  \\ $$                                                ");
            Console.WriteLine("                                       | $$  | $$      |  $$ / $$/      | $$$$$         | $$$$$$$/                                                  ");
            Console.WriteLine("                                       | $$  | $$       \\  $$ $$/       | $$__/         | $$__  $$                                                 ");
            Console.WriteLine("                                       | $$  | $$        \\  $$$/        | $$            | $$  \\ $$                                                ");
            Console.WriteLine("                                       |  $$$$$$/         \\  $/         | $$$$$$$$      | $$  | $$                                                 ");
            Console.WriteLine("                                        \\______/           \\_/          |________/      |__/  |__/                                                ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                PLEASE WAIT....                                     ");
            Console.WriteLine("                                                                                                                                                    ");
            Console.WriteLine("                                                                                                                                                    ");
        }

        static void DrawTitleScreen()
        {
            Console.WriteLine("                                                 ____                                                                                                 ");
            Console.WriteLine("                                                /\\  _`\\                               __                                                            ");
            Console.WriteLine("                                                \\ \\ \\/\\_\\    ___     ____    ___ ___ /\\_\\    ___                                               ");
            Console.WriteLine("                                                 \\ \\ \\/_/_  / __`\\  /',__\\ /' __` __`\\/\\ \\  /'___\\                                           ");
            Console.WriteLine("                                                  \\ \\ \\_\\ \\/\\ \\_\\ \\/\\__, `\\/\\ \\/\\ \\/\\ \\ \\ \\/\\ \\__/                               ");
            Console.WriteLine("                                                   \\ \\____/\\ \\____/\\/\\____/\\ \\_\\ \\_\\ \\_\\ \\_\\ \\____\\                                  ");
            Console.WriteLine("                                                    \\/___/  \\/___/  \\/___/  \\/_/\\/_/\\/_/\\/_/\\/____/                                           ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                 ____         ___           __                                                                        ");
            Console.WriteLine("                                                /\\  _`\\   __ /\\_ \\         /\\ \\__                                                               ");
            Console.WriteLine("                                                \\ \\ \\_\\ \\/\\_\\\\//\\ \\     ___\\ \\ ,_\\                                                       ");
            Console.WriteLine("                                                 \\ \\ ,__/\\/\\ \\ \\ \\ \\   / __`\\ \\ \\/                                                         ");
            Console.WriteLine("                                                  \\ \\ \\/  \\ \\ \\ \\_\\ \\_/\\ \\_\\ \\ \\ \\_                                                    ");
            Console.WriteLine("                                                   \\ \\_\\   \\ \\_\\/\\____\\ \\____/\\ \\__\\                                                      ");
            Console.WriteLine("                                                    \\/_/    \\/_/\\/____/\\/___/  \\/__/                                                             ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("        After a tense space battle, your ship has barely survived.                                                                                    ");
            Console.WriteLine("        You are Harry Potter, a space cadet who is fresh out of the academy.                                                                          ");
            Console.WriteLine("        The ship's pilot died in the attack, you must take their place.                                                                               ");
            Console.WriteLine("        Can you maneuver the ship to safety?                                                                                                          ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("        Pay close attention to what you read on the radio comms.                              PRESS ENTER TO PLAY!                                    ");
            Console.WriteLine("                                                                                              PRESS ENTER TO PLAY!                                    ");
            Console.WriteLine("                                                                                              PRESS ENTER TO PLAY!                                    ");
            Console.WriteLine("                                                                                              PRESS ENTER TO PLAY!                                    ");
        }

        static void DrawWinScreen()
        {
            Console.WriteLine("                    'lOOlOOll.                              'llllllllllllOOl.                                    'lllllllll'                          ");
            Console.WriteLine("                .,llll,. .,,lll,                          .ll.           .lxkl.                                .lkl,,,,,,,ll,ll.                      ");
            Console.WriteLine("               ,kl,..,,ll,l,  ,kl.                      .lKMx,,,ll,,,,ll,,,,lkkl.                           .ll,,.           .lk,                     ");
            Console.WriteLine("              ,kl,lllKKk, ..   .lk,                    ,kKKl.   .,,,,,,.  .,,,lKk.                         ,kl.                .ll.                   ");
            Console.WriteLine("              lKkl,. .,lk, .,,,. ,k,                  ,klkl                   lKkk,                      .,l,            .,ll,,,kMKl.                 ");
            Console.WriteLine("              lK,       .,,ll,llllkl                  lllk.                   .kl,k,                    ,k, .,ll,,,,,,,,,ll,.   .kkkk.                ");
            Console.WriteLine("             .l,                .kk.                  llll                     ll ll                   .l, ,kl,.                 ll.kl                ");
            Console.WriteLine("             ll   .,,,.    .,.   ll                   llll  ...,,.     .,,.    ll ll                   lk. ll                    ,k,.l,               ");
            Console.WriteLine("            ,k,   .,,.     .,,.  lKl,l,              .lkk,  .,,,.      .,,.    ll.kl                   .kl,k,                     ll ll               ");
            Console.WriteLine("            lk.                  lklklll.            ,klkl          ..         lklKl                    ,lkl    .,,,,,.  .,,,,.  ,k,.kl               ");
            Console.WriteLine("          .llkk.         .l,     ll,k,.kk.            lkl,          ll         ll ll                     lk.    .,,,,,.  .,,,,.  ,kkKMl               ");
            Console.WriteLine("         .kk..kl       ,,ll.     lKk,  .kl            .ll.        .,ll,.       ,, lk,.                   lk.                     .kkkk.               ");
            Console.WriteLine("        .kk.  ,k,               ,kk,    lk.         .ll,lk,                   ,kllklll.                  .kk.        ..## .  .   lk.ll                ");
            Console.WriteLine("        lk.    ,k,   .,,,,,   .lKl,x,   .kl         lk.  .ll.    .,,,,,.     .kl.,.  ,k,                  .kl    .     ##,    ,  lklk,                ");
            Console.WriteLine("        ll      lk,.        .lklkl .ll.  lk.       ,k,   ,lkk.              .kkl,     lk.                  .l,  .l            .  lk,.                 ");
            Console.WriteLine("        ll      ll,l,,,.  .lkl. ll  .lkl,lKl       ll    ll.lkl,.      .,,lkkl.ll     .kl                   ,kl.   .,lkkkl,,.   .kl                   ");
            Console.WriteLine("        ll      ll  .,ll,lKk.  .kl    .,,,kl       ll    ll  .,ll,,,,,,ll,,,.  ll      ll                    .lkl.             .kk.                   ");
            Console.WriteLine("        ll      ll      'OO.   l0'        ''       ll    ll                    ll      ll                      '0l           'lO0'                    ");
            Console.WriteLine("         [CADET Harry Potter]                           [ENGINEER John Haggard]                            [CAPTAIN Ted Haddock]                     ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                               YOU WIN!                                                                               ");
            Console.WriteLine("                                                               YOU WIN!                                                                               ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("       The ship turns at the last moment and everyone is safe.                                                                                        ");
            Console.WriteLine("                                                                                                                                                      ");
            Console.WriteLine("            Congratulations!                                                                                                                          ");
            Console.WriteLine("                                                                                                    Please wait....                                   ");
            Console.WriteLine("                                                                                                                                                      ");
        }
    }
}
