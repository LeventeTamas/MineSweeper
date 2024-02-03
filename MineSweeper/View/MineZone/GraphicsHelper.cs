using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.View.MineZone
{
    public class GraphicsHelper
    {
        public static SizeF MeasureString(string s, Font font)
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

        public static Bitmap CreateFieldImage(int width, int height, Color color1, Color color2)
        {
            Bitmap bitmap = new Bitmap(width, height);

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(0, 0, width, height));

            PathGradientBrush brush = new PathGradientBrush(path);
            brush.CenterColor = color1;
            brush.SurroundColors = new Color[] { color2 };

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(brush, 0, 0, width, height);
            }

            return bitmap;
        }

        public static Bitmap CreateImageMine(Bitmap background)
        {
            Bitmap bitmap = new Bitmap(background.Width, background.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(background, 0, 0, background.Width, background.Height);
                int mineImageWidth = background.Width - 10;
                int mineImageHeight = background.Height - 10;
                g.DrawImage(Properties.Resources.mine, background.Width/2 - mineImageWidth/2, background.Height/2 - mineImageHeight/2, mineImageWidth, mineImageHeight);
            }
            return bitmap;
        }
    }
}
