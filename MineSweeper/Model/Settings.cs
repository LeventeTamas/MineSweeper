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
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int NumberOfMines { get; set; }

        public Settings() { 
            GridWidth = 10;
            GridHeight = 10;
            NumberOfMines = 12;
        }

        public Settings(int gridWidth, int gridHeight, int numberOfMines)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            NumberOfMines = numberOfMines;
        }
    }
}
