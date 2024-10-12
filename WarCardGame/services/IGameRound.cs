namespace WarCardGame.services;
using WarCardGame.models;

public interface IGameRound
{
    public Deck _Deck { get; set; }
    public Hand _Hand { get; set; }
    public PlayedCards _PlayedCards { get; set; }
    public PlayerHands _PlayerHands { get; set; }
    public Players _Players { get; set; }
    public int PlayerCount { get; set; }
    public void DrawCards(int input);

    public Card DetermineWinner();

    public void CreateHands(int PlayerCount);

}