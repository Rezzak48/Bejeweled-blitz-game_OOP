using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Bejeweled_blitz
{
    internal class Game
    {
        public bool selectionExist = false;
        public int[] lastSelection;

        //public static int cursorX = 0;
        //public static int cursorY = 0;
        public int cursorX;

        public int cursorY;

        // public static int score = 0;
        private static Map map = new Map();

        private static Map map2 = new Map();

        private int score;
        private bool[,] boxesToremove;
        private static ConsoleColor[] colors = { ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Cyan };
        private Random rnd = new Random();
        private static Map[,] playField = map.InitField();
        private bool[,] boxesToRemove = FindBoxesForRemove();

        public Game()
        {
            score = 0;
            lastSelection = new int[] { -1, -1 };
            cursorX = 0;
            cursorY = 0;
        }

        //Console Display settings
        public void SettingDisplay()
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight = 45;
            Console.BufferWidth = Console.WindowWidth = 33;
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

        public void Test()
        {
            SettingDisplay();
            ScoreDisplay();
        }

        // algo to decide if a sequence (horizantal or vertical has the same color) and return those boxes in a sequence called selected
        public static bool[,] FindBoxesForRemove()
        {
            //Map mp = new Map();
            //Map[,] playField = mp.InitField();

            int currentSeq = 1;
            int bestSeq = int.MinValue;
            int bestSeqX = 0;
            int bestSeqY = 0;
            int bestSeqDirection = 1; // 1 = horizontal; 2 = right
            bool finishFlag = false;
            bool[,] selectedCells = new bool[playField.GetLength(0), playField.GetLength(1)];

            do
            {
                // horizontal sequences - left to right
                for (int x = 0; x < playField.GetLength(0); x++)
                {
                    for (int y = 0; y < playField.GetLength(1) - 1; y++)
                    {
                        // Hadi 9adiya dl color, I am still not sure about it
                        if (playField[x, y].Color == playField[x, y + 1].Color && selectedCells[x, y] == false)
                        {
                            currentSeq++;
                        }
                        else
                        {
                            currentSeq = 1;
                        }

                        if (currentSeq > bestSeq)
                        {
                            bestSeq = currentSeq;
                            bestSeqX = x;
                            bestSeqY = y + 1;
                            bestSeqDirection = 1; // 1 = horizontal
                        }
                    }
                    currentSeq = 1;
                }

                // vertical sequences - top to down
                for (int y = 0; y < playField.GetLength(1); y++)
                {
                    for (int x = 0; x < playField.GetLength(0) - 1; x++)
                    {
                        if (playField[x, y].Color == playField[x + 1, y].Color && selectedCells[x, y] == false)
                        {
                            currentSeq++;
                        }
                        else
                        {
                            currentSeq = 1;
                        }

                        if (currentSeq > bestSeq)
                        {
                            bestSeq = currentSeq;
                            bestSeqY = y;
                            bestSeqX = x + 1;
                            bestSeqDirection = 2; // 2 = down
                        }
                    }
                    currentSeq = 1;
                }

                // Populate the bool matrix for selected cells only when the sequence length is longer than 2
                if (bestSeq >= 3)
                {
                    switch (bestSeqDirection)
                    {
                        case 1: // 1 = horizontal
                            for (int i = bestSeqY; i >= Math.Abs(bestSeq - bestSeqY - 1); i--)
                            {
                                selectedCells[bestSeqX, i] = true;
                            }
                            break;

                        case 2: // 2 = down
                            for (int i = bestSeqX; i >= Math.Abs(bestSeq - bestSeqX - 1); i--)
                            {
                                selectedCells[i, bestSeqY] = true;
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    finishFlag = true;
                }
                currentSeq = 1;
                bestSeq = int.MinValue;
                bestSeqX = 0;
                bestSeqY = 0;
                bestSeqDirection = 1; // 1 = horizontal; 2 = right
            } while (finishFlag == false);
            return selectedCells;
        }

        // take the return of FindBoxTorRemove and remove it by turning it's color to black again
        public void DestroyJewels()
        {
            //bool[,] boxesToRemove = FindBoxesForRemove();
            FindBoxesForRemove();
            Thread.Sleep(400); //TODO: Adjust Speed
            for (int y = 0; y < playField.GetLength(0); y++)
            {
                for (int x = 0; x < playField.GetLength(1); x++)
                {
                    if (boxesToRemove[x, y] == true)
                    {
                        playField[x, y].InitOneJewelry('\u2593'); // Dark-Shade
                        playField[x, y].drawBox();
                        Thread.Sleep(50);
                        playField[x, y].InitOneJewelry('\u2592'); // Medium-Shade
                        playField[x, y].drawBox();
                        Thread.Sleep(50);
                        playField[x, y].InitOneJewelry('\u2591'); // Light-Shade
                        playField[x, y].drawBox();
                        Thread.Sleep(50);
                        playField[x, y].Color = ConsoleColor.Black;
                        Thread.Sleep(50);
                        playField[x, y].InitOneJewelry('\u2588'); // Restore FULL BLOCK when BLACK!
                        playField[x, y].drawBox();
                    }
                }
            }
        }

        // it return the boolean if a boxes has been destroyed  - This method checks if the boxesToRemove bool matrix is empty or not
        public bool isEmpty()
        {
            // bool[,] boxesToRemove = FindBoxesForRemove();
            for (int y = 0; y < boxesToRemove.GetLength(0); y++)
            {
                for (int x = 0; x < boxesToRemove.GetLength(1); x++)
                {
                    if (boxesToRemove[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // I'll need it to make sure that the map is full before u can move or select sth
        public bool isFull()
        {
            //Map mp = new Map();
            //Map[,] playField = mp.InitField();
            for (int y = 0; y < playField.GetLength(0); y++)
            {
                for (int x = 0; x < playField.GetLength(1); x++)
                {
                    if (playField[x, y].Color == ConsoleColor.Black)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void FallDownAndGenerateNewJewels()
        {
            //Map mp = new Map();
            //Map[,] playField = mp.InitField();
            do
            {
                //bool[,] boxesToRemove = FindBoxesForRemove();

                // #------------check removed boxes and calculate points-------------
                int jewelCount = 0;
                int bonus = 1;
                for (int y = 0; y < boxesToRemove.GetLength(0); y++)
                {
                    for (int x = 0; x < boxesToRemove.GetLength(1); x++)
                    {
                        if (boxesToRemove[x, y])
                        {
                            jewelCount += 10;
                        }
                    }
                }
                //bonus = jewelCount / 30; // Тука може да гръмне ако bonus стане 0-ла
                //DisplayCombo(bonus);
                score += jewelCount * bonus;

                ScoreDisplay();
                DestroyJewels();
                // #------------------------------------------------------------------

                // #--------------------if not full---------------
                while (!isFull())
                {
                    for (int y = playField.GetLength(0) - 2; y >= 0; y--) // very Important to be GetLength(0) - 2 becouse we dont want to check the last ROW! Why?
                    {
                        for (int x = playField.GetLength(1) - 1; x >= 0; x--)
                        {
                            //hadi katchecki kola box and if the box is not black and li ta7to black katnazlo lta7ta

                            if (playField[x, y].Color != ConsoleColor.Black && playField[x, y + 1].Color == ConsoleColor.Black)
                            {
                                //\\Console.WriteLine("Condition hold");

                                Thread.Sleep(50);
                                lastSelection[0] = x;
                                lastSelection[1] = y;
                                cursorX = x;
                                cursorY = y + 1;
                                // Swap(playField[x, y], playField[x, y + 1], playField);
                                Swap(playField[x, y], playField[x, y + 1], playField);
                                // playField[x, y + 1];
                            }
                            //ila kant l box khawi wkayan fl first row just create one with that anuimation
                            if (y == 0 && playField[x, y].Color == ConsoleColor.Black)
                            {
                                playField[x, y].Color = colors[rnd.Next(0, colors.Length)];
                                Thread.Sleep(30);
                                playField[x, y].InitOneJewelry('\u2591'); // Light-Shade
                                playField[x, y].drawBox();
                                Thread.Sleep(50);
                                playField[x, y].InitOneJewelry('\u2592'); // Medium-Shade
                                playField[x, y].drawBox();
                                Thread.Sleep(50);
                                playField[x, y].InitOneJewelry('\u2593'); // Dark-Shade
                                playField[x, y].drawBox();
                                Thread.Sleep(50);
                                playField[x, y].InitOneJewelry('\u2588'); // Restore FULL BLOCK when BLACK!
                                playField[x, y].drawBox();
                            }
                        }
                    }
                }
                // #----------------------------------------

                // Set the bool matrix to False
                Array.Clear(boxesToRemove, 0, boxesToRemove.Length);
            } while (!isEmpty());
            //TestMatrix(boxesToRemove);
        }

        public void Swap(Map map, Map map2, Map[,] playField)
        {
            int tempX = map.X;
            int tempY = map.Y;
            map.X = map2.X;
            map.Y = map2.Y;
            map2.X = tempX;
            map2.Y = tempY;

            selectionExist = false;
            map.IsSelected = false;
            map2.IsSelected = false;
            map.drawBox();
            map2.drawBox();

            Map tempJewel = playField[lastSelection[0], lastSelection[1]];
            playField[lastSelection[0], lastSelection[1]] = playField[cursorX, cursorY];
            playField[cursorX, cursorY] = tempJewel;
        }
    }
}