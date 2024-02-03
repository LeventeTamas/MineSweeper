using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.View
{
    public delegate void WindowClosingEventHandler();
    public delegate void MarkFieldEventHandler(int row, int col);
    public delegate void RevealFieldEventHandler(int row, int col);
    public delegate void ClearFieldsAroundEventHandler(int row, int col);
    public interface IMainView
    { 
        void Show();
        void UpdateView();
        event WindowClosingEventHandler OnWindowClosing;
        event MarkFieldEventHandler OnMarkField;
        event RevealFieldEventHandler OnRevealField;
        event ClearFieldsAroundEventHandler OnClearFieldsAround;
    }
}
