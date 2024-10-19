namespace WarCardGame.models;

/// <summary>
///     Represents a card object, consisting of a Rank and a Suit.
/// </summary>
public class Card
{
    /// <summary>
    ///     A constructor the creates a card object with a suit and rank.
    /// </summary>
    /// <param name="suitKey">The integer key that binds to a string representing the suit.</param>
    /// <param name="rank">An integer reflecting the value of the card.</param>
    public Card(int suitKey, int rank)
    {
        Rank = rank;
        Suit = SuitDictionary[suitKey];
    }

    /// <summary>
    ///     An integer reflecting the value a card has. Can go from 2-14 (Ace as 14).
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     A string representing the suit a card has.
    /// </summary>
    public string Suit { get; set; }

    /// <summary>
    ///     A simple dictionary that binds an integer as a key to a string reflecting the suit as the value.
    /// </summary>
    public Dictionary<int, string> SuitDictionary { get; set; } = new()
    {
        { 1, "Hearts" },
        { 2, "Diamonds" },
        { 3, "Spades" },
        { 4, "Clubs" }
    };
}