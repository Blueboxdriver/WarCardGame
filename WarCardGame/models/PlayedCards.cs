namespace WarCardGame.models;
using WarCardGame.services;

public class PlayedCards
{
    public Dictionary<string, Card> PlayedHand { get; set; } = new();
    
    public void AddToPlayedHand(string playerName, Card card)
    {
        // Check if the player already exists in the dictionary
        if (PlayedHand.ContainsKey(playerName))
        {
            // Update the card if the player already exists
            PlayedHand[playerName] = card;
        }
        else
        {
            // Add the player and card if they don't already exist
            PlayedHand.Add(playerName, card);
        }
    }
    
    
    public void ClearPlay()
    {
        PlayedHand.Clear();
    }
}