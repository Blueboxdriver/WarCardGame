namespace WarCardGame.models;

public class Card
{
    public Card(int suitKey, int rank)
    {
        Rank = rank;
        Suit = SuitDictionary[suitKey];
    }

    public Dictionary<int, string> SuitDictionary { get; set; } = new()
    {
        { 1, "Hearts" },
        { 2, "Diamonds" },
        { 3, "Spades" },
        { 4, "Clubs" }
    };

    public int Rank { get; set; }
    public string Suit { get; set; }
}