using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.View
{
    public delegate void WindowClosingEventHandler();
    public delegate void MarkFieldEventHandler(int row, int col);
    public delegate void RevealFieldEventHandler(int row, int col);
    public delegate void ClearFieldsAroundEventHandler(int row, int col);
    public delegate void NewGameEventHandler();
    public delegate void PauseGameEventHandler();
    public interface IMainView
    {
        event WindowClosingEventHandler OnWindowClosing;
        event MarkFieldEventHandler OnMarkField;
        event RevealFieldEventHandler OnRevealField;
        event ClearFieldsAroundEventHandler OnClearFieldsAround;
        event NewGameEventHandler OnNewGame;
        event PauseGameEventHandler OnPauseGame;
        void Show();
        void UpdateView();
        void UpdateTime();
        void LoseGame();
        void WinGame();
        object Invoke(Delegate method);

    }
}
