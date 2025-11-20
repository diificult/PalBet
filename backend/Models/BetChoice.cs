namespace PalBet.Models
{
    public class BetChoice
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public Bet Bet { get; set; }
        public int BetId { get; set; }


    }
}
