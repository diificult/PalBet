using System.Security.Claims;
using PalBet.Models;
using Microsoft.AspNetCore.Identity;

namespace PalBet.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;

        }
        public static async Task<AppUser> GetCurrentUserAsync(this ClaimsPrincipal user, UserManager<AppUser> userManager)
        {
            var username = user.GetUsername();
            return await userManager.FindByNameAsync(username);
        }
        public static async Task<string> GetCurrentUserIdAsync(this ClaimsPrincipal user, UserManager<AppUser> userManager)
        {
            var appUser = await user.GetCurrentUserAsync(userManager);
            return appUser.Id;
        }
    }
}
