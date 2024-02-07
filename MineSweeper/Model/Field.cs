using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.Model
{
    public enum FieldState
    {
        COVERED,
        MARKED,
        REVEALED
    }

    [Serializable]
    public class Field
    {
        public bool IsMine;         // Indicates whether this field contains a mine
        public FieldState State;    
        public byte MinesAround;    // Number of mines around this field

        public Field()
        {
            IsMine = false;
            State = FieldState.COVERED;
            MinesAround = 0;
        }
    }
}
