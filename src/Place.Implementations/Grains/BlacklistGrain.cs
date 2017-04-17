using Orleans;
using Place.Interfaces.Grains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Place.Implementations.Grains
{
    public class BlacklistGrain : Grain, IBlacklistGrain
    {
        private HashSet<string> _blacklist = new HashSet<string>();

        public Task Add(string author)
        {
            _blacklist.Add(author);
            return TaskDone.Done;
        }

        public Task<IEnumerable<string>> GetBlacklist()
        {
            return Task.FromResult(_blacklist.AsEnumerable());
        }

        public Task<bool> IsBlacklisted(string author)
        {
            return Task.FromResult(_blacklist.Contains(author));
        }

        public Task Remove(string author)
        {
            _blacklist.Remove(author);
            return TaskDone.Done;
        }
    }
}
