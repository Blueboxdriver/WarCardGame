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

    public void DrawCards(string playerName)
    {
        Card drawnCard = _PlayerHands.HandQueue[playerName].Cards.Dequeue();
        _PlayedCards.AddToPlayedHand(playerName, drawnCard);
        _Deck.Cards.Push(drawnCard);
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
    }

    public void FindWinner()
    {
        List<Card> cardsInPot = new();

        foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand) cardsInPot.Add(thing.Value);

        cardsInPot.Sort((card1, card2) => card2.Rank.CompareTo(card1.Rank));
        Console.WriteLine($"{cardsInPot[0].Rank} and {cardsInPot[1].Rank}");

        if (cardsInPot[0].Rank == cardsInPot[1].Rank)
        {
            Console.WriteLine($"Tie Breaker: {cardsInPot[0].Rank} == {cardsInPot[1].Rank}");
            _PlayedCards.ClearPlay();
            CheckIfLost();
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

            EndRound(_PlayerHands.HandQueue[winningPlayer]);
        }
    }

    public void TieBreaker()
    {
        List<Card> cardsInPot = new();

        for (int i = 0; i < PlayerCount; i++)
        {
            DrawCards(_Players.PlayerNames[i]);
        }

        foreach (KeyValuePair<string, Card> thing in _PlayedCards.PlayedHand) cardsInPot.Add(thing.Value);

        cardsInPot.Sort((card1, card2) => card2.Rank.CompareTo(card1.Rank));

        if (cardsInPot[0].Rank == cardsInPot[1].Rank)
        {
            Console.WriteLine($"Recursive Tie Breaker: {cardsInPot[0].Rank} == {cardsInPot[1].Rank}");
            _PlayedCards.ClearPlay();
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

            EndRound(_PlayerHands.HandQueue[winningPlayer]);
        }
    }

    public void EndRound(Hand winningHand)
    {
        // While the Deck has cards, move them to the winning hand
        while (_Deck.Cards.Count > 0)
        {
            // Pop a card from the Deck stack
            Card cardToMove = _Deck.Cards.Pop();

            // Enqueue the card to the winning hand's card queue
            winningHand.Cards.Enqueue(cardToMove);
        }
        CheckIfLost();
    }

    public void CheckIfLost()
    {
        // Create a list to store players who have lost
        List<string> playersToRemove = new();

        // Check each player's hand
        foreach (var player in _PlayerHands.HandQueue)
        {
            // If a player's hand is empty, add their name to the removal list
            if (player.Value.Cards.Count == 0)
            {
                Console.WriteLine($"{player.Key} has lost the game.");
                playersToRemove.Add(player.Key);  // Store the player to be removed
                ListOfActivePlayers.Remove(player.Key);
            }
        }

        // Remove players who have lost from the HandQueue
        foreach (var playerName in playersToRemove)
        {
            PlayerCount--;
            _PlayerHands.HandQueue.Remove(playerName);  // Remove the player from the HandQueue
        }
    }

}