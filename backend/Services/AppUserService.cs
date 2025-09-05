using PalBet.Interfaces;

namespace PalBet.Services
{
    public class AppUserService : IAppUserService
    {

        public readonly IAppUserRepository _AppUserRepository;
        public AppUserService(IAppUserRepository appUserRepository) {
        _AppUserRepository = appUserRepository;
        }
        public async Task<int> GetCoins(string id)
        {
            return await _AppUserRepository.GetCoins(id);
        }
    }
}
