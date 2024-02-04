using MineSweeper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    public delegate void TimeElapsedEventHandler();
    public delegate void LoseGameEventHandler();
    public delegate void WinGameEventHandler();

    public interface IGameModel
    {
        event TimeElapsedEventHandler OnTimeElapsed;
        event LoseGameEventHandler OnLoseGame;
        event WinGameEventHandler OnWinGame;

        // Inputs
        void CreateNewGame();
        void StartGame();
        void PauseGame();
        void StopGame();
        void MarkField(int row, int col);
        void RevealField(int row, int col);
        void ClearFieldsAround(int row, int col);

        // Outputs
        SharedStructs.Field[,] GetFields();
        string GetElapsedTime();
        int GetRemainingMines();
        SharedStructs.GameState GetGameState();

    }
}
