namespace PalBet.Models
{
    public class Group
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int DefaultCoinBalance { get; set; } = 100;
        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
