namespace PalBet.Interfaces
{
    public interface IRewardService
    {

        public Task GetRewards(string UserId);
        public Task<string> GetTimeRemaining(string UserId);
    }
}
