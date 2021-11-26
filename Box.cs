using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled_blitz
{
    public class Box
    {
        public int x;
        public int y;
        public ConsoleColor color;
        public bool isSelected;
        public bool isCursorPosition;
        private char[,] symbols = new char[3, 3];

        public Box(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.isSelected = false;
            this.isCursorPosition = false;
        }

        public void InitBox(char symbol)
        {
            for (int i = 0; i < symbols.GetLength(0); i++)
            {
                for (int j = 0; j < symbols.GetLength(1); j++)
                {
                    symbols[i, j] = symbol;
                }
            }
        }

        public void DrawBox()
        {
            Console.ForegroundColor = this.color;
            for (int i = 0; i < this.symbols.GetLength(0); i++)
            {
                for (int j = 0; j < this.symbols.GetLength(1); j++)
                {
                    Console.SetCursorPosition(this.x + i, this.y + j);
                    Console.Write(this.symbols[i, j]);
                    //Console.Write('T');
                }
            }
            //TODO: DYALI HADI MASAL7A LTA 9ALWA 5ASNI NA
            switch (this.isSelected)
            {
                case false: // Not Selected
                    Console.SetCursorPosition(this.x + 1, this.y - 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 3, this.y + 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 3, this.y);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 3, this.y + 2);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 1, this.y + 3);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x - 1, this.y + 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x - 1, this.y);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x - 1, this.y + 2);
                    Console.Write(' ');
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
                    Console.SetCursorPosition(this.x - 1, this.y - 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 3, this.y - 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x + 3, this.y + 3);
                    Console.Write(' ');
                    Console.SetCursorPosition(this.x - 1, this.y + 3);
                    Console.Write(' ');
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
    }
}