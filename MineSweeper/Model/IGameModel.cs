using MineSweeper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    public delegate void TimeElapsedEventHandler();
    public interface IGameModel
    {
        void NewGame();
        SharedStructs.Field[,] GetFields();
        void MarkField(int row, int col);
        void RevealField(int row, int col);
        void ClearFieldsAround(int row, int col);
        string GetElapsedTime();
        int GetRemainingMines();
        event TimeElapsedEventHandler OnTimeElapsed;
    }
}
