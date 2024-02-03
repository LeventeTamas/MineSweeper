using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    [Serializable]
    public class Settings
    {
        public int NumOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfMines { get; set; }

        public Settings() { 
            NumOfColumns = 10;
            NumberOfRows = 10;
            NumberOfMines = 15;
        }

        public Settings(int numberOfColumns, int numberOfRows, int numberOfMines)
        {
            NumOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
            NumberOfMines = numberOfMines;
        }
    }
}
