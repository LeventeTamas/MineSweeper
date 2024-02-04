using MineSweeper.Model;
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
    public delegate void SaveGameEventHandler(string path);
    public delegate void LoadSavedGameEventHandler(string path);
    public interface IMainView
    {
        event WindowClosingEventHandler OnWindowClosing;
        event MarkFieldEventHandler OnMarkField;
        event RevealFieldEventHandler OnRevealField;
        event ClearFieldsAroundEventHandler OnClearFieldsAround;
        event NewGameEventHandler OnNewGame;
        event PauseGameEventHandler OnPauseGame;
        event SaveGameEventHandler OnSaveGame;
        event LoadSavedGameEventHandler OnLoadSavedGame;

        void SetGameModel(IGameModel gameModel);
        void Show();
        void UpdateView();
        void UpdateTime();
        void LoseGame();
        void WinGame();
        object Invoke(Delegate method);

    }
}
