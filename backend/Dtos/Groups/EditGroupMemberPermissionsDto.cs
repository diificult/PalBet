namespace PalBet.Dtos.Groups
{
    public class EditGroupMemberPermissionsDto
    {
        public int GroupId { get; set; }
        public string Username { get; set; }
        public bool CanCreateBets { get; set; }

    }
}
