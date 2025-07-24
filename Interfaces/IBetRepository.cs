using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IBetRepository
    {
        Task<Bet> CreateBet(Bet bet);
    }
}
