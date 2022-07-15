using System;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFML_Test
{
    class Global
    {
        public static VideoMode VM = VideoMode.FullscreenModes[0];

        public static Vector2f SpawnPoint;
        public static Entity MyEntity;

        public static string[] Map = {
            "          00000000000000000000000000000000000000000000000000000000000000000000000000          ",
            "          000                                                                    000          ",
            "          000                                                                    000          ",
            "          000                                                                    000          ",
            "          000                                                                    000          ",
            "          000                                                                    000          ",
            "G         000                                                     0000000000W   W000          ",
            "DG        000                  WWWWWWWW                 WWWWWWWW               WWW00          ",
            "DDG       000                 WBBBBBBBBW               WBBBBBBBBW             WWWWW0          ",
            "DDDG      000                 BBBBBBBBBB        P       BBBBBBBBBB            WWWWWWW          ",
            "DDDDG     000                 BBBBBBBBBB      GGG      BBBBBBBBBB           WWWWWWWWW         ",
            "DDDDDGGGGGGGGGGGGGGGGGGGGGGGGGWWWWWWWWWWGGGGGGDDDGGGGGGWWWWWWWWWWGGGGGGGBGGGGGGGGGGGGGGGGGGGGG",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDWBBWWWWWWWWWWWWWWWWWDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDWBBBBBBBBBBBBBBBBBBBWWW",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDWBBBBBBBBBBBBBBBBBBBBB",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDWWWWWWWWWWWWWWWWBBBBB",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDWWWWW",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
            "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD"};

        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(VM, "Лабка");
            window.SetFramerateLimit(60);
            window.Closed += (s, e) => window.Close();
            window.KeyPressed += Window_KeyPressed;
            window.KeyReleased += Window_KeyReleased;
            window.SetKeyRepeatEnabled(false);

            for (int Height = 0; Height < Map.Length; Height++)
            {
                for (int Width = 0; Width < Map[Height].Length; Width++)
                {
                    if (Map[Height][Width] == 'P')
                    {
                        SpawnPoint = new Vector2f(Width * Resource.BlockSize, Height * Resource.BlockSize);
                        Map[Height] = Map[Height].Remove(Width, 1);
                    }
                }
            }

            MyEntity = new Entity(SpawnPoint);
            Person[] p = new Person[10];
            for (int i = 0; i < 10; i++)
            {
                p[i] = new Person(SpawnPoint, new Random().Next(0, 5));
            }
            Sprite Background = new Sprite(new Texture("Texture/background.png"));
            Background.Scale = new Vector2f(VM.Width / 1920f, VM.Height / 1080f);
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw( Background );
                Sprite block;
                for (int Height = 0; Height < Map.Length; Height++)
                {
                    for (int Width = 0; Width < Map[Height].Length; Width++)
                    {
                        switch (Map[Height][Width])
                        {
                            case 'W':
                                {
                                    block = Resource.GetBlock(0);
                                    block.Position = new Vector2f(Width * Resource.BlockSize - MyEntity.GetPosition.X + Entity.OffsetX, Height * Resource.BlockSize - MyEntity.GetPosition.Y + Entity.OffsetY);
                                    window.Draw(block);
                                    break;
                                }
                            case 'B':
                                {
                                    block = Resource.GetBlock(1);
                                    block.Position = new Vector2f(Width * Resource.BlockSize - MyEntity.GetPosition.X + Entity.OffsetX, Height * Resource.BlockSize - MyEntity.GetPosition.Y + Entity.OffsetY);
                                    window.Draw(block);
                                    break;
                                }
                            case 'D':
                                {
                                    block = Resource.GetBlock(2);
                                    block.Position = new Vector2f(Width * Resource.BlockSize - MyEntity.GetPosition.X + Entity.OffsetX, Height * Resource.BlockSize - MyEntity.GetPosition.Y + Entity.OffsetY);
                                    window.Draw(block);
                                    break;
                                }
                            case 'G':
                                {
                                    block = Resource.GetBlock(3);
                                    block.Position = new Vector2f(Width * Resource.BlockSize - MyEntity.GetPosition.X + Entity.OffsetX, Height * Resource.BlockSize - MyEntity.GetPosition.Y + Entity.OffsetY);
                                    window.Draw(block);
                                    break;
                                }
                        }
                    }
                }
                window.Draw(MyEntity.Draw());
                for (int i = 0; i < 10; i++)
                {
                    window.Draw(p[i].Draw());
                }
                window.Display();
            }
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Space:
                    {
                        MyEntity.Jump();
                        break;
                    }
                case Keyboard.Key.A:
                    {
                        MyEntity.isMoving = true;
                        MyEntity.FacingRight = false;
                        break;
                    }
                case Keyboard.Key.D:
                    {
                        MyEntity.isMoving = true;
                        MyEntity.FacingRight = true;
                        break;
                    }
                /*
                case Keyboard.Key.W:
                    {

                        break;
                    }
                case Keyboard.Key.S:
                    {

                        break;
                    }
                */
                case Keyboard.Key.Escape:
                    {
                        ( (RenderWindow) sender ).Close();
                        break;
                    }
            }
        }

        private static void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                /*
                case Keyboard.Key.Space:
                    {
                        
                        break;
                    }
                */
                case Keyboard.Key.A:
                    {
                        MyEntity.isMoving = false;
                        break;
                    }
                case Keyboard.Key.D:
                    {
                        MyEntity.isMoving = false;
                        break;
                    }
                /*
                case Keyboard.Key.W:
                    {
                        
                        break;
                    }
                case Keyboard.Key.S:
                    {
                        
                        break;
                    }
                */
            }
        }
    }
}
