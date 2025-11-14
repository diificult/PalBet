using PalBet.Interfaces;
using PalBet.Repository;

namespace PalBet.Services
{
    public class RewardService : IRewardService
    {
        public readonly IAppUserRepository _appUserRepository;
        public RewardService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task GetRewards(string UserId)
        {
            var user = await _appUserRepository.GetUserAsync(UserId);

            Console.WriteLine(user.DailyRewardLastClaim + ", " + (user.DailyRewardLastClaim - DateTime.UtcNow) + ", " + TimeSpan.FromHours(24) + ", "+ (user.DailyRewardLastClaim - DateTime.UtcNow < TimeSpan.FromHours(24)));

            if (user.DailyRewardLastClaim.AddHours(24) - DateTime.UtcNow > TimeSpan.Zero) 
            {
                throw new Exception("Daily reward already claimed. Please wait before claiming again.");
            }
            user.DailyRewardLastClaim = DateTime.UtcNow;
            await _appUserRepository.UpdateCoins(UserId, 20);

        }

        public async Task<string> GetTimeRemaining(string UserId)
        {
            var user = await _appUserRepository.GetUserAsync(UserId);
            Console.WriteLine(user.DailyRewardLastClaim);
            var timeSpan = user.DailyRewardLastClaim.AddHours(24) - DateTime.UtcNow;
            Console.WriteLine(user.DailyRewardLastClaim.AddHours(24) + ", " + DateTime.UtcNow + ", " + timeSpan);
            if (timeSpan < TimeSpan.Zero) return "00:00:00";
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
