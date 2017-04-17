using Orleans;
using Place.Interfaces.Commands;
using System.Threading.Tasks;

namespace Place.Interfaces.Grains
{
    public interface IUserGrain : IGrainWithStringKey
    {
        Task<string> GetUsername();
        Task WritePixel(WritePixelCommand command);
        Task<bool> IsRateLimited();
        Task<bool> IsBlacklisted();
    }
}
