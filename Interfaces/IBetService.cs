using PalBet.Dtos;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IBetService
    {
        Task<Bet> CreateBet(CreateBetDto betDto, string betHost);
        //Gets the bets that others have requested on you.
        Task<List<Bet>?> GetBetRequests(string userId);
        //Gets the bets that you have requested.
        Task<List<Bet>?> GetRequestedBets(string userId);
        //Accept a bet
        Task<bool> AcceptBet(string userId, int betId);
    }
}
