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
    public void DrawCards(string playerName);

    public void StartGame(string playerName, int playerCount);

    public void RoundStart(string playerName);

    public void FindWinner();

    public void EndRound(Hand winningHand);
}