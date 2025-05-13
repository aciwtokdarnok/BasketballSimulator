using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Models.Players;
using BasketballSimulator.Logic.Generators.Players;

namespace BasketballSimulator.Logic.Generators.Teams;

/// <summary>
/// Generates a team roster of 12–15 players aged 19–38 years old,
/// distributed by archetype: Point, Wing, Big.
/// Each player is generated at a randomized start age (19–22),
/// then developed up to a randomized final age (19–38).
/// </summary>
public class TeamRosterGenerator
{
    private readonly Random _rng;
    private readonly PlayerGenerator _playerGen;
    private readonly int _initialSeasonYear;

    /// <summary>
    /// Initializes the generator.
    /// </summary>
    /// <param name="rng">Random number generator.</param>
    /// <param name="initialSeasonYear">The season year for which roster is generated (e.g. 2025).</param>
    public TeamRosterGenerator(Random rng, int initialSeasonYear = 2025)
    {
        _rng = rng;
        _playerGen = new PlayerGenerator(rng);
        _initialSeasonYear = initialSeasonYear;
    }

    /// <summary>
    /// Generates a roster for a single team.
    /// Each player is generated at a random age between 19 and 22,
    /// then developed up to a randomized final age between 19 and 38.
    /// </summary>
    /// <param name="teamAbbrev">Team abbreviation for snapshots.</param>
    public IReadOnlyList<Player> GenerateRoster(string teamAbbrev)
    {
        int totalSize = _rng.Next(12, 16);    // 12..15 players
        int guards = _rng.Next(5, 7);      // 5..6 guards
        int wings = _rng.Next(4, 6);      // 4..5 wings
        int bigs = totalSize - guards - wings;

        var roster = new List<Player>(totalSize);

        // Helper to generate and develop one player
        Player CreatePlayer(Archetype arch)
        {
            // Target final age between 19 and 38
            byte finalAge = (byte)_rng.Next(19, 39);
            // Randomized starting age between 19 and 22
            byte startAge = (byte)_rng.Next(19, 23);
            // Use the lesser of startAge and finalAge for generation
            byte genAge = (byte)Math.Min(startAge, finalAge);

            var player = _playerGen.Generate(genAge, arch);

            // Develop for each year until reaching finalAge
            int developYears = Math.Max(0, finalAge - genAge);
            int draftYear = _initialSeasonYear - developYears;

            // Develop one season at a time, advancing year by year
            for (int i = 0; i < developYears; i++)
            {
                int seasonYear = draftYear + i + 1;
                player.Develop(seasonYear, teamAbbrev);
            }

            return player;
        }

        // 1) Generate guards
        for (int i = 0; i < guards; i++)
        {
            roster.Add(CreatePlayer(Archetype.Point));
        }

        // 2) Generate wings
        for (int i = 0; i < wings; i++)
        {
            roster.Add(CreatePlayer(Archetype.Wing));
        }

        // 3) Generate bigs – mostly Big, occasionally Wing
        for (int i = 0; i < bigs; i++)
        {
            var arch = _rng.NextDouble() < 0.15
                ? Archetype.Wing
                : Archetype.Big;
            roster.Add(CreatePlayer(arch));
        }

        return roster;
    }
}
