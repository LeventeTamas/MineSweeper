using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    [Serializable]
    public class Settings
    {
        private static readonly string SETTINGS_PATH = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "settings.ini");

        private int numberOfColumns;
        private int numberOfRows;
        private int numberOfMines;

        public int NumberOfColumns
        {
            get { return numberOfColumns; }
            set { numberOfColumns = value; }
        }

        public int NumberOfRows
        {
            get { return numberOfRows; }
            set { numberOfRows = value; } 
        }

        public int NumberOfMines
        {
            get { return numberOfMines; }
            set { numberOfMines = value; }
        }

        public Settings() { 
            numberOfColumns = 10;
            numberOfRows = 10;
            numberOfMines = 15;
        }

        public Settings(int numberOfColumns, int numberOfRows, int numberOfMines)
        {
            this.numberOfColumns = numberOfColumns;
            this.numberOfRows = numberOfRows;
            this.numberOfMines = numberOfMines;
        }

        public void ChangeSettings(SharedStructs.Settings newSettings)
        {
            this.numberOfRows = newSettings.NumOfRows;
            this.numberOfColumns = newSettings.NumOfColumns;
            this.numberOfMines = newSettings.NumOfMines;
        }

        public void Save()
        {
            if (!File.Exists(SETTINGS_PATH))
                File.Create(SETTINGS_PATH);

            // Write settings file
            IOManager.IniWriteValue(SETTINGS_PATH, "GAME", "NUMBER_OF_ROWS", NumberOfRows.ToString());
            IOManager.IniWriteValue(SETTINGS_PATH, "GAME", "NUMBER_OF_COLUMNS", NumberOfColumns.ToString());
            IOManager.IniWriteValue(SETTINGS_PATH, "GAME", "NUMBER_OF_MINES", NumberOfMines.ToString());
        }

        public static Settings Load()
        {
            // Sreating settings object with default values
            Settings settings = new Settings(10, 10, 15); 

            if (File.Exists(SETTINGS_PATH))
            {
                // Read the settings file
                settings.NumberOfRows = Convert.ToInt32(IOManager.IniReadValue(SETTINGS_PATH, "GAME", "NUMBER_OF_ROWS"));
                settings.NumberOfColumns = Convert.ToInt32(IOManager.IniReadValue(SETTINGS_PATH, "GAME", "NUMBER_OF_COLUMNS"));
                settings.NumberOfMines = Convert.ToInt32(IOManager.IniReadValue(SETTINGS_PATH, "GAME", "NUMBER_OF_MINES"));
            }

            return settings;
        }
    }
}
