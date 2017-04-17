using System;

namespace Place.Interfaces.Models
{
    public class Pixel
    {
        public int Index { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public Colors Color { get; set; }
        public string LastAuthor { get; set; }
        public DateTime? LastTimestamp{ get; set; }
    }
}