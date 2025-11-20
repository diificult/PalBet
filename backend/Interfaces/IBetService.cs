using PalBet.Dtos.Bet;
using PalBet.Enums;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IBetService
    {
        Task<Bet> CreateBet(CreateBetDto betDto, string betHost);
        //Gets the bets that others have requested on you.
        Task<List<BetDto>?> GetBetRequests(string userId);
        //Gets the bets that you have requested.
        Task<List<Bet>?> GetRequestedBets(string userId);
        //Allows a user to accept a Bet
        Task<bool> AcceptBet(string userId, int betId, string? choice);
        //User can decline Bet
        Task<bool> DeclineBet(string userId, int betId);
        //Choose winner
        Task DeclareWinner(string updaterUserId, int betId, int WinningChoiceId);
        Task<List<BetDto>?> GetBetsByState(string userId, BetState? betState);
        Task<bool> DeleteBet(string userId, int betId);
        Task<bool> CancelBet(string userId, int betId);
        Task<BetDto> GetBetById (string userId,int betId);
    }
}
