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
    class SnakeMove
    {
        private enum Directions
        {
            Right = 0,
            Up = 1,
            Left = 2,
            Down = 3,
            None = 4
        }

        private int DirectionOfMoving = (int)Directions.None;
        private int DirectionOfMovingBuffer = (int)Directions.None; //Contains next Direction of moving
        private bool CanChangeDirectionOfMoving = true;
        private readonly object PadLock = new object();
        private Game game;
        private Snake snake;

        public bool AbortMoveThread = false;
        public Thread moveThread;

        public SnakeMove(Game game, Snake snake)
        {
            this.game = game;
            this.snake = snake;

            StartNewMoveThread();
        }

        public void StartNewMoveThread()
        {
            moveThread = new Thread(new ThreadStart(() => Move(game.MainWindowObject)))
            {
                IsBackground = true
            };
            moveThread.SetApartmentState(ApartmentState.STA);
            moveThread.Start();
        }

        public void EndMoveThread()
        {
            if (moveThread.IsAlive)
            {
                AbortMoveThread = true;
            }
        }

        public void ChangeDirectionOfMoving(Key key)
        {
            //Checks if Direction of moving isn't Oposing Current Direction of moving
            if (key == Key.Left)
            {
                if (DirectionOfMoving == (int)Directions.Right)
                    return;
                lock (PadLock)
                {
                    DirectionOfMovingBuffer = (int)Directions.Left;
                }
            }
            else if (key == Key.Right)
            {
                if (DirectionOfMoving == (int)Directions.Left)
                    return;

                lock (PadLock)
                {
                    DirectionOfMovingBuffer = (int)Directions.Right;
                }
            }
            else if (key == Key.Down)
            {
                if (DirectionOfMoving == (int)Directions.Up)
                    return;

                lock (PadLock)
                {
                    DirectionOfMovingBuffer = (int)Directions.Down;
                }
            }
            else if (key == Key.Up)
            {
                if (DirectionOfMoving == (int)Directions.Down)
                    return;

                lock (PadLock)
                {
                    DirectionOfMovingBuffer = (int)Directions.Up;
                }
            }
            if (CanChangeDirectionOfMoving)
            {
                DirectionOfMoving = DirectionOfMovingBuffer;
                CanChangeDirectionOfMoving = false;
            }
        }

        public void Move(MainWindow mainWindow)
        {
            Thread.Sleep(100);
            AbortMoveThread = false;
            mainWindow.UpdateUI(snake);
            while (true)
            {
                lock (PadLock)
                {
                    if (AbortMoveThread == true)
                    {
                        return;
                    }
                }
                for (int i = 0; i < snake.SnakeBodyList.Count; i++)
                {
                    snake.SnakeBodyList[i].LastCanvasLeft = snake.SnakeBodyList[i].CanvasLeft;
                    snake.SnakeBodyList[i].LastCanvasTop = snake.SnakeBodyList[i].CanvasTop;
                }

                GoToTargetPositon();
               
                if (CanChangeDirectionOfMoving)
                {
                    DirectionOfMoving = DirectionOfMovingBuffer;
                }
                game.copyOfSnake.UpdateCopyOfSnake();
            }
        }
        
        private void GoToTargetPositon()
        {
            var timeBetwenMove = 16;
            var TargetPosition = new int[2] { snake.SnakeBodyList[0].CanvasLeft, snake.SnakeBodyList[0].CanvasTop };

            CanChangeDirectionOfMoving = true;
            switch (DirectionOfMoving)
            {
                case (int)Directions.Right:
                    {
                        TargetPosition[0] += Game.RectangleSize;
                        CheckNextPosition(TargetPosition);

                        while (snake.SnakeBodyList[0].CanvasLeft != TargetPosition[0])
                        {
                            snake.SnakeBodyList[0].CanvasLeft += 1;
                            MoveTailOfSnake();

                            game.MainWindowObject.UpdateUI(snake);
                            Thread.Sleep(timeBetwenMove);
                        }
                        break;
                    }
                case (int)Directions.Up:
                    {
                        TargetPosition[1] -= Game.RectangleSize;
                        CheckNextPosition(TargetPosition);

                        while (snake.SnakeBodyList[0].CanvasTop != TargetPosition[1])
                        {
                            snake.SnakeBodyList[0].CanvasTop -= 1;
                            MoveTailOfSnake();

                            game.MainWindowObject.UpdateUI(snake);
                            Thread.Sleep(timeBetwenMove);
                        }
                        break;
                    }
                case (int)Directions.Left:
                    {
                        TargetPosition[0] -= Game.RectangleSize;
                        CheckNextPosition(TargetPosition);

                        while (snake.SnakeBodyList[0].CanvasLeft != TargetPosition[0])
                        {
                            snake.SnakeBodyList[0].CanvasLeft -= 1;
                            MoveTailOfSnake();

                            game.MainWindowObject.UpdateUI(snake);
                            Thread.Sleep(timeBetwenMove);
                        }
                        break;
                    }
                case (int)Directions.Down:
                    {
                        TargetPosition[1] += Game.RectangleSize;
                        CheckNextPosition(TargetPosition);

                        while (snake.SnakeBodyList[0].CanvasTop != TargetPosition[1])
                        {
                            snake.SnakeBodyList[0].CanvasTop += 1;
                            MoveTailOfSnake();

                            game.MainWindowObject.UpdateUI(snake);
                            Thread.Sleep(timeBetwenMove);
                        }
                        break;
                    }
                default:
                    {
                        Thread.Sleep(100);
                        break;
                    }
            }
        }

        private void MoveTailOfSnake()
        {
            for (int i = 1; i < snake.SnakeBodyList.Count; i++)
            {
                if (snake.SnakeBodyList[i - 1].LastCanvasLeft > snake.SnakeBodyList[i].CanvasLeft)
                {
                    snake.SnakeBodyList[i].CanvasLeft += 1;
                }
                else if (snake.SnakeBodyList[i - 1].LastCanvasLeft < snake.SnakeBodyList[i].CanvasLeft)
                {
                    snake.SnakeBodyList[i].CanvasLeft -= 1;
                }
                else if (snake.SnakeBodyList[i - 1].LastCanvasTop > snake.SnakeBodyList[i].CanvasTop)
                {
                    snake.SnakeBodyList[i].CanvasTop += 1;
                }
                else if (snake.SnakeBodyList[i - 1].LastCanvasTop < snake.SnakeBodyList[i].CanvasTop)
                {
                    snake.SnakeBodyList[i].CanvasTop -= 1;
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
            if (TargetPosition[0] == game.cherry.CherryBody.CanvasLeft && TargetPosition[1] == game.cherry.CherryBody.CanvasTop)
            {
                game.MainWindowObject.Dispatcher.BeginInvoke((Action)(() =>
                {
                    snake.SnakeBodyList.Add(new MapSquere(snake));
                    game.copyOfSnake.SnakeCopyBodyList.Add(new MapSquere(snake, game.copyOfSnake));
                }));
                game.MainWindowObject.Score += 10;
                game.cherry.CherryBody.SetRandomCherryPosition(game);
                game.MainWindowObject.UpdateUI(game.cherry);
            }
            else if (CheckIfNextPositonIsSnake(TargetPosition) || game.MainWindowObject.Score >= 3960)
            {
                game.EndOfGame();
                Thread.CurrentThread.Abort();
            }
        }


        private bool CheckIfNextPositonIsSnake(int[] TargetPosition)
        {
            for (int i = 2; i < snake.SnakeBodyList.Count; i++)
            {
                if (TargetPosition[0] == snake.SnakeBodyList[i].CanvasLeft && TargetPosition[1] == snake.SnakeBodyList[i].CanvasTop)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
