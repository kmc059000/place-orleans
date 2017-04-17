using System;

namespace Place.Interfaces
{
    public class Constants
    {
        public const int Width = 1000;
        public const int Height = 1000;
        public const int WriteRateSeconds = 0;
        public const int SliceSize = 1000;
        public const int TotalPixels = Width * Height;
    }

    public static class SliceHelpers
    {
        public static int GetSliceKey(int x, int y)
        {
            return (x + (y * Constants.Width)) / Constants.SliceSize;
        }

        public static int GetOffset(int sliceKey)
        {
            return (sliceKey * Constants.SliceSize);
        }

        public static int GetIndex(int x, int y)
        {
            return x + y * Constants.Width;
        }
    }
}
