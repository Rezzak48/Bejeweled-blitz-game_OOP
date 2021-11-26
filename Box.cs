using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled_blitz
{
    public class Map
    {
        private static ConsoleColor[] colors = { ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Cyan };
        private Random rnd = new Random();
        private int x;
        private int y;
        private bool isSelected;
        private bool isCursorPosition;
        private ConsoleColor color;
        private char[,] symbols = new char[3, 3];
        private Map[,] playField = new Map[8, 8];

        public ConsoleColor Color
        {
            get
            {
                return color;
            }
            set
            {
                if (value == ConsoleColor.Black)
                {
                    //Check if that really works
                    color = value;
                }
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }

        public bool IsCursorPosition
        {
            get
            {
                return isCursorPosition;
            }
            set
            {
                isCursorPosition = value;
            }
        }

        //public Map(int x, int y, ConsoleColor color)
        public Map(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.isSelected = false;
            this.isCursorPosition = false;
            this.color = color;
        }

        public Map() : this(0, 0, colors[0])
        {
        }

        // hadi ri katchad kola element fl array wkatstori fih symbol li kayjiha ka parameter (khaso ywali mli t3ayat l class)
        public void InitOneJewelry(char symbol)
        {
            for (int i = 0; i < symbols.GetLength(0); i++)
            {
                for (int j = 0; j < symbols.GetLength(1); j++)
                {
                    symbols[i, j] = symbol;
                }
            }
        }

        //katchad kola element bo7do wta5do b position dyalo wtsaba4lo sub element li fosto lda5al bach ta3tina one box
        public void drawBox()
        {
            //Color kay3ni l color li tpasa mli t3ayat 3la had l class ka parameter
            Console.ForegroundColor = this.color;
            for (int i = 0; i < symbols.GetLength(0); i++)
            {
                for (int j = 0; j < symbols.GetLength(1); j++)
                {
                    Console.SetCursorPosition(this.x + i, this.y + j);
                    Console.Write(this.symbols[i, j]);
                }
            }
            //TODO: Maybe 5asni n7atha f function bo7dhom hado
            // is seledected 3naw biha mli tkon wati 3la espace tma katkon selected
            switch (this.isSelected)
            {
                case false: // Not Selected
                    //Console.SetCursorPosition(this.x + 1, this.y - 1);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 3, this.y + 1);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 3, this.y);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 3, this.y + 2);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 1, this.y + 3);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x - 1, this.y + 1);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x - 1, this.y);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x - 1, this.y + 2);
                    //Console.Write(' ');
                    break;

                case true: // isSelected
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(this.x + 3, this.y + 1);
                    Console.Write('|');
                    Console.SetCursorPosition(this.x + 3, this.y);
                    Console.Write('|');
                    Console.SetCursorPosition(this.x + 3, this.y + 2);
                    Console.Write('|');
                    Console.SetCursorPosition(this.x - 1, this.y + 1);
                    Console.Write('|');
                    Console.SetCursorPosition(this.x - 1, this.y);
                    Console.Write('|');
                    Console.SetCursorPosition(this.x - 1, this.y + 2);
                    Console.Write('|');
                    break;
            }

            switch (isCursorPosition)
            {
                case false: // Not Selected
                    //Console.SetCursorPosition(this.x - 1, this.y - 1);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 3, this.y - 1);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x + 3, this.y + 3);
                    //Console.Write(' ');
                    //Console.SetCursorPosition(this.x - 1, this.y + 3);
                    //Console.Write(' ');
                    break;

                case true: // isSelected
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(this.x - 1, this.y - 1);
                    Console.Write('\u250c');
                    Console.SetCursorPosition(this.x + 3, this.y - 1);
                    Console.Write('\u2510');
                    Console.SetCursorPosition(this.x + 3, this.y + 3);
                    Console.Write('\u2518');
                    Console.SetCursorPosition(this.x - 1, this.y + 3);
                    Console.Write('\u2514');
                    break;
            }
        }

        // kaysna3 lfield kaml by making box by box, kola wa7d m5talaf 3la lakhor by y3tihom kola mra location dyal box w symbol li brah tma wlkhra l color li yfilih
        public Map[,] InitField()
        {
            for (int i = 0; i < playField.GetLength(0); i++)
            {
                for (int j = 0; j < playField.GetLength(1); j++)
                {
                    playField[i, j] = new Map(i * 4 + 1, j * 4 + 1, colors[rnd.Next(0, colors.Length)]);
                    // Dark-Shade: \u2593; MediumShade: \u2592; LightShade: \u2591
                    playField[i, j].InitOneJewelry('\u2588'); // Dark-Shade: \u2593; MediumShade: \u2592; LightShade: \u2591
                    //playField[i, j].InitOneJewelry(''); // Dark-Shade: \u2593; MediumShade: \u2592; LightShade: \u2591
                    playField[i, j].drawBox();
                }
            }
            return playField;
        }
    }
}