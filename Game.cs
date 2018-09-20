using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System;
using SnakeNamespace;

namespace SnakeNamespace
{

    class Game
    {
        public Canvas GameField;
        public MainWindow MainWindowObject;
        public Snake snake;
        public Cherry cherry;
        public VisualCopyOfSnake copyOfSnake;
        public Thread moveThread;
        public const int RectangleSize = 15;

        private readonly object PadLock = new object();
        private int GameCanvasSize;
        public int GameCanvasSizeProp
        {
            get
            {
                return GameCanvasSize;
            }
            set
            {
                //Checks if Height of GameCanvas devided by Rectangle Size is intiger or GameCanvasSize isn't arledy set. If so, then Shows Error message and Trhow Excepction
                if (GameCanvasSize != 0 || value / RectangleSize != value / RectangleSize)
                {
                    MessageBox.Show("Height of GameField isn't devisible by Rectangle Size", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception();
                }
                else
                {
                    GameCanvasSize = value;
                }
            }
        }

        public Game(Canvas gameField, MainWindow mainWindow)
        {
            GameField = gameField;
            GameCanvasSizeProp = (int)GameField.Height;
            MainWindowObject = mainWindow;

            snake = new Snake(this);
            cherry = new Cherry(this);
            copyOfSnake = new VisualCopyOfSnake(snake, this);

            CreateMoveThread();
        }

        public void CreateMoveThread()
        {
            moveThread = new Thread(new ThreadStart(() => snake.Move(MainWindowObject)))
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
                snake.AbortMoveThread = true;
            }
        }

        public void EndOfGame()
        {
            MainWindowObject.Dispatcher.Invoke((() =>
            {
                if(MainWindowObject.Score > MainWindowObject.BestScore)
                {
                    MainWindowObject.BestScore = MainWindowObject.Score;
                }

                MainWindowObject.MainButton.Content = "RESTART";
            }));
        }
    }
}
