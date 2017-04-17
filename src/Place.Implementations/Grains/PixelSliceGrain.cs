using Orleans;
using Place.Interfaces.Grains;
using System;
using System.Threading.Tasks;
using Place.Interfaces.Commands;
using Place.Interfaces;
using Place.Interfaces.Models;
using Orleans.Concurrency;

namespace Place.Implementations.Grains
{
    public class PixelSliceGrain : Grain, IPixelSliceGrain
    {
        private PixelSlice _slice;

        private short Offset => SliceHelpers.GetOffset((int)this.GetPrimaryKeyLong());

        public override Task OnActivateAsync()
        {
            _slice = new PixelSlice()
            {
                Offset = Offset,
                Pixels = new Pixel[Constants.SliceSize]
            };

            for(var i = 0; i < Constants.SliceSize; i++)
            {
                var index = Offset + i;

                _slice.Pixels[i] = new Pixel()
                {
                    Index = index,
                    X = (short)(index % Constants.Width),
                    Y = (short)(index / Constants.Width),
                    Color = Colors.White,
                    LastAuthor = null,
                    LastTimestamp = null
                };
            }

            return TaskDone.Done;
        }

        public Task<PixelSlice> Get()
        {
            return Task.FromResult(_slice);
        }

        public Task WritePixel(WritePixelCommand command)
        {
            var index = SliceHelpers.GetIndex(command.X, command.Y);
            var relativeIndex = index = Offset;

            var pixel = _slice.Pixels[relativeIndex];

            //only replace pixel if new command is newer.
            if (command.Timestamp >= pixel.LastTimestamp)
            {
                pixel.Color = command.Color;
                pixel.LastAuthor = command.Author;
                pixel.LastTimestamp = command.Timestamp;
            }

            return TaskDone.Done;
        }
    }
}
