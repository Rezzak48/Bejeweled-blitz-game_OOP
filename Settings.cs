using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bejeweled_blitz
{
    internal class Settings
    {
        private static string[] scoresData = new string[5];
        private string path = "gameScore.txt";

        public static Game jew = new Game();

        public void InitSettings()
        {
            // Screen Size
            Console.Clear();
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight = 45;
            Console.BufferWidth = Console.WindowWidth = 33;
            Console.Title = "Just Jewels";
        }

        public string[] NewScore(int score)
        {
            int i = 0;
            while (i < scoresData.Length && scoresData[i] != null)
            {
                i++;
            }
            if (i < scoresData.Length)
            {
                scoresData[i] = score.ToString();
            }
            return scoresData;
        }

        public void SaveInFile()
        {
            for (int i = 0; i < scoresData.Length; i++)
            {
                string[] sortedData = sortByPoints(scoresData);
                File.WriteAllLines(path, sortedData);
            }
        }

        public string[] sortByPoints(string[] cntnt)
        {
            for (int i = 0; i < cntnt.Length; i++)
            {
                int j = i + 1;
                while (j < cntnt.Length - 1 && cntnt[i] != null && cntnt[j] != null)
                {
                    if (int.Parse(cntnt[i]) > int.Parse(cntnt[j]))
                    {
                        string temp = cntnt[i];
                        cntnt[i] = cntnt[j];
                        cntnt[j] = temp;
                    }
                    j++;
                }
            }
            return scoresData;
        }

        public void scoreResult()
        {
            Console.WriteLine("The results obtained are: ");
            string[] AllLines = File.ReadAllLines(path);
            for (int i = 0; i < AllLines.Length; i++)
            {
                Console.WriteLine(AllLines[i]);
            }
        }
    }
}