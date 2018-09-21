using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SnakeNamespace
{
    class VisualCopyOfSnake : IUpdateUI
    {
        private Snake snake;
        private Game game;

        public SolidColorBrush Colour = new SolidColorBrush(Colors.Blue);
        public List<MapSquere> SnakeCopyBodyList = new List<MapSquere>();
        public List<MapSquere> GetBodyAsList
        {
            get
            {
                return SnakeCopyBodyList;
            }
        }

        public VisualCopyOfSnake(Snake snake, Game game) //Create Visual Copy of Snake
        {
            this.game = game;
            this.snake = snake;
            for (int i = 0; i < snake.SnakeBodyList.Count - 1; i++)
            {
                SnakeCopyBodyList.Add(new MapSquere(snake, this));
            }
        }

        public void UpdateCopyOfSnake()
        {
            for (int i = 0; i < SnakeCopyBodyList.Count; i++)
            {
                SnakeCopyBodyList[i].CanvasLeft = snake.SnakeBodyList[i].CanvasLeft;
                SnakeCopyBodyList[i].CanvasTop = snake.SnakeBodyList[i].CanvasTop;
            }
            game.MainWindowObject.UpdateUI(this);
        }
    }
}
