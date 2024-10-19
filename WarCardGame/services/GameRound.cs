using WarCardGame.models;

namespace WarCardGame.services;

public class GameRound : IGameRound
{
    public List<string> ListOfActivePlayers { get; set; } = new();
    public bool GameWon { get; set; }
    public int GameCount { get; set; }
    public bool IsTied { get; set; }
    public Deck _Deck { get; set; } = new();
    public Hand _Hand { get; set; } = new();
    public PlayedCards _PlayedCards { get; set; } = new();
    public PlayerHands _PlayerHands { get; set; } = new();
    public int PlayerCount { get; set; }

    public void DrawCards(string playerName)
    {
        if (_PlayerHands.HandQueue[playerName].Cards.Count != 0)
        {
            Card drawnCard = _PlayerHands.HandQueue[playerName].Cards.Dequeue();
            _PlayedCards.AddToPlayedHand(playerName, drawnCard);
            _Deck.Cards.Push(drawnCard);
        }
        else
        {
            CheckIfLost();
            FindGameWinner();
        }
    }

    public void StartGame(string playerName, int playerCount)
    {
        PlayerCount = playerCount;
        _PlayerHands.AddPlayer(playerName);
        _Deck.AddToHand(_PlayerHands.HandQueue[playerName], playerCount);
    }

    public void RoundStart(string playerName)
    {
        DrawCards(playerName);
        if (GameCount % 52 == 0)
        {
            foreach (string player in ListOfActivePlayers)
            {
                _PlayerHands.ShuffleHand(_PlayerHands.HandQueue[player]);
            }
        }

        GameCount++;
    }

    public string FindWinner()
    {
        string winnerName = "";
        List<Card> cardsInPlay = new();

        if (ListOfActivePlayers.Count == 1)
        {
            FindGameWinner();
            return ListOfActivePlayers[0];
        }

        foreach (string playerName in ListOfActivePlayers)
        {
            cardsInPlay.Add(_PlayedCards.PlayedHand[playerName]);
        }

        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank).ToList();

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank)
        {
            IsTied = true;
            _PlayedCards.ClearPlay();
            return TieBreaker();
        }

        foreach (KeyValuePair<string, Card> playedHand in _PlayedCards.PlayedHand)
        {
            if (playedHand.Value == cardsInPlay[0])
            {
                winnerName = playedHand.Key;
            }
        }

        return winnerName;
    }

    public void EndRound(Hand winningHand)
    {
        while (_Deck.Cards.Count > 0)
        {
            Card cardToMove = _Deck.Cards.Pop();
            winningHand.Cards.Enqueue(cardToMove);
        }

        IsTied = false;
        FindGameWinner();
        CheckIfLost();
    }

    public string TieBreaker()
    {
        List<Card> cardsInPlay = new();
        bool allCannotContinue = true;
        string winnerName = "";

        foreach (string playerName in ListOfActivePlayers.ToList())
        {
            DrawCards(playerName);
            if (_PlayerHands.HandQueue[playerName].Cards.Count == 0)
            {
                CheckIfLost();
            }
            else
            {
                cardsInPlay.Add(_PlayedCards.PlayedHand[playerName]);
                allCannotContinue = false;
            }
        }

        if (allCannotContinue)
        {
            CheckIfLost();
        }

        if (cardsInPlay.Count < 2)
        {
            if (cardsInPlay.Count == 1)
            {
                foreach (KeyValuePair<string, Card> playedHand in _PlayedCards.PlayedHand)
                {
                    if (playedHand.Value == cardsInPlay[0])
                    {
                        return playedHand.Key;
                    }
                }
            }
            return "";
        }

        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank).ToList();

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank)
        {
            _PlayedCards.ClearPlay();
            return TieBreaker();
        }

        foreach (KeyValuePair<string, Card> playedHand in _PlayedCards.PlayedHand)
        {
            if (playedHand.Value == cardsInPlay[0])
            {
                winnerName = playedHand.Key;
            }
        }
        
        return winnerName;
    }

    public void CheckIfLost()
    {
        List<string> playerToRemove = new();
        foreach (string playerName in ListOfActivePlayers)
        {
            if (_PlayerHands.HandQueue[playerName].Cards.Count == 0)
            {
                playerToRemove.Add(playerName);
            }
        }

        foreach (string playerName in playerToRemove)
        {
            ListOfActivePlayers.Remove(playerName);
            FindGameWinner();
        }
    }

    public void FindGameWinner()
    {
        if (ListOfActivePlayers.Count == 1)
        {
            GameWon = true;
        }
    }
}
