namespace WarCardGame.models;

public class Card
{
    public Dictionary<int, string> SuitDictionary { get; set; } = new Dictionary<int, string>()
    {
        { 1, "Hearts" },
        { 2, "Diamonds" },
        { 3, "Spades" },
        { 4, "Clubs" }
    };
    public int Rank { get; set; }
    public string Suit { get; set; }

    public Card(int suitKey, int rank)
    {
        Rank = rank;
        Suit = SuitDictionary[suitKey];
    }
    
    
}