namespace PalBet.Dtos.Groups
{
    public class GroupMemberDto
    {
        public string Username { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public int? Balance { get; set; }
        public string? Role { get; set; }

        //Permissions
        public bool? CanCreateBets { get; set; }
    }
}
