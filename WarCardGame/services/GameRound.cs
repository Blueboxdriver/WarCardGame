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

    public void DrawCards(string playerName)
    {
        // Check if the player's hand is not empty
        if (_PlayerHands.HandQueue[playerName].Cards.Count != 0)
        {
            Card drawnCard = _PlayerHands.HandQueue[playerName].Cards.Dequeue();
            _PlayedCards.AddToPlayedHand(playerName, drawnCard);
            _Deck.Cards.Push(drawnCard);
        }
        else
        {
            Console.WriteLine($"{playerName} has no cards left to draw.");
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
        if (GameCount % 5 == 0)
        {
            foreach (var player in ListOfActivePlayers)
            {
                _PlayerHands.ShuffleHand(_PlayerHands.HandQueue[player]);
            }
        }

        GameCount++;
    }

    public void FindWinner()
    {
        List<Card> cardsInPot = new();
        bool test = false;
        
        
        foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand) cardsInPot.Add(thing.Value);
        
        if (cardsInPot.Count < 2)
        {
            Console.WriteLine("Not enough cards for comparison.");
            CheckIfLost();
            FindGameWinner();
            return;
        }
        
        cardsInPot.Sort((card1, card2) => card2.Rank.CompareTo(card1.Rank));
        
        Console.WriteLine($"{cardsInPot[0].Rank} and {cardsInPot[1].Rank}");

        if (cardsInPot[0].Rank == cardsInPot[1].Rank)
        {
            Console.WriteLine($"Tie Breaker: {cardsInPot[0].Rank} == {cardsInPot[1].Rank}");
            
            _PlayedCards.ClearPlay();
            foreach (var player in ListOfActivePlayers)
            {
                if (_PlayerHands.HandQueue[player].Cards.Count != 0)
                {
                    CheckIfLost();
                    FindGameWinner();
                }
                break;
            }
            TieBreaker();
        }
        else if (cardsInPot[0].Rank != cardsInPot[1].Rank)
        {
            string winningPlayer = null;
            foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand)
                if (thing.Value == cardsInPot[0])
                {
                    winningPlayer = thing.Key;
                    break;
                }
            FindGameWinner();
            EndRound(_PlayerHands.HandQueue[winningPlayer]);
        }
    }

    public void TieBreaker()
    {
        List<Card> cardsInPot = new();
        List<string> playersToDraw = new(ListOfActivePlayers);

        if (playersToDraw.All(player => _PlayerHands.HandQueue[player].Cards.Count > 0))
        {
            foreach (var player in playersToDraw)
            {
                DrawCards(player);
            }
        }
        else
        {
            Console.WriteLine("A player has no more cards for a tie-breaker.");
            CheckIfLost();
            FindGameWinner();
            return;
        }

        foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand)
        {
            cardsInPot.Add(thing.Value);
        }

        if (cardsInPot.Count < 2)
        {
            Console.WriteLine("Not enough cards for comparison.");
            CheckIfLost();
            FindGameWinner();
            return;
        }
        
        cardsInPot.Sort((card1, card2) => card2.Rank.CompareTo(card1.Rank));

        if (cardsInPot[0].Rank == cardsInPot[1].Rank)
        {
            Console.WriteLine($"Recursive Tie Breaker: {cardsInPot[0].Rank} == {cardsInPot[1].Rank}");
            _PlayedCards.ClearPlay();
            TieBreaker(); 
        }
        else
        {
            string winningPlayer = null;
            foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand)
            {
                if (thing.Value == cardsInPot[0])
                {
                    winningPlayer = thing.Key;
                    break;
                }
            }
            EndRound(_PlayerHands.HandQueue[winningPlayer]);
        }
    }



    public void EndRound(Hand winningHand)
    {
        while (_Deck.Cards.Count > 0)
        {
            Card cardToMove = _Deck.Cards.Pop();

            winningHand.Cards.Enqueue(cardToMove);
        }
        CheckIfLost();
        FindGameWinner();
    }

    public void CheckIfLost()
    {
        List<string> playersToRemove = new();
        
        foreach (var player in _PlayerHands.HandQueue)
        {
            if (player.Value.Cards.Count == 0)
            {
                Console.WriteLine($"{player.Key} has lost the game.");
                playersToRemove.Add(player.Key);
            }
        }
        
        foreach (var playerName in playersToRemove)
        {
            _PlayerHands.HandQueue.Remove(playerName);
            ListOfActivePlayers.Remove(playerName); 
            PlayerCount--; 
        }
        FindGameWinner();
    }


    public void FindGameWinner()
    {
        foreach (var thing in ListOfActivePlayers)
        {
            if (_PlayerHands.HandQueue[thing].Cards.Count == 52)
            {
                GameWon = true;
            }
        }
    }

}