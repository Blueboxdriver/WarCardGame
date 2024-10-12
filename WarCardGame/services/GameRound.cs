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

    public void DrawCards(int input)
    {
        PlayerCount = input;
        for (int i = 0; i < PlayerCount; i++)
        {
            string playerName = _Players.PlayerNames[i + 1];
            Card cardPulled = _PlayerHands.HandQueue[playerName].Cards.Dequeue();
            _PlayedCards.AddToPlayedHand(playerName, cardPulled);

            Console.WriteLine($"{playerName} was created and pulled {cardPulled.Suit} | {cardPulled.Rank}.");
        }
    }

    public Card DetermineWinner()
    {
        List<Card> ballotOfCards = new();
        List<Card> tiedCards = new();

        foreach (var entry in _PlayedCards.PlayedHand)
        { 
            ballotOfCards.Add(entry.Value);
        }

        List<Card> sortedCards = ballotOfCards.OrderByDescending(card => card.Rank).ToList();

        // Check for a tie
        if (sortedCards.Count > 1 && sortedCards[0].Rank == sortedCards[1].Rank)
        {
            tiedCards.AddRange(sortedCards);

            _PlayedCards.ClearPlay();
            return TieBreaker(tiedCards);
        }
        else if (sortedCards.Count > 1 && sortedCards[0].Rank != sortedCards[1].Rank)
        {
            _PlayedCards.ClearPlay();
            int victorIndex = ballotOfCards.IndexOf(sortedCards[0]) + 1;
            string victorName = _Players.PlayerNames[victorIndex];
        
            AddCardsToVictor(sortedCards, victorName, _PlayerHands);
            return sortedCards[0];
        }
        
        _PlayedCards.ClearPlay();
        return null;
    }

    public Card TieBreaker(List<Card> tiedCards)
    {
        DrawCards(PlayerCount);

        List<Card> ballotOfCards = new();
        
        foreach (var entry in _PlayedCards.PlayedHand)
        {
            ballotOfCards.Add(entry.Value);
        }

        List<Card> sortedCards = ballotOfCards.OrderByDescending(card => card.Rank).ToList();

        if (sortedCards.Count > 1 && sortedCards[0].Rank == sortedCards[1].Rank)
        {
            tiedCards.AddRange(sortedCards);

            _PlayedCards.ClearPlay();
            return TieBreaker(tiedCards);
        }
        else if (sortedCards.Count > 1 && sortedCards[0].Rank != sortedCards[1].Rank)
        {
            _PlayedCards.ClearPlay();
            Card winner = sortedCards[0];
            
            sortedCards.AddRange(tiedCards);
            
            int victorIndex = ballotOfCards.IndexOf(sortedCards[0]) + 1;
            string victorName = _Players.PlayerNames[victorIndex];
        
            AddCardsToVictor(sortedCards, victorName, _PlayerHands);
            return winner;
        }
        _PlayedCards.ClearPlay();
        return null;
    }

    public void AddCardsToVictor(List<Card> cardsToAdd, string victorName, PlayerHands victorHand) 
    {
        foreach (Card card in cardsToAdd)
        {
            victorHand.HandQueue[victorName].Cards.Enqueue(card);
        }
    }


    public void CreateHands(int PlayerCount)
    {
        _PlayerHands.HandQueue.Clear();

        for (int i = 0; i < PlayerCount; i++)
        {
            string playerName = _Players.PlayerNames[i + 1];
            _PlayerHands.AddPlayer(playerName);
            _Deck.AddToHand(_PlayerHands.HandQueue[playerName], PlayerCount);
        }
    }
}