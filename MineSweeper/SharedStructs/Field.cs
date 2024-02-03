using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.SharedStructs
{
    public enum FiledState
    {
        COVERED,
        CLEARED,
        MARKED,
        MINE
    }
    public struct Field
    {
        private FiledState state;

        // Value: 0-8: number of mines around
        // Value is also 0 if this field hasn't been revealed yet
        private byte value;

        // Properties
        public FiledState State { get { return state; } }
        public byte Value {  get { return value; } }

        public Field(FiledState state, byte value)
        {
            this.state = state;
            this.value = value;
        }
    }
}
