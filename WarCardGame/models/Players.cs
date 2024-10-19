namespace WarCardGame.models;

/// <summary>
///     This class holds a large dictionary of names, corresponding to the amount of players in a game.
/// </summary>
public class Players
{
    public Dictionary<int, string> PlayerNames { get; set; } = new()
    {
        { 0, "Alice" },
        { 1, "Bob" },
        { 2, "Charlie" },
        { 3, "Diana" }
    };
}