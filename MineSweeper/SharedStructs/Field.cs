using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.SharedStructs
{
    public enum FieldState
    {
        COVERED,
        CLEARED,
        MARKED,
        MINE
    }
    public struct Field
    {
        private FieldState state;

        // Value: 0-8: number of mines around
        // Value is also 0 if this field hasn't been revealed yet
        private byte value;

        // Properties
        public FieldState State { get { return state; } }
        public byte Value {  get { return value; } }

        public Field(FieldState state, byte value)
        {
            this.state = state;
            this.value = value;
        }
    }
}
