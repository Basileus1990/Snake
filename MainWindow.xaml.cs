using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;


namespace SnakeNamespace
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;
        public static Canvas GameField;

        private Key KeyBuffer;
        public Key SetKeyBuffer
        {
            set
            {
                KeyBuffer = value;
            }
        }

        private int ScoreValue = 0; 
        public int Score
        {
            get
            {
                return ScoreValue;
            }
            set
            {
                Dispatcher.Invoke((Action)(() => ScoreLabel.Content = value));
                ScoreValue = value;
            }
        }

        private int BestScoreValue = 0;
        public int BestScore
        {
            get
            {
                return BestScoreValue;
            }
            set
            {
                StreamWriter streamWriter = new StreamWriter(new FileStream("BestScoreBuffer.txt", FileMode.OpenOrCreate));
                streamWriter.WriteLine(value);

                streamWriter.Close();

                Dispatcher.Invoke((Action)(() => BestScoreLabel.Content = value));
                BestScoreValue = value;
            }
        }

        public void UpdateBestScore()
        {
            StreamReader streamReader = new StreamReader(new FileStream("BestScoreBuffer.txt", FileMode.OpenOrCreate));

            try
            {
                BestScoreValue = int.Parse(streamReader.ReadLine());
            }
            catch(ArgumentNullException)
            {
                BestScoreValue = 0;
            }
            Dispatcher.Invoke((Action)(() => BestScoreLabel.Content = BestScoreValue));

            streamReader.Close();
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content)
            {
                case "START":
                    {
                        (sender as Button).Content = "STOP";
                        game = new Game(GameField, this);
                        break;
                    }
                case "STOP":
                    {
                        game.EndMoveThread();
                        SetKeyBuffer = Key.None;

                        RestartButton.Visibility = Visibility.Visible;
                        (sender as Button).Content = "CONTINUE";
                        break;
                    }
                case "CONTINUE":
                    {
                        game.CreateMoveThread();

                        RestartButton.Visibility = Visibility.Hidden;
                        (sender as Button).Content = "STOP";
                        break;
                    }
                case "RESTART":
                    {
                        RestartButton_Click(new object(), new RoutedEventArgs());
                        break;
                    }
            }

            //Restains button form changing colout when focused 
            //(sender as Button).Focusable = false;
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            RestartButton.Visibility = Visibility.Hidden;

            if (Score > BestScore)
            {
                BestScore = Score;
            }
            Score = 0;

            GameField.Children.Clear();
            MainButton.Content = "START";
        }

        public void UpdateUI(object GameObject)
        {
            IUpdateUI UIobject;
            if (GameObject is Snake)
                UIobject = (Snake)GameObject;
            else if (GameObject is Cherry)
                UIobject = (Cherry)GameObject;
            else
                UIobject = (VisualCopyOfSnake)GameObject;

            List<MapSquere> Body = UIobject.GetBody;
            Dispatcher.BeginInvoke((Action)(() => 
            {
                for (int i = 0; i < Body.Count; i++)
                {
                    if(GameField.Children.Contains(Body[i].Squere))
                    {
                        GameField.Children.Remove(Body[i].Squere);
                    }
                    GameField.Children.Add(Body[i].Squere);

                    Canvas.SetLeft(Body[i].Squere, Body[i].CanvasLeft);
                    Canvas.SetTop(Body[i].Squere, Body[i].CanvasTop);
                }
            }));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyBuffer == e.Key || game == null || !game.moveThread.IsAlive)
            {
                return;
            }
            game.snake.ChangeDirectionOfMoving(e.Key);
            KeyBuffer = e.Key;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBestScore();
            GameField = GameCanvas;
        }
    }
}
