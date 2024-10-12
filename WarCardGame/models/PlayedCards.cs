namespace WarCardGame.models;

public class PlayedCards
{
    public Dictionary<string, Card> PlayedHand { get; set; } = new();

    public void AddToPlayedHand(string playerName, Card card)
    {
        PlayedHand.Add(playerName, card);
    }

    public Card DetermineWinner()
    {
        List<Card> playedCardList = PlayedHand.Values.ToList();
        
        Card winner = playedCardList.OrderByDescending(card => card.Rank).First();
        return winner;
    }
    
    public void ClearPlay()
    {
        PlayedHand.Clear();
    }
}