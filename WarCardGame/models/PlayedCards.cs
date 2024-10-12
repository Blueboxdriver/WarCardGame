namespace WarCardGame.models;
using WarCardGame.services;

public class PlayedCards
{
    public Dictionary<string, Card> PlayedHand { get; set; } = new();
    
    public void AddToPlayedHand(string playerName, Card card)
    {
        PlayedHand.Add(playerName, card);
    }
    
    
    public void ClearPlay()
    {
        PlayedHand.Clear();
    }
}