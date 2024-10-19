namespace WarCardGame.models;

/// <summary>
///     This class holds a dictionary of names, corresponding to the amount of players in a game.
/// </summary>
public class Players
{
    /// <summary>
    ///     A small dictionary that binds an integer key to a string value.
    /// </summary>
    public Dictionary<int, string> PlayerNames { get; set; } = new()
    {
        { 0, "Alice" },
        { 1, "Bob" },
        { 2, "Charlie" },
        { 3, "Diana" }
    };
}