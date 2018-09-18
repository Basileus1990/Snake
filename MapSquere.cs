using System;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media;

namespace SnakeNamespace
{
    class MapSquere
    {
        public Rectangle Squere = new Rectangle();
        public int CanvasLeft, CanvasTop, LastCanvasLeft, LastCanvasTop;

        //Create Head of Snake or Cherry and set it's position. Next fils it's with colour
        public MapSquere(Game game, Object SnakeOrCherryObject)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;

            if (SnakeOrCherryObject is Snake)
            {
                Squere.Fill = new SolidColorBrush(Colors.Blue);
                SetRandomSnakeHeadPosition(game);
            }
            else
            {
                Squere.Fill = new SolidColorBrush(Colors.Red);
                SetRandomCherryPosition(game);
            }
        }

        //Create next part of Snake Body and Sets it's position.  Next fils it's with colour
        public MapSquere(Snake snake)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;


            CanvasLeft = snake.SnakeBodyList[snake.SnakeBodyList.Count - 1].CanvasLeft;
            CanvasTop = snake.SnakeBodyList[snake.SnakeBodyList.Count - 1].CanvasTop;


            Squere.Fill = new SolidColorBrush(Colors.Blue);
        }

        //Create next part of VisualCopyOfSnake Body.
        public MapSquere(Snake snake, VisualCopyOfSnake copyOfSnake)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;

            Squere.Fill = new SolidColorBrush(Colors.Blue);
        }

        public void SetRandomSnakeHeadPosition(Game game)
        {
            int NumberOfCels = (game.GameCanvasSizeProp / Game.RectangleSize);

            Random r = new Random();
            CanvasLeft = r.Next(NumberOfCels) * Game.RectangleSize;
            CanvasTop = r.Next(NumberOfCels) * Game.RectangleSize;
        }
        public void SetRandomCherryPosition(Game game)
        {
            while (true)
            {
                int NumberOfCels = game.GameCanvasSizeProp / Game.RectangleSize;

                Random r = new Random();
                CanvasLeft = r.Next(NumberOfCels) * Game.RectangleSize;
                CanvasTop = r.Next(NumberOfCels) * Game.RectangleSize;

                //Checks if Cherry isn' created before Snake and throws error
                if (game.snake.SnakeBodyList.Count == 0)
                {
                    MessageBox.Show("Cherry is Created first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception();
                }
                //Checks if Cherry Positon is the same as Snake Positon. If so then again draws positon of Cherry
                else
                {
                    try
                    {
                        for (int i = 0; i < game.snake.SnakeBodyList.Count - 1; i++)
                        {
                            if (CanvasLeft == game.snake.SnakeBodyList[i].LastCanvasLeft && CanvasTop == game.snake.SnakeBodyList[i].LastCanvasTop ||
                                CanvasLeft == game.snake.SnakeBodyList[i].CanvasLeft && CanvasTop == game.snake.SnakeBodyList[i].CanvasTop)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return;
            }
        }
    }
}
