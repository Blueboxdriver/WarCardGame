namespace WarCardGame.models;

/// <summary>
///     A class that dictates the logic on how a hand can function in the Game.
/// </summary>
public class Hand
{
    /// <summary>
    ///     A simple constructor that creates a new queue of cards whenever an object is made.
    /// </summary>
    public Hand()
    {
        Cards = new Queue<Card>();
    }

    /// <summary>
    ///     The queue of cards in a hand.
    /// </summary>
    public Queue<Card> Cards { get; set; }
}