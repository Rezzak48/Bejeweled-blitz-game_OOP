using System;

namespace Bejeweled_blitz
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Game gm = new Game();
            gm.Test();
            Map tst = new Map();
            //tst.InitField();
            gm.SettingDisplay();
            gm.DestroyJewels();
            //  gm.isEmpty();
            // tst.drawMap();
            gm.FallDownAndGenerateNewJewels();
            // Console.ReadLine();
        }
    }
}