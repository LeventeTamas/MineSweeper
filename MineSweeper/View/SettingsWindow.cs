using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.View
{
    public partial class SettingsWindow : Form
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int Mines { get; set; }
        
        public SettingsWindow()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        public SettingsWindow(int rows, int cols, int mines) : this()
        {
            this.Rows = rows;
            this.Cols = cols;
            this.Mines = mines;

            this.nudRows.Value = rows;
            this.nudCols.Value = cols;
            this.nudMines.Maximum = Rows * Cols;
            this.nudMines.Value = Math.Min(mines, this.nudMines.Maximum);

            nudRows.ValueChanged += ValueChanged;
            nudCols.ValueChanged += ValueChanged;
            nudMines.ValueChanged += ValueChanged;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            Rows = (int)nudRows.Value;
            Cols = (int)nudCols.Value;
            nudMines.Maximum = Rows * Cols;
            Mines = (int)nudMines.Value;
        }
    }
}
