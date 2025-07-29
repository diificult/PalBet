using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IBetRepository
    {
        Task<Bet> CreateBet(Bet bet);
        //Gets the bets that others have requested on you.
        Task<List<Bet>?> GetBetRequests(string userId);
        //Gets the bets that you have requested.
        Task<List<Bet>?> GetRequestedBets(string userId);
        Task<Bet?> GetByIdAsync(int id);
        Task<Bet> AcceptBet(int betId, string userId);
    }
}
