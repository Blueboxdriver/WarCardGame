namespace WarCardGame.models;

public class Hand
{
    public Hand()
    {
        Cards = new Queue<Card>();
    }

    public Queue<Card> Cards { get; set; }
    public Players _players { get; set; } = new();

    public PlayerHands _playerHands { get; set; } = new();
    public PlayedCards _playedCards { get; set; } = new();
}