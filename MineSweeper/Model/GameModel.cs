using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    [Serializable]
    public class GameModel : IGameModel
    {
        private Random random;
        private Settings settings;
        private Field[,] fields; // [row, column]

        private byte remainingMines;
        private TimeSpan elapsedTime;

        public Settings Settings { get { return settings; } }

        public GameModel()
        {
            random = new Random();
            settings = new Settings();
            fields = new Field[0,0];
        }

        public void NewGame()
        {
            fields = new Field[settings.GridWidth, settings.GridHeight];

            // Create fields
            for (int i = 0; i < fields.GetLength(0); i++)
                for (int j = 0; j < fields.GetLength(1); j++)
                    fields[i, j] = new Field();

            // Place mines
            int count = settings.NumberOfMines;
            while(count > 0){
                int r = random.Next(0, settings.GridWidth);
                int c = random.Next(0, settings.GridHeight);
                if (!fields[r, c].IsMine){
                    fields[r, c].IsMine = true;
                    count--;
                }
            }

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
                        case FieldState.CLEARED: {
                                if (fields[r, c].IsMine)
                                    newState = SharedStructs.FiledState.MINE;
                                else
                                    newState = SharedStructs.FiledState.CLEARED;
                                break; 
                            }
                    }
                    newFileds[r, c] = new SharedStructs.Field(newState, 0);
                }
            }
            return newFileds;
        }

        public void MarkField(int row, int column)
        {
            if (fields[row, column].State == FieldState.COVERED)
                fields[row, column].State = FieldState.MARKED;
            else if(fields[row, column].State == FieldState.MARKED)
                fields[row, column].State = FieldState.COVERED;
        }
    }
}
