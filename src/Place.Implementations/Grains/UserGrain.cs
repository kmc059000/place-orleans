using Orleans;
using Place.Interfaces.Grains;
using System;
using System.Threading.Tasks;
using Place.Interfaces.Commands;
using Place.Interfaces;

namespace Place.Implementations.Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private static readonly TimeSpan MaxWriteRate = TimeSpan.FromSeconds(Constants.WriteRateSeconds);

        private DateTime? LastWrite { get; set; }

        private int RateLimitedRequests = 0;

        public Task<string> GetUsername()
        {
            return Task.FromResult(this.GetPrimaryKeyString());
        }

        public async Task<bool> IsBlacklisted()
        {
            return await GrainFactory.GetGrain<IBlacklistGrain>(0).IsBlacklisted(await GetUsername());
        }

        public Task<bool> IsRateLimited()
        {
            var isRateLimited = LastWrite.HasValue && LastWrite.Value.Add(MaxWriteRate) > DateTime.UtcNow;

            if(isRateLimited)
            {
                RateLimitedRequests++;
            }
            else
            {
                RateLimitedRequests = 0;
            }

            return Task.FromResult(isRateLimited);
        }

        public async Task WritePixel(WritePixelCommand command)
        {
            if(await IsRateLimited() || await IsBlacklisted())
            {
                //throttle user
                await Task.Delay(TimeSpan.FromSeconds(RateLimitedRequests));

                return;
            }

            var slice = GrainFactory.GetGrain<IPixelSliceGrain>(command.SliceKey);

            await slice.WritePixel(command);

            LastWrite = DateTime.UtcNow;
        }
    }
}
