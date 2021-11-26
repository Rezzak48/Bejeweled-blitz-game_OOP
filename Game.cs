using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Bejeweled_blitz
{
    internal class Game
    {
        public static bool selectionExist = false;
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
            //Console.BufferHeight = Console.WindowHeight = 35;
            Console.BufferHeight = Console.WindowHeight = 45; // 38 default

            Console.BufferWidth = Console.WindowWidth = 33;
            Console.Title = "Bejeweled_blitz";
        }

        public void ScoreDisplay()
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
            FallDownAndGenerateNewJewels();
            Engine();
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
            map.InitField();
            FindBoxesForRemove();
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
                bonus = jewelCount / 30;
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

                            //if (playField[x, y].Color != ConsoleColor.Black && playField[x, y + 1].Color == ConsoleColor.Black)
                            if (true)
                            {
                                //Console.WriteLine("Condition hold");

                                Thread.Sleep(50);
                                this.lastSelection[0] = x;
                                this.lastSelection[1] = y;
                                this.cursorX = x;
                                this.cursorY = y + 1;
                                Swap(playField[x, y], playField[x, y + 1], playField);
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

        //public void FallDownAndGenerateNewJewels()

        //{
        //    do
        //    {
        //        //boxesToRemove = FindBoxesForRemove(playField);
        //        // Console.WriteLine("boxesToRemove");
        //        //TestMatrix(boxesToRemove);

        //        int jewelCount = 0;
        //        int bonus = 1;
        //        for (int y = 0; y < boxesToRemove.GetLength(0); y++)
        //        {
        //            for (int x = 0; x < boxesToRemove.GetLength(1); x++)
        //            {
        //                if (boxesToRemove[x, y])
        //                {
        //                    jewelCount += 10;
        //                }
        //            }
        //        }
        //        bonus = jewelCount / 30; // Тука може да гръмне ако bonus стане 0-ла
        //        //DisplayCombo(bonus);
        //        score += jewelCount * bonus;
        //        ScoreDisplay();
        //        DestroyJewels();

        //        // #____if not full
        //        while (!isFull())
        //        {
        //            for (int y = playField.GetLength(0) - 2; y >= 0; y--) // very Important to be GetLength(0) - 2 becouse we dont want to check the last ROW!
        //            {
        //                for (int x = playField.GetLength(1) - 1; x >= 0; x--)
        //                {
        //                    //hadi katchecki kola box and if the box is not black and li ta7to black katnazlo lta7ta
        //                    if (playField[x, y].Color != ConsoleColor.Black && playField[x, y + 1].Color == ConsoleColor.Black)
        //                    {
        //                        Thread.Sleep(50);
        //                        lastSelection[0] = x;
        //                        lastSelection[1] = y;
        //                        cursorX = x;
        //                        cursorY = y + 1;
        //                        Swap(playField[x, y], playField[x, y + 1], playField);
        //                        //Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
        //                    }

        //                    //ila kant l box khawi wkayan fl first row just create one with that anuimation
        //                    if (y == 0 && playField[x, y].Color == ConsoleColor.Black)
        //                    {
        //                        playField[x, y].Color = colors[rnd.Next(0, colors.Length)];
        //                        Thread.Sleep(30);
        //                        playField[x, y].InitOneJewelry('\u2591'); // Light-Shade
        //                        playField[x, y].drawBox();
        //                        Thread.Sleep(50);
        //                        playField[x, y].InitOneJewelry('\u2592'); // Medium-Shade
        //                        playField[x, y].drawBox();
        //                        Thread.Sleep(50);
        //                        playField[x, y].InitOneJewelry('\u2593'); // Dark-Shade
        //                        playField[x, y].drawBox();
        //                        Thread.Sleep(50);
        //                        playField[x, y].InitOneJewelry('\u2588'); // Restore FULL BLOCK when BLACK!
        //                        playField[x, y].drawBox();
        //                    }
        //                }
        //            }
        //        }
        //        // Set the bool matrix to False
        //        Array.Clear(boxesToRemove, 0, boxesToRemove.Length);
        //    } while (!isEmpty());
        //    //TestMatrix(boxesToRemove);
        //}

        public void Engine()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                    if (keyPressed.Key == ConsoleKey.LeftArrow)
                    {
                        if (cursorX > 0)
                        {
                            cursorX--;
                        }
                        if (selectionExist)
                        {
                            Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);

                            if (isEmpty())
                            {
                                Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            }
                            // Ако имаме комбинация - пускаме метода за чистене
                            else
                            {
                                playField[cursorX, cursorY].IsCursorPosition = false;
                                playField[cursorX, cursorY].IsSelected = false;
                                playField[cursorX, cursorY].drawBox();
                                FallDownAndGenerateNewJewels();
                            }
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.RightArrow)
                    {
                        if (cursorX < 7)
                        {
                            cursorX++;
                        }
                        if (selectionExist)
                        {
                            Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            // Ако след разменянето - нямаме комбинация - връщаме оригиналните позиции.
                            // TODO: Може да се вкара анимация, понеже в момента изглежда все едно, че нищо не се случва.
                            if (isEmpty())
                            {
                                Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            }
                            // Ако имаме комбинация - пускаме метода за чистене
                            else
                            {
                                playField[cursorX, cursorY].IsCursorPosition = false;
                                playField[cursorX, cursorY].drawBox();
                                FallDownAndGenerateNewJewels();
                            }
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.UpArrow)
                    {
                        if (cursorY > 0)
                        {
                            cursorY--;
                        }
                        if (selectionExist)
                        {
                            Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            // Ако след разменянето - нямаме комбинация - връщаме оригиналните позиции.
                            // TODO: Може да се вкара анимация, понеже в момента изглежда все едно, че нищо не се случва.
                            if (isEmpty())
                            {
                                Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            }
                            // Ако имаме комбинация - пускаме метода за чистене
                            else
                            {
                                playField[cursorX, cursorY].IsCursorPosition = false;
                                playField[cursorX, cursorY].drawBox();
                                FallDownAndGenerateNewJewels();
                            }
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.DownArrow)
                    {
                        if (cursorY < 7)
                        {
                            cursorY++;
                        }
                        if (selectionExist)
                        {
                            Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            // Ако след разменянето - нямаме комбинация - връщаме оригиналните позиции.
                            // TODO: Може да се вкара анимация, понеже в момента изглежда все едно, че нищо не се случва.
                            if (isEmpty())
                            {
                                Swap(playField[lastSelection[0], lastSelection[1]], playField[cursorX, cursorY], playField);
                            }
                            // Ако имаме комбинация - пускаме метода за чистене
                            else
                            {
                                playField[cursorX, cursorY].IsCursorPosition = false;
                                playField[cursorX, cursorY].drawBox();
                                FallDownAndGenerateNewJewels();
                            }
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.Spacebar)
                    {
                        playField[cursorX, cursorY].IsSelected = true;
                        playField[cursorX, cursorY].drawBox();
                        selectionExist = true;
                        lastSelection[0] = cursorX;
                        lastSelection[1] = cursorY;
                    }
                    else
                    {
                        for (int i = 0; i < playField.GetLength(0); i++)
                        {
                            for (int j = 0; j < playField.GetLength(1); j++)
                            {
                                if (playField[i, j].IsCursorPosition)
                                {
                                    playField[i, j].IsCursorPosition = false;
                                }
                                playField[i, j].drawBox();
                            }
                        }
                        playField[cursorX, cursorY].IsCursorPosition = true;
                        playField[cursorX, cursorY].drawBox();
                    }
                    // ScoreField(); - Here we can update the score field
                }
            }
        }

        public void Swap(Map first, Map second, Map[,] playField)
        {
            int tempX = first.X;
            int tempY = first.Y;

            first.X = second.X;
            first.Y = second.Y;

            second.X = tempX;
            second.Y = tempY;

            selectionExist = false;
            first.IsSelected = false;
            second.IsSelected = false;
            first.drawBox();
            second.drawBox();

            Map tempJewel = playField[this.lastSelection[0], this.lastSelection[1]];
            playField[this.lastSelection[0], this.lastSelection[1]] = playField[this.cursorX, this.cursorY];
            playField[this.cursorX, this.cursorY] = tempJewel;
        }
    }
}