namespace WarCardGame.models;

public class PlayerHands
{
    public Dictionary<string, Hand> HandQueue { get; set; } = new(); // Initialize here

    public void AddPlayer(string playerName)
    {
        HandQueue[playerName] = new Hand();
    }
    
    public Hand GetHand(string playerName)
    {
        if (HandQueue.TryGetValue(playerName, out var hand))
        {
            return hand;
        }

        throw new KeyNotFoundException($"Player {playerName} not found."); // Handle the case if the player does not exist
    }


}