using PalBet.Dtos.Bet;

namespace PalBet.Interfaces
{
    public interface IRedisBetsCacheService
    {
        Task<List<BetDto>?> GetBetsByStateAsync(string userId, string state);
        Task SetBetsByStateAsync(string userId, string state, List<BetDto> bets);

        Task InvalidateBetsAsync(string userId, string state);
    }
}
