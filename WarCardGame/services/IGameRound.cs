using WarCardGame.models;

namespace WarCardGame.services;

public interface IGameRound
{
    Deck _Deck { get; set; }
    Hand _Hand { get; set; }
    PlayedCards _PlayedCards { get; set; }
    PlayerHands _PlayerHands { get; set; }
    int PlayerCount { get; set; }
    void DrawCards(string playerName);
    void StartGame(string playerName, int playerCount);
    void RoundStart(string playerName);
    string FindWinner();
    void EndRound(Hand winningHand);
}