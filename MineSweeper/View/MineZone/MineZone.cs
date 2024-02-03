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
    public delegate void MarkFieldEventHandler(int row, int column);
    public delegate void RevealFieldEventHandler(int row, int column);
    public delegate void ClearFieldsAroundEventHandler(int row, int column);
    public class MineZone : Control
    {
        private MineZoneField[,] fields;
        public const int FIELD_SIZE = 40;

        // Create Images
        Bitmap imageCovered;
        Bitmap imageHighlighted;
        Bitmap imageMarked;
        Bitmap imageCleared;
        Bitmap imageMine;

        public event MarkFieldEventHandler OnMarkField;
        public event RevealFieldEventHandler OnRevealField;
        public event ClearFieldsAroundEventHandler OnClearFieldsAround;

        public MineZone()
        {
            this.DoubleBuffered = true;

            fields = new MineZoneField[0, 0];
            this.ResizeRedraw = true;
            this.MouseMove += MouseMoveHandler;
            this.MouseClick += MouseClickHandler;
            this.MouseDoubleClick += MouseDoubleClickHandler;

            // Create Images
            imageCovered = GraphicsHelper.CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.AliceBlue, Color.FromArgb(45, 128, 255));
            imageHighlighted = GraphicsHelper.CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.AliceBlue, Color.FromArgb(95, 178, 255));
            imageMarked = GraphicsHelper.CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.AliceBlue, Color.FromArgb(255, 178, 45));
            imageCleared = GraphicsHelper.CreateFieldImage(FIELD_SIZE, FIELD_SIZE, Color.LightGray, Color.DarkGray);
            imageMine = GraphicsHelper.CreateImageMine(imageCleared);
        }

        public void SetFileds(MineZoneField[,] fields)
        {
            this.fields = fields;
            this.Invalidate();
        }

        #region mouse events
        private void MouseDoubleClickHandler(object sender, MouseEventArgs e)
        {
            int[] selectedFieldCoords = GetSelectedFieldCoords(e.X, e.Y);
            OnClearFieldsAround(selectedFieldCoords[0], selectedFieldCoords[1]);
        }

        private void MouseClickHandler(object sender, MouseEventArgs e)
        {
            int[] selectedFieldCoords = GetSelectedFieldCoords(e.X, e.Y);
            switch (e.Button) {
                case MouseButtons.Right:{
                        OnMarkField(selectedFieldCoords[0], selectedFieldCoords[1]); // row, column
                        break;
                    }
                case MouseButtons.Left:
                    {
                        OnRevealField(selectedFieldCoords[0], selectedFieldCoords[1]);
                        break;
                    }
            }
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
                int fieldRow = mouseY / FIELD_SIZE;
                int fieldCol = mouseX / FIELD_SIZE;
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

            // Draw
            Graphics g = e.Graphics;

            // FIELDS
            for (int r = 0; r < rowNum; r++)
            {
                for (int c = 0; c < colNum; c++)
                {
                    int y = r * FIELD_SIZE;
                    int x = c * FIELD_SIZE;
                    Bitmap image = imageCovered;
                    switch (fields[r, c].FieldType)
                    {
                        case MineZoneFieldType.MARKED: { image = imageMarked; break; }
                        case MineZoneFieldType.REVEALED: { image = imageCleared; break; }
                        case MineZoneFieldType.MINE: { image = imageMine; break; }
                        case MineZoneFieldType.COVERED: {
                                if (fields[r, c].IsHighlighted)
                                    image = imageHighlighted;
                                else
                                    image = imageCovered;
                                break;
                            }
                    }
                    g.DrawImage(image, x, y, FIELD_SIZE, FIELD_SIZE);
                    if (fields[r, c].FieldType == MineZoneFieldType.REVEALED) {
                        Font font = new Font("Comic Sans MS", 12);
                        SizeF stringSize = GraphicsHelper.MeasureString(fields[r, c].Content, font);
                        g.DrawString(fields[r, c].Content, font, new SolidBrush(Color.Black), x + FIELD_SIZE/2 - stringSize.Width/2, y + FIELD_SIZE/2 - stringSize.Height/2);
                    }
                }
            }

            // GRID
            // Horizontal lines
            for (int r = 0; r < rowNum + 1; r++)
            {
                int y = r * FIELD_SIZE;
                g.DrawLine(new Pen(Color.Black), 0, y, e.ClipRectangle.Width, y);
            }
            // Vertical lines
            for (int c = 0; c < colNum + 1; c++)
            {
                int x = c * FIELD_SIZE;
                g.DrawLine(new Pen(Color.Black), x, 0, x, e.ClipRectangle.Height);
            }
            
        }
        
        #endregion
    }
}
