using System;

namespace Bejeweled_blitz
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Game gm = new Game();
            Map tst = new Map();

            gm.Test();

            //tst.InitField();
            //gm.SettingDisplay();
            //gm.ScoreDisplay();
            //gm.FallDownAndGenerateNewJewels();
            //gm.Engine();
            // Console.ReadLine();
        }
    }
}