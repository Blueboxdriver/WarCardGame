using WarCardGame.models;

namespace WarCardGame.services;

/// <summary>
///     A service that handles the entire operation of the game.
/// </summary>
public class GameRound : IGameRound
{
    /// <summary>
    ///     A list of strings reflecting the currently active players in the game.
    /// </summary>
    public List<string> ListOfActivePlayers { get; set; } = new();

    /// <summary>
    ///     A boolean that is used by the Razor page to determine if a player has won the game yet.
    /// </summary>
    public bool GameWon { get; set; }

    /// <summary>
    ///     An interger reflecting the amount of rounds that have been played in a single session.
    /// </summary>
    public int GameCount { get; set; }

    /// <summary>
    ///     A boolean that that is used by the Razor page to determine if there is a tie.
    /// </summary>
    public bool IsTied { get; set; }

    /// <inheritdoc />
    public Deck _Deck { get; set; } = new();

    /// <inheritdoc />
    public Hand _Hand { get; set; } = new();

    /// <inheritdoc />
    public PlayedCards _PlayedCards { get; set; } = new();

    /// <inheritdoc />
    public PlayerHands _PlayerHands { get; set; } = new();

    /// <inheritdoc />
    public int PlayerCount { get; set; }

    /// <inheritdoc />
    public void DrawCards(string playerName)
    {
        if (_PlayerHands.HandQueue[playerName].Cards.Count != 0) // First, ensure that the player's hand has cards.
        {
            Card drawnCard = _PlayerHands.HandQueue[playerName].Cards.Dequeue();
            _PlayedCards.AddToPlayedHand(playerName, drawnCard); // Remove a card from their queue and add it to the PlayedCards dictionary.
            _Deck.Cards.Push(drawnCard); // Push the card that was dequeued into the deck stack.
        }
        else // If someone doesn't have any cards in their hand, then they have lost.
        {
            CheckIfLost();
            FindGameWinner();
        }
    }

    /// <inheritdoc />
    public void StartGame(string playerName, int playerCount)
    {
        PlayerCount = playerCount;
        _PlayerHands.AddPlayer(playerName); // Create a hand for the player.
        _Deck.AddToHand(_PlayerHands.HandQueue[playerName],
            playerCount); // Divide the amount of cards in the deck(52) by the amount of players in the game and give the quotient to the player's hand.
    }

    /// <inheritdoc />
    public void RoundStart(string playerName)
    {
        DrawCards(playerName); // Take a card from each player's hand.
        if (GameCount % 52 == 0) // If the amount of rounds that have been played can be divided by 52, reshuffle everyone's cards.
        {
            foreach (string player in ListOfActivePlayers) _PlayerHands.ShuffleHand(_PlayerHands.HandQueue[player]);
        }

        GameCount++;
    }

    /// <inheritdoc />
    public string FindWinner()
    {
        string winnerName = ""; // First, create two temporary variables to store the winner's name and the cards that are in play for the round.
        List<Card> cardsInPlay = new();

        if (ListOfActivePlayers.Count ==
            1) // Ensure there's more than 2 players, otherwise end the round by finding a winner and giving the cards back to the remaining player.
        {
            FindGameWinner();
            return ListOfActivePlayers[0];
        }

        foreach (string playerName in ListOfActivePlayers) // Every player adds whatever card they pulled into our temporary list.
            cardsInPlay.Add(_PlayedCards.PlayedHand[playerName]);

        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank).ToList(); // The list is sorted from the largest value to lowest.

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank) // If there is a tie, then the first two values will always be the same.
        {
            IsTied = true;
            _PlayedCards.ClearPlay(); // Set IsTied to true and return the result of the Tiebreaker method.
            return TieBreaker();
        }

        foreach (KeyValuePair<string, Card> playedHand in
                 _PlayedCards.PlayedHand) // If the tie detecting if statement isn't triggered, then there cannot be a tie breaker.
            if (playedHand.Value ==
                cardsInPlay[0]) // We can identify whose player owns the winning card by using the PlayedHand dictionary in the PlayedCards class.
            {
                winnerName = playedHand
                    .Key; // If the value of the Played Hand dictionary, which is a card matches the our winning card, then they are a match, and the key of said dictionary belongs to our winner.
            }

        return winnerName; // Return the winner.
    }

    /// <inheritdoc />
    public void EndRound(Hand winningHand)
    {
        while (_Deck.Cards.Count > 0) // Empty the deck, which has been storing our drawn cards like a pot into the winning player's hand.
        {
            Card cardToMove = _Deck.Cards.Pop();
            winningHand.Cards.Enqueue(cardToMove);
        }

        IsTied = false;
        FindGameWinner(); // At the end of every round, reset IsTied and test to see if the game is over or if anyone needs to be removed.
        CheckIfLost();
    }

    /// <inheritdoc />
    public string TieBreaker()
    {
        List<Card> cardsInPlay = new();
        bool allCannotContinue = true; // This time, we will set three temporary variables. 
        string winnerName = "";

        foreach (string playerName in ListOfActivePlayers.ToList())
        {
            DrawCards(playerName);
            if (_PlayerHands.HandQueue[playerName].Cards.Count ==
                0) // Sometimes, players won't have enough cards for a tie breaker. In this case, we remove them.
            {
                CheckIfLost();
            }
            else
            {
                cardsInPlay.Add(
                    _PlayedCards.PlayedHand
                        [playerName]); // If there is just one player who can continue, turn allCannotContinue to false and add their cards to our temporary list.
                allCannotContinue = false;
            }
        }

        if (allCannotContinue) // In the truly bizarre case that NO ONE has any cards left (which should be impossible), remove them from the game.
        {
            CheckIfLost();
        }

        if (cardsInPlay.Count < 2)
        {
            if (cardsInPlay.Count == 1) // Technically, this will break if no one has cards. But it's also impossible for everyone to have zero cards. 
            {
                // Regardless, we still need to account for it. 
                foreach (KeyValuePair<string, Card> playedHand in _PlayedCards.PlayedHand)
                    if (playedHand.Value ==
                        cardsInPlay
                            [0]) // Like in the normal FindWinner method, if there is one player left, we return their string as the winner the same way we do it for FindWinner.
                    {
                        return playedHand.Key;
                    }
            }

            return "";
        }

        cardsInPlay = cardsInPlay.OrderByDescending(card => card.Rank)
            .ToList(); // Now that all the safe measures are out of the way (yes, really.) we can sort the list.

        if (cardsInPlay[0].Rank == cardsInPlay[1].Rank) // If we tie, we just recursively call tiebreaker again.
        {
            _PlayedCards.ClearPlay();
            return TieBreaker();
        }

        foreach (KeyValuePair<string, Card> playedHand in _PlayedCards.PlayedHand)
            if (playedHand.Value == cardsInPlay[0])
            {
                winnerName = playedHand
                    .Key; // Same as above, if we have a winner we pair the winning card with the PlayedHand dictionary and collect the key from it.
            }

        return winnerName;
    }

    /// <inheritdoc />
    public void CheckIfLost()
    {
        List<string> playerToRemove = new(); // Because we can't edit a list that's being iterated through, we make a secondary list for players to remove.
        foreach (string playerName in ListOfActivePlayers)
            if (_PlayerHands.HandQueue[playerName].Cards.Count == 0) // If a player has zero cards, we add them to that list.
            {
                playerToRemove.Add(playerName);
            }

        foreach (string playerName in playerToRemove) // And then we iterate through the temporary list and remove the players from the actual list.
        {
            ListOfActivePlayers.Remove(playerName);
            FindGameWinner(); // We check for a gamewinner whenever we remove a player.
        }
    }

    /// <inheritdoc />
    public void FindGameWinner()
    {
        if (ListOfActivePlayers.Count == 1) // If the list of active players has only one person in it, it can be assumed that they are the winner.
        {
            GameWon = true;
        }
    }
}