using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SnakeNamespace;

namespace SnakeNamespace
{
    class Cherry : IUpdateUI
    {
        public MapSquere CherryBody;
        public SolidColorBrush Colour = new SolidColorBrush(Colors.Red);
        public List<MapSquere> GetBodyAsArrayList
        {
            get
            {
                List<MapSquere> Body = new List<MapSquere>
                {
                    CherryBody
                };
                return Body;
            }
        }

        public Cherry(Game game)
        {
            CherryBody = new MapSquere(game, this);
            game.MainWindowObject.UpdateUI(this);
        }
    }
}
