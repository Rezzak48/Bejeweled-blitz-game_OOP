using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled_blitz
{
    internal class Game
    {
        private int score;

        public Game()
        {
            score = 0;
        }

        //Console Display settings
        private void SettingDisplay()
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight = 50;
            Console.BufferWidth = Console.WindowWidth = 35;
            Console.Title = "Bejeweled_blitz";
        }

        private void ScoreDisplay()
        {
            Console.SetCursorPosition(0, 33);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("---------------------------------");
            Console.SetCursorPosition(1, 34);
            Console.Write("Score: {0}", score);
            Console.SetCursorPosition(20, 35);
        }

        private void GenerateNewJewels()
        {
            do
            {
            }
        }

        public void Test()
        {
            SettingDisplay();
            ScoreDisplay();
        }
    }
}