using Orleans;
using Place.Interfaces.Commands;
using Place.Interfaces.Models;
using System.Threading.Tasks;

namespace Place.Interfaces.Grains
{
    public interface IPixelSliceGrain : IGrainWithIntegerKey
    {
        Task WritePixel(WritePixelCommand command);
        Task<PixelSlice> Get();
    }
}
