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
    public partial class GameOverMessageBox : Form
    {
        public GameOverMessageBox(string gameResultText)
        {
            InitializeComponent();

            lbGameResult.Text = gameResultText;

            // Placing text to the center
            SizeF textSize = MeasureString(lbGameResult.Text, lbGameResult.Font);
            int locationX = (int)Math.Round(this.Width / 2 - textSize.Width / 2);
            int locationY = lbGameResult.Location.Y;
            lbGameResult.Location = new Point(locationX, locationY);

            this.DialogResult = DialogResult.OK;
        }

        private SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }

            return result;
        }
    }
}
