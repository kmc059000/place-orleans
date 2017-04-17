using Place.Interfaces.Models;
using System;

namespace Place.Interfaces.Commands
{
    public class WritePixelCommand
    {
        public short X { get; set; }
        public short Y { get; set; }
        public DateTime Timestamp { get; set; }
        public string Author { get; set; }
        public Colors Color { get; set; }

        public int SliceKey => SliceHelpers.GetSliceKey(X, Y);
    }
}