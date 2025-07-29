using PalBet.Dtos.Bet;
using PalBet.Enums;
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
        //Allows a user to accept a bet
        Task<bool> AcceptBet(string userId, int betId);
        //Choose winner
        Task<bool> SetWinner(string winnerUserId, string updaterUserId, int betId);
        Task<List<Bet>?> GetBetsByState(string userId, BetState? betState);
    }
}
