using PalBet.Dtos.Friends;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class FriendMapper
    {

        public static FriendDto fromFreindshipToFriendDto(this AppUser user) {

            return new FriendDto
            {
                UserId = user.Id,
                Username = user.UserName,
            };
        }
    }
}
