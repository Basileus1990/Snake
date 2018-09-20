using System;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media;

namespace SnakeNamespace
{
    //Class sets single squere in Game Field
    class MapSquere
    {
        public Rectangle Squere = new Rectangle();
        public int CanvasLeft, CanvasTop, LastCanvasLeft, LastCanvasTop;

        //Create Head of Snake, set it's Height and Width. Next fils it's with colour
        public MapSquere(Game game, Snake snake)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;

            Squere.Fill = snake.Colour;
            SetRandomSnakeHeadPosition(game);
        }

        //Create Cherry, set it's Height and Width. Next fils it's with colour
        public MapSquere(Game game, Cherry cherry)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;

            Squere.Fill = cherry.Colour;
            SetRandomCherryPosition(game);
        }

        //Create next part of Snake Body and Sets it's position.  Next fils it's with colour
        public MapSquere(Snake snake)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;


            CanvasLeft = snake.SnakeBodyList[snake.SnakeBodyList.Count - 1].CanvasLeft;
            CanvasTop = snake.SnakeBodyList[snake.SnakeBodyList.Count - 1].CanvasTop;


            Squere.Fill = snake.Colour;
        }

        //Create next part of VisualCopyOfSnake Body.
        public MapSquere(Snake snake, VisualCopyOfSnake copyOfSnake)
        {
            Squere.Height = Game.RectangleSize;
            Squere.Width = Game.RectangleSize;

            Squere.Fill = copyOfSnake.Colour;
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

                if(CheckCherryPositionIsSnake(game.snake))
                {
                    continue;
                }
                return;
            }
        }

        private bool CheckCherryPositionIsSnake(Snake snake)
        {
            for (int i = 0; i < snake.SnakeBodyList.Count - 1; i++)
            {
                if (CanvasLeft == snake.SnakeBodyList[i].LastCanvasLeft && CanvasTop == snake.SnakeBodyList[i].LastCanvasTop ||
                    CanvasLeft == snake.SnakeBodyList[i].CanvasLeft && CanvasTop == snake.SnakeBodyList[i].CanvasTop)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
