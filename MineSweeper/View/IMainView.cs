using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.View
{
    public delegate void WindowClosing();
    public interface IMainView
    {
        void Show();
        event WindowClosing OnWindowClosing;
    }
}
