using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
