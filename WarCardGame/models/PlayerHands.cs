namespace WarCardGame.models;

/// <summary>
///     A class that contains the logic for binding Player to their Hand.
/// </summary>
public class PlayerHands
{
    /// <summary>
    ///     A Random class for the purpose of hand shuffling.
    /// </summary>
    private static readonly Random _rng = new();

    /// <summary>
    ///     A dictionary that binds a string(The player's name) key to a hand object value, giving them ownership of the hand.)
    /// </summary>
    public Dictionary<string, Hand> HandQueue { get; set; } = new(); // Initialize here

    /// <summary>
    ///     A method that binds a player to a new hand by taking their name as a string and passing it as a key.
    /// </summary>
    /// <param name="playerName">A string representing the player's name.</param>
    public void AddPlayer(string playerName)
    {
        HandQueue[playerName] = new Hand();
    }

    /// <summary>
    ///     A method that takes every card in a player's hand and shuffles it, creating a new order in which their cards are
    ///     drawn.
    /// </summary>
    /// <param name="hand">The player's hand to be shuffled.</param>
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
}