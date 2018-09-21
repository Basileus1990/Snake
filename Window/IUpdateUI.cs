using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeNamespace
{
    interface IUpdateUI
    {
        List<MapSquere> GetBodyAsArrayList { get; } //Used only for Update of UI
    }
}
