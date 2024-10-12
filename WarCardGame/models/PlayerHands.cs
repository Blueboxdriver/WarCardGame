namespace WarCardGame.models;

public class PlayerHands
{
    public Dictionary<string, Hand> HandQueue { get; set; } = new(); // Initialize here

    public void AddPlayer(string playerName)
    {
        HandQueue[playerName] = new Hand();
    }
}