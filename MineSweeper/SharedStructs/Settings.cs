using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.SharedStructs
{
    public struct Settings
    {
        public int NumOfRows {  get; set; }
        public int NumOfColumns { get; set;}
        public int NumOfMines { get; set; }

        public Settings(int numOfRows, int numOfColumns, int numOfMines)
        {
            this.NumOfRows = numOfRows;
            this.NumOfColumns = numOfColumns;
            this.NumOfMines = numOfMines;
        }
    }
}
