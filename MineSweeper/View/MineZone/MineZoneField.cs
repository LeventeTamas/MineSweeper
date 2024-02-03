using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.View.MineZone
{
    public enum MineZoneFieldType
    {
        COVERED,
        REVEALED,
        MARKED,
        MINE
    }

    public class MineZoneField
    {
        public MineZoneFieldType FieldType { get; set; }
        public string Content { get; set; }
        public bool IsHighlighted { get; set; }

        public MineZoneField(MineZoneFieldType fieldType, string content = "")
        {
            IsHighlighted = false;
            FieldType = fieldType;
            Content = content;
        }
    }
}
