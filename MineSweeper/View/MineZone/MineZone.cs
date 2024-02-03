using MineSweeper.SharedStructs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.View.MineZone
{
    public delegate void MarkFieldHandler(int row, int column);
    public class MineZone : Control
    {
        private MineZoneField[,] fields;
        public const int FIELD_SIZE = 30;

        public event MarkFieldHandler OnMarkField;

        public MineZone()
        {
            fields = new MineZoneField[0, 0];
            this.ResizeRedraw = true;
            this.MouseMove += MouseMoveHandler;
            this.MouseClick += MouseClickHandler;
        }

        public void SetFileds(MineZoneField[,] fields)
        {
            this.fields = fields;
        }

        #region mouse events
        private void MouseClickHandler(object sender, MouseEventArgs e)
        {
            int[] selectedFieldCoords = GetSelectedFieldCoords(e.X, e.Y);
            if(e.Button == MouseButtons.Right)
                OnMarkField(selectedFieldCoords[0], selectedFieldCoords[1]); // row, column
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            int[] selectedFieldCoords = GetSelectedFieldCoords(e.X, e.Y);
            if (selectedFieldCoords != null)
            {
                MineZoneField selectedField = fields[selectedFieldCoords[0], selectedFieldCoords[1]];
                if (!selectedField.IsHighlighted)
                {
                    // change every field with COVERED_HOVER state to COVERED
                    for (int r = 0; r < fields.GetLength(0); r++)
                        for (int c = 0; c < fields.GetLength(1); c++)
                            fields[r, c].IsHighlighted = false;

                    // Highlight the field the mouse is over
                    selectedField.IsHighlighted = true;

                    this.Invalidate();
                }
            }
        } 

        private int[] GetSelectedFieldCoords(int mouseX, int mouseY)
        {
            int[] coords = null;

            int rowNum = fields.GetLength(0);
            int colNum = fields.GetLength(1);
            if (rowNum > 0 && colNum > 0)
            {
                // Which field is the mouse over?
                int fieldRow = mouseX / FIELD_SIZE;
                int fieldCol = mouseY / FIELD_SIZE;
                if (fieldRow < rowNum && fieldCol < colNum)
                    coords = new int[] { fieldRow, fieldCol };
            }

            return coords;
        }
        #endregion

        #region Render
        protected override void OnPaint(PaintEventArgs e)
        {
            int rowNum = fields.GetLength(0);
            int colNum = fields.GetLength(1);

            // Create Images
            Bitmap imageCovered = CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.AliceBlue, Color.FromArgb(45, 128, 255));
            Bitmap imageHover = CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.AliceBlue, Color.FromArgb(255, 239, 45));
            Bitmap imageMarked = CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.FromArgb(45, 255, 128), Color.DarkGreen);
            Bitmap imageCleared = CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.LightGray, Color.DarkGray);
            Bitmap imageMine = CreateImageMine(imageCleared);

            // Draw
            Graphics g = e.Graphics;
            
            g.Clear(Color.White);

            // FIELDS
            for (int r = 0; r < rowNum; r++)
            {
                for (int c = 0; c < colNum; c++)
                {
                    int y = c * FIELD_SIZE;
                    int x = r * FIELD_SIZE;
                    Bitmap image = imageCovered;
                    switch(fields[r, c].FieldType)
                    {
                        case MineZoneFieldType.MARKED:{ image = imageMarked; break; }
                        case MineZoneFieldType.CLEARED: { image = imageCleared; break; }
                        case MineZoneFieldType.MINE: { image = imageMine; break; }
                        case MineZoneFieldType.COVERED: {
                                if (fields[r, c].IsHighlighted)
                                    image = imageHover;
                                else
                                    image = imageCovered;
                                break; 
                            }
                    }
                    g.DrawImage(image, x, y, FIELD_SIZE, FIELD_SIZE);
                }
            }

            // GRID
            // Horizontal lines
            for (int r = 0; r < rowNum+1; r++)
            {
                int y = r * FIELD_SIZE;
                g.DrawLine(new Pen(Color.Black), 0, y, e.ClipRectangle.Width, y);
            }
            // Vertical lines
            for (int c = 0; c < colNum+1; c++)
            {
                int x = c * FIELD_SIZE;
                g.DrawLine(new Pen(Color.Black), x, 0, x, e.ClipRectangle.Height);
            }
        }

        private Bitmap CreateFieldImage(int width, int height, Color color1, Color color2)
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

        private Bitmap CreateImageMine(Bitmap background)
        {
            Bitmap bitmap = new Bitmap(background.Width, background.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(background, 0, 0, background.Width, background.Height);
                g.DrawImage(Properties.Resources.mine, 0, 0, background.Width, background.Height);
            }
            return bitmap;
        }
        #endregion
    }
}
