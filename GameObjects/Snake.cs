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
        private Game game;

        public SolidColorBrush Colour = new SolidColorBrush(Colors.Blue);
        public List<MapSquere> SnakeBodyList = new List<MapSquere>();
        public List<MapSquere> GetBodyAsList
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
    }
}