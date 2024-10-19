namespace WarCardGame.models;

public class PlayerHands
{
    private static readonly Random _rng = new();
    public Dictionary<string, Hand> HandQueue { get; set; } = new(); // Initialize here

    public void AddPlayer(string playerName)
    {
        HandQueue[playerName] = new Hand();
    }

    public void ShuffleHand(Hand hand)
    {
        List<Card> list = new(hand.Cards);

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        hand.Cards.Clear();
        foreach (Card card in list) hand.Cards.Enqueue(card);
    }

    public void ResetHand(string playerName)
    {
        HandQueue[playerName].Cards.Clear();
    }
}