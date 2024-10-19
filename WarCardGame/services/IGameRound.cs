using WarCardGame.models;

namespace WarCardGame.services;

/// <summary>
///     Interface that mandates the behaviors required by <see cref="GameRound" />.
/// </summary>
public interface IGameRound
{
    /// <summary>
    ///     A deck object for managing the Game's <see cref="Deck" />.
    /// </summary>
    Deck _Deck { get; set; }

    /// <summary>
    ///     A Hand object for managing the Game's <see cref="Hand" />s.
    /// </summary>
    Hand _Hand { get; set; }

    /// <summary>
    ///     A PlayedCards object for managing the Game's currently <see cref="PlayedCards" />.
    /// </summary>
    PlayedCards _PlayedCards { get; set; }

    /// <summary>
    ///     A PlayerHands object mean for managing the Game's collection of <see cref="PlayerHands" />.
    /// </summary>
    PlayerHands _PlayerHands { get; set; }

    /// <summary>
    ///     An integer reflecting the amount of players the Game started off with.
    /// </summary>
    int PlayerCount { get; set; }

    /// <summary>
    ///     Drawcards allocates a set amount of <see cref="Card" /> objects into a <see cref="PlayedCards" />/
    /// </summary>
    /// <param name="playerName">The name of the player cards are being drawn from.</param>
    void DrawCards(string playerName);

    /// <summary>
    ///     A method that begins the game by supplying each Player's <see cref="Hand" /> with 52 / <see cref="PlayerCount" />
    ///     cards from the <see cref="Deck" />.
    /// </summary>
    /// <param name="playerName">The name of the player whose <see cref="Hand" /> is being filled with <see cref="Card" />s.</param>
    /// <param name="playerCount"><see cref="PlayerCount" />, the amount of players when the game starts.</param>
    void StartGame(string playerName, int playerCount);

    /// <summary>
    ///     A method that begins a round by taking a <see cref="Card" /> from each <see cref="PlayerHands" /> and adding them
    ///     to <see cref="PlayedCards" />.
    /// </summary>
    /// <param name="playerName">The name of the player whose <see cref="Hand" /> is getting drawn from.</param>
    void RoundStart(string playerName);

    /// <summary>
    ///     A method that creates and sorts a list of <see cref="PlayedCards" /> and determines the highest value card,
    ///     returning it as a winner.
    /// </summary>
    /// <returns>A string representing the winning player's name.</returns>
    string FindWinner();

    /// <summary>
    ///     A recursive method meant for handling any and all ties discovered by <see cref="FindWinner" />. Will recursively
    ///     run until there is a winner using the same logic as <see cref="FindWinner" />
    /// </summary>
    /// <returns>A string representing the winning player's name.</returns>
    string TieBreaker();

    /// <summary>
    ///     A method that nds the round, giving all drawn cards to the winner.
    /// </summary>
    /// <param name="winningHand">The <see cref="Hand" /> belonging to the player who won.</param>
    void EndRound(Hand winningHand);

    /// <summary>
    ///     A method which creates a temporary list and pulls from a current list of players, removing any player from that
    ///     list if they no longer have any cards.
    /// </summary>
    void CheckIfLost();

    /// <summary>
    ///     A method that determines the amount of players left in the game, if only one player remains the the game is
    ///     finished.
    /// </summary>
    void FindGameWinner();
}