namespace WarCardGame.models;

/// <summary>
///     A class that handles binding a Player's name to a card, granting them ownership of the card.
/// </summary>
public class PlayedCards
{
    /// <summary>
    ///     A dictionary that binds a string(A player's name) as the key and their card as the value, granting the player
    ///     ownership of the card.
    /// </summary>
    public Dictionary<string, Card> PlayedHand { get; set; } = new();

    /// <summary>
    ///     A method that binds a player's name (as a string) to a card, persumably one that they've drawn.
    /// </summary>
    /// <param name="playerName">The string representing a player's name</param>
    /// <param name="card"><see cref="Card" /> Representing a drawn card.</param>
    public void AddToPlayedHand(string playerName, Card card)
    {
        if (PlayedHand.ContainsKey(playerName)) // Check if the player already exists in the dictionary
        {
            PlayedHand[playerName] = card; // Update the card if the player already exists
        }
        else // Essentially a safety mechanism to ensure that every player can bind a card to their name.
        {
            PlayedHand.Add(playerName, card); // Add the player and card if they don't already exist
        }
    }

    /// <summary>
    ///     A simple method that clears every Key Value Pair from the dictionary for future rounds.
    /// </summary>
    public void ClearPlay()
    {
        PlayedHand.Clear();
    }
}