using System;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using SnakeNamespace;

namespace SnakeNamespace
{
    class Snake : IUpdateUI
    {
        private int DirectionOfMoving = 4; // { 0-Right  1-Up  2-Left  3-Down }
        private int DirectionOfMovingBuffer = 4; //Contains next Direction of moving
        private bool CanChangeDirectionOfMoving = true;
        private readonly object PadLock = new object();
        private Game game;

        public SolidColorBrush Colour = new SolidColorBrush(Colors.Blue);
        public bool AbortMoveThread = false;
        public List<MapSquere> SnakeBodyList = new List<MapSquere>();
        public List<MapSquere> GetBodyAsArrayList
        {
            get
            {
                return SnakeBodyList;
            }
        }

        public Snake(Game game)
        {
            this.game = game;
            //Create head of snake
            SnakeBodyList.Add(new MapSquere(game, this));

            //Create 3 more element of snake body
            for (int i = 0; i < 3; i++)
            {
                SnakeBodyList.Add(new MapSquere(this));
            }
        }
    
        public void ChangeDirectionOfMoving(Key key)
        {
            //Checks if Direction of moving isn't Oposing Current Direction of moving
            if(key == Key.Left) 
            {
                if (game.snake.DirectionOfMoving == 0)
                    return;
                lock (game.snake.PadLock)
                {
                    game.snake.DirectionOfMovingBuffer = 2;
                }
            }
            else if (key == Key.Right)
            {
                if (game.snake.DirectionOfMoving == 2)
                    return;

                lock (game.snake.PadLock)
                {
                    game.snake.DirectionOfMovingBuffer = 0;
                }
            }
            else if (key == Key.Down)
            {
                if (game.snake.DirectionOfMoving == 1)
                    return;

                lock (game.snake.PadLock)
                {
                    game.snake.DirectionOfMovingBuffer = 3;
                }
            }
            else if (key == Key.Up)
            {
                if (game.snake.DirectionOfMoving == 3)
                    return;

                lock (game.snake.PadLock)
                {
                    game.snake.DirectionOfMovingBuffer = 1;
                }
            }
            if (game.snake.CanChangeDirectionOfMoving == true)
            {
                game.snake.DirectionOfMoving = game.snake.DirectionOfMovingBuffer;
                game.snake.CanChangeDirectionOfMoving = false;
            }
        }

        public void Move(MainWindow mainWindow)
        {
            Thread.Sleep(100);
            AbortMoveThread = false;
            var timeBetwenMove = 16;
            mainWindow.UpdateUI(this);
            while (true)
            {
                lock (PadLock)
                {
                    if (AbortMoveThread == true)
                    {
                        return;
                    }
                }
                for(int i = 0; i < SnakeBodyList.Count; i++)
                {
                    SnakeBodyList[i].LastCanvasLeft = SnakeBodyList[i].CanvasLeft;
                    SnakeBodyList[i].LastCanvasTop = SnakeBodyList[i].CanvasTop;
                }

                var TargetPosition = new int[2] { SnakeBodyList[0].CanvasLeft, SnakeBodyList[0].CanvasTop };

                CanChangeDirectionOfMoving = true;
                switch (DirectionOfMoving)
                {
                    case 0: //Right
                        {
                            TargetPosition[0] += Game.RectangleSize;
                            CheckNextPosition(TargetPosition);

                            while (SnakeBodyList[0].CanvasLeft != TargetPosition[0])
                            {
                                SnakeBodyList[0].CanvasLeft += 1;
                                MoveTailOfSnake();

                                mainWindow.UpdateUI(this);
                                Thread.Sleep(timeBetwenMove);
                            }
                            break;
                        }
                    case 1: //up
                        {
                            TargetPosition[1] -= Game.RectangleSize;
                            CheckNextPosition(TargetPosition);

                            while (SnakeBodyList[0].CanvasTop != TargetPosition[1])
                            {
                                SnakeBodyList[0].CanvasTop -= 1;
                                MoveTailOfSnake();

                                mainWindow.UpdateUI(this);
                                Thread.Sleep(timeBetwenMove);
                            }
                            break;
                        }
                    case 2: //Left
                        {
                            TargetPosition[0] -= Game.RectangleSize;
                            CheckNextPosition(TargetPosition);

                            while (SnakeBodyList[0].CanvasLeft != TargetPosition[0])
                            {
                                SnakeBodyList[0].CanvasLeft -= 1;
                                MoveTailOfSnake();

                                mainWindow.UpdateUI(this);
                                Thread.Sleep(timeBetwenMove);
                            }
                            break;
                        }
                    case 3: //Down
                        {
                            TargetPosition[1] += Game.RectangleSize;
                            CheckNextPosition(TargetPosition);

                            while (SnakeBodyList[0].CanvasTop != TargetPosition[1])
                            {
                                SnakeBodyList[0].CanvasTop += 1;
                                MoveTailOfSnake();

                                mainWindow.UpdateUI(this);
                                Thread.Sleep(timeBetwenMove);
                            }
                            break;
                        }
                    default:
                        {
                            Thread.Sleep(100);
                            continue ;
                        }
                }
                if(CanChangeDirectionOfMoving == true)
                {
                    DirectionOfMoving = DirectionOfMovingBuffer;
                }
                game.copyOfSnake.UpdateCopyOfSnake();
            }
        }
        private void MoveTailOfSnake()
        {
            for (int i = 1; i < SnakeBodyList.Count; i++)
            {
                if (SnakeBodyList[i - 1].LastCanvasLeft > SnakeBodyList[i].CanvasLeft)
                {
                    SnakeBodyList[i].CanvasLeft += 1;
                }
                else if (SnakeBodyList[i - 1].LastCanvasLeft < SnakeBodyList[i].CanvasLeft)
                {
                    SnakeBodyList[i].CanvasLeft -= 1;
                }
                else if (SnakeBodyList[i - 1].LastCanvasTop > SnakeBodyList[i].CanvasTop)
                {
                    SnakeBodyList[i].CanvasTop += 1;
                }
                else if (SnakeBodyList[i - 1].LastCanvasTop < SnakeBodyList[i].CanvasTop)
                {
                    SnakeBodyList[i].CanvasTop -= 1;
                }
            }
        }


        private void CheckNextPosition(int[] TargetPosition)
        {
            for (int i = 0; i < 2; i++)
            {
                if (TargetPosition[i] >= game.GameCanvasSizeProp || TargetPosition[i] <= -1)
                {
                    game.EndOfGame();
                    Thread.CurrentThread.Abort();
                }
            }
            if(TargetPosition[0] == game.cherry.CherryBody.CanvasLeft && TargetPosition[1] == game.cherry.CherryBody.CanvasTop)
            {
                game.MainWindowObject.Dispatcher.BeginInvoke((Action)(() =>
                {
                    SnakeBodyList.Add(new MapSquere(this));
                    game.copyOfSnake.SnakeCopyBodyList.Add(new MapSquere(this, game.copyOfSnake));
                }));
                game.MainWindowObject.Score += 10;
                game.cherry.CherryBody.SetRandomCherryPosition(game);
                game.MainWindowObject.UpdateUI(game.cherry);
            }
            else if(CheckIfNextPositonIsSnake(TargetPosition) || game.MainWindowObject.Score >= 3960)
            {
                game.EndOfGame();
                Thread.CurrentThread.Abort();
            }
        }


        private bool CheckIfNextPositonIsSnake(int[] TargetPosition)
        {
            for(int i = 2; i < SnakeBodyList.Count; i++)
            {
                if(TargetPosition[0] == SnakeBodyList[i].CanvasLeft && TargetPosition[1] == SnakeBodyList[i].CanvasTop)
                {
                    return true;
                }
            }
            return false;
        }
    }


    
}