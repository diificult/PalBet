using PalBet.Dtos;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IBetService
    {
        Task<Bet> CreateBet(CreateBetDto betDto);
    }
}
