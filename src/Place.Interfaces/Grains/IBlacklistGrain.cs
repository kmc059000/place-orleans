using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Place.Interfaces.Grains
{
    public interface IBlacklistGrain : IGrainWithIntegerKey
    {
        Task<IEnumerable<string>> GetBlacklist();
        Task<bool> IsBlacklisted(string author);
        Task Add(string author);
        Task Remove(string author);
    }
}
