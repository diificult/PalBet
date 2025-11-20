namespace PalBet.Dtos.Bet
{
    public class ChooseWinnerDto
    {
        public int BetId { get; set; }
        public List<string> WinnerUsernames { get; set; }
    }
}
