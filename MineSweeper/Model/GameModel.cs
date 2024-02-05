using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MineSweeper.Model
{

    [Serializable]
    public class GameModel : IGameModel
    {
        private enum GameState
        {
            ONGOING,
            PAUSED,
            STOPPED
        }

        private Random random;
        private Settings settings;
        private Field[,] fields; // [row, column]

        private int remainingMines;
        private long elapsedSeconds;
        private GameState gameState;

        [field: NonSerialized]
        public event LoseGameEventHandler OnLoseGame;
        [field: NonSerialized]
        public event WinGameEventHandler OnWinGame;

        public Settings Settings { get { return settings; } }

        public GameModel()
        {
            random = new Random();
            settings = new Settings();
            fields = new Field[0,0];
            gameState = GameState.STOPPED;
        }

        public void TimerTick()
        {
            if(gameState == GameState.ONGOING)
                elapsedSeconds++;
        }

        public void CreateNewGame()
        {
            fields = new Field[settings.NumberOfRows, settings.NumOfColumns];

            // Create fields
            for (int r = 0; r < fields.GetLength(0); r++)
                for (int c = 0; c < fields.GetLength(1); c++)
                    fields[r, c] = new Field();

            // Place mines
            int count = settings.NumberOfMines;
            while(count > 0){
                int r = random.Next(0, settings.NumberOfRows);
                int c = random.Next(0, settings.NumOfColumns);
                if (!fields[r, c].IsMine){
                    fields[r, c].IsMine = true;
                    count--;
                }
            }

            // Count mines around each field
            for (int r = 0; r < fields.GetLength(0); r++)
                for (int c = 0; c < fields.GetLength(1); c++)
                    fields[r, c].MinesAround = CountMinesAroundField(r, c);

            remainingMines = settings.NumberOfMines;
            elapsedSeconds = 0;
        }

        public void ResetGame()
        {
            remainingMines = settings.NumberOfMines;
            elapsedSeconds = 0;

            // Reset fields
            for (int r = 0; r < fields.GetLength(0); r++)
                for (int c = 0; c < fields.GetLength(1); c++)
                    fields[r, c].State = FieldState.COVERED;
        }

        public void StartGame()
        {
            gameState = GameState.ONGOING;
        }

        public void PauseGame()
        {
            gameState = GameState.PAUSED;
        }

        public void StopGame()
        {
            gameState = GameState.STOPPED;
        }

        public string GetElapsedTime()
        {
            TimeSpan elapsedTime = TimeSpan.FromSeconds(elapsedSeconds);
            return elapsedTime.Minutes.ToString("d2") + ":" + elapsedTime.Seconds.ToString("d2");
        }

        public int GetRemainingMines()
        {
            return remainingMines;
        }

        public SharedStructs.Field[,] GetFields()
        {
            SharedStructs.Field[,] newFileds = new SharedStructs.Field[fields.GetLength(0), fields.GetLength(1)];

            for (int r = 0; r < fields.GetLength(0); r++) {
                for (int c = 0; c < fields.GetLength(1); c++) {
                    SharedStructs.FiledState newState = SharedStructs.FiledState.COVERED;
                    switch (fields[r, c].State)
                    {
                        case FieldState.MARKED: { newState = SharedStructs.FiledState.MARKED; break; }
                        case FieldState.REVEALED: {
                                if (fields[r, c].IsMine)
                                    newState = SharedStructs.FiledState.MINE;
                                else
                                    newState = SharedStructs.FiledState.CLEARED;
                                break; 
                            }
                    }

                    newFileds[r, c] = new SharedStructs.Field(newState, fields[r, c].MinesAround);
                }
            }
            return newFileds;
        }

        public SharedStructs.GameState GetGameState()
        {
            // Convert GameState to SharedStructs.GameState
            switch (gameState)
            {
                case GameState.PAUSED: 
                    return SharedStructs.GameState.PAUSED; 
                case GameState.STOPPED: 
                    return SharedStructs.GameState.STOPPED;
                default:
                    return SharedStructs.GameState.ONGOING;
            }
        }

        private byte CountMinesAroundField(int row, int col)
        {
            byte mines = 0;

            int rowStart = Math.Max(row - 1, 0);
            int colStart = Math.Max(col - 1, 0);
            int rowEnd = Math.Min(row + 1, fields.GetLength(0)-1);
            int colEnd = Math.Min(col + 1, fields.GetLength(1)-1);

            for (int r = rowStart; r <= rowEnd; r++)
                for (int c = colStart; c <= colEnd; c++)
                    if ((r != row || c != col) && fields[r, c].IsMine) mines++;

            return mines;
        }

        private byte CountMarkedFieldsAround(int row, int col)
        {
            byte marked = 0;

            int rowStart = Math.Max(row - 1, 0);
            int colStart = Math.Max(col - 1, 0);
            int rowEnd = Math.Min(row + 1, fields.GetLength(0) - 1);
            int colEnd = Math.Min(col + 1, fields.GetLength(1) - 1);

            for (int r = rowStart; r <= rowEnd; r++)
                for (int c = colStart; c <= colEnd; c++)
                    if ((r != row || c != col) && fields[r, c].State == FieldState.MARKED) marked++;

            return marked;
        }

        public void MarkField(int row, int col)
        {
            if (fields[row, col].State == FieldState.COVERED && remainingMines > 0){
                fields[row, col].State = FieldState.MARKED;
                remainingMines--;
            }
            else if (fields[row, col].State == FieldState.MARKED){
                fields[row, col].State = FieldState.COVERED;
                remainingMines++;
            }
        }

        public void RevealField(int row, int col)
        {
            if (fields[row, col].State != FieldState.MARKED && fields[row, col].State != FieldState.REVEALED){
                // Set this field to revealed
                fields[row, col].State = FieldState.REVEALED;

                // If it was a mine, then the player lose
                if (fields[row, col].IsMine){
                    LoseGame();
                    return;
                }

                // Check if the player won the game
                // If the number of unrevealed fields are equal to the number of mines
                int numOfUnrevealed = (from Field field in fields 
                                       where field.State != FieldState.REVEALED
                                       select field).Count();
                if(numOfUnrevealed == Settings.NumberOfMines){
                    WinGame();
                    return;
                }

                // If this field has no mines around it, than we call the RevealField method recursively for all surrounding fields
                if (fields[row, col].MinesAround == 0){
                    int rowStart = Math.Max(row - 1, 0);
                    int colStart = Math.Max(col - 1, 0);
                    int rowEnd = Math.Min(row + 1, fields.GetLength(0) - 1);
                    int colEnd = Math.Min(col + 1, fields.GetLength(1) - 1);

                    for (int r = rowStart; r <= rowEnd; r++)
                        for (int c = colStart; c <= colEnd; c++)
                                RevealField(r, c);
                }
            }
        }

        public void ClearFieldsAround(int row, int col)
        {
            // If the fields is already revealed and has at least one mine around it
            if (fields[row, col].State == FieldState.REVEALED && fields[row, col].MinesAround > 0)
            {
                byte markedAround = CountMarkedFieldsAround(row, col);
                byte minesAround = fields[row, col].MinesAround;

                // If the number of mines around this field equal to the number of marked fields around, then we call RevealField recursive method
                if (markedAround == minesAround)
                {
                    int rowStart = Math.Max(row - 1, 0);
                    int colStart = Math.Max(col - 1, 0);
                    int rowEnd = Math.Min(row + 1, fields.GetLength(0) - 1);
                    int colEnd = Math.Min(col + 1, fields.GetLength(1) - 1);

                    for (int r = rowStart; r <= rowEnd; r++)
                        for (int c = colStart; c <= colEnd; c++)
                            RevealField(r, c);
                }
                    
            }
        }

        private void LoseGame()
        {
            StopGame();

            // event call: Notify player
            OnLoseGame();
        }
        private void WinGame()
        {
            StopGame();

            // event call: Notify player
            OnWinGame();
        }

    }
}
