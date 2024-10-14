using WarCardGame.models;

namespace WarCardGame.services;

public class GameRound : IGameRound
{
    public Deck _Deck { get; set; } = new();
    public Hand _Hand { get; set; } = new();
    public PlayedCards _PlayedCards { get; set; } = new();
    public PlayerHands _PlayerHands { get; set; } = new();
    public Players _Players { get; set; } = new();
    public int PlayerCount { get; set; }
    public List<string> ListOfActivePlayers { get; set; } = new();
    public bool GameWon { get; set; }
    public int GameCount { get; set; }
    public bool CannotContinue { get; set; }

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
            CannotContinue = true;
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
            foreach (string player in ListOfActivePlayers) _PlayerHands.ShuffleHand(_PlayerHands.HandQueue[player]);
            Console.WriteLine("Cards shuffled");
        }

        GameCount++;
    }

    public void FindWinner()
    {
        List<Card> cardsInPlay = new List<Card>();

        foreach (string playerName in ListOfActivePlayers)
        {
            cardsInPlay.Add(_PlayedCards.PlayedHand[playerName]);
        }
        Console.WriteLine("Cards in play compiled");
        
        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank).ToList();
        Console.WriteLine("Cards in play sorted");

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank)
        {
            Console.WriteLine("Tie.");
            _PlayedCards.ClearPlay();
            TieBreaker();
        }
        else if (cardsInPlay[0].Rank != cardsInPlay[1].Rank)
        {
            Console.WriteLine("Winner.");

            string winnerName = "";

            foreach (var playedHand in _PlayedCards.PlayedHand)
            {
                if (playedHand.Value == cardsInPlay[0])
                {
                    winnerName = playedHand.Key;
                }
            }
            EndRound(_PlayerHands.HandQueue[winnerName]);
        }
    }


    public void TieBreaker()
    {
        List<Card> cardsInPlay = new List<Card>();
        List<string> tempList = ListOfActivePlayers.ToList();
        bool allCannotContinue = true;
        string winnerName = "";
        
        foreach (string playerName in tempList)
        {
            DrawCards(playerName);
            if (CannotContinue)
            {
                Console.WriteLine($"{playerName} has no cards left to draw.");
                CheckIfLost();
            }
            else if (!CannotContinue)
            {
                Console.WriteLine($"{playerName} has {_PlayedCards.PlayedHand.Count} played cards.");
                cardsInPlay.Add(_PlayedCards.PlayedHand[playerName]);
                allCannotContinue = false;
            }
        }

        if (allCannotContinue)
        {
            Console.WriteLine("No player has cards left.");
            return;
        }

        if (cardsInPlay.Count < 2)
        {
            Console.WriteLine("Less than two cards are in the cards in play list");
            foreach (var playedHand in _PlayedCards.PlayedHand)
            {
                if (playedHand.Value == cardsInPlay[0])
                {
                    winnerName = playedHand.Key;
                }
            }
            EndRound(_PlayerHands.HandQueue[winnerName]);
        }
        
        Console.WriteLine("Cards in play compiled at tiebreaker");
        
        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank).ToList();
        Console.WriteLine("Cards in play sorted in tie breaker");

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank)
        {
            Console.WriteLine("Recursive Tie.");
            _PlayedCards.ClearPlay();
            TieBreaker();
        }
        else if (cardsInPlay[0].Rank != cardsInPlay[1].Rank)
        {
            Console.WriteLine("Tie Winner");

            winnerName = "";

            foreach (var playedHand in _PlayedCards.PlayedHand)
            {
                if (playedHand.Value == cardsInPlay[0])
                {
                    winnerName = playedHand.Key;
                }
            }
            EndRound(_PlayerHands.HandQueue[winnerName]);
        }
    }


    public void EndRound(Hand winningHand)
    {
        // Move all cards from the deck to the winning hand
        while (_Deck.Cards.Count > 0)
        {
            Card cardToMove = _Deck.Cards.Pop();
            winningHand.Cards.Enqueue(cardToMove);
        }
        CheckIfLost();
    }


    public void CheckIfLost()
    {
        List<string> playerToRemove = new List<string>();
        foreach (string playerName in ListOfActivePlayers)
        {
            if (_PlayerHands.HandQueue[playerName].Cards.Count == 0)
            {
                playerToRemove.Add(playerName);
                Console.WriteLine($"{playerName} Removed");
            }
        }

        foreach (string playerName in playerToRemove)
        {
            ListOfActivePlayers.Remove(playerName);
        }
        FindGameWinner();
    }


    public void FindGameWinner()
    {
        if (ListOfActivePlayers.Count == 1)
        {
            GameWon = true;
        }
    }
}