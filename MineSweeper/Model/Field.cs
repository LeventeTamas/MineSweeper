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
        OPENED
    }

    [Serializable]
    public class Field
    {
        public bool IsMine;
        public FieldState State;

        public Field()
        {
            IsMine = false;
            State = FieldState.COVERED;
        }

        public Field(bool isMine, FieldState state = FieldState.COVERED)
        {
            IsMine = isMine;
            State = state;
        }
    }
}
