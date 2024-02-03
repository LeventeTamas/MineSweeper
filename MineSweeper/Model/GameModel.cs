using System;
using System.Collections.Generic;
using System.Linq;
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
            return newFileds;
        }
    }
}
