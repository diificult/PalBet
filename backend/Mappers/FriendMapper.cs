using PalBet.Dtos.Friends;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class FriendMapper
    {

        public static OtherUserDto fromFreindshipToFriendDto(this AppUser user) {

            return new OtherUserDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Balance = user.PersonalCoins,
            };
        }
    }
}
