using BasketballSimulator.Core.Enums.Player;
using BasketballSimulator.Core.Utilities.Player;

namespace BasketballSimulator.Core.Models.Player;

/// <summary>
/// Represents a basketball player with core ratings and metadata.
/// Overall is computed on-the-fly. Potential is calculated once on creation.
/// </summary>
public class Player
{
    // Raw data
    public int Height { get; init; }
    public byte HeightRating { get; init; }
    public byte Weight { get; init; }
    public string Country { get; init; }
    public string FullName { get; init; }
    public required RatingPresets Ratings { get; init; }
    public Archetype Archetype { get; init; }
    public Position Position { get; init; }
    public byte Age { get; init; }

    // Backing field for potential (computed once)
    public byte Potential { get; init; }

    // Dynamic convenience properties
    public byte Strength => Ratings.Strength;
    public byte Speed => Ratings.Speed;
    public byte Jumping => Ratings.Jumping;
    public byte Endurance => Ratings.Endurance;
    public byte Inside => Ratings.Inside;
    public byte DunksLayups => Ratings.DunksLayups;
    public byte FreeThrows => Ratings.FreeThrows;
    public byte MidRange => Ratings.MidRange;
    public byte ThreePointers => Ratings.ThreePointers;
    public byte OffensiveIQ => Ratings.OffensiveIQ;
    public byte DefensiveIQ => Ratings.DefensiveIQ;
    public byte Dribbling => Ratings.Dribbling;
    public byte Passing => Ratings.Passing;
    public byte Rebounding => Ratings.Rebounding;

    /// <summary>
    /// Overall rating, computed as a weighted regression + fudge factor.
    /// Always up-to-date with Height and Ratings.
    /// </summary>
    public byte Overall => RatingCalculator.ComputeOverall(HeightRating, Ratings);

    /// <summary>
    /// Factory: create a new Player from raw inputs.
    /// Potential is calculated once here.
    /// </summary>
    public static Player Create(
        int height,
        byte heightRating,
        byte weight,
        string country,
        string fullName,
        RatingPresets ratings,
        Archetype archetype,
        Position position,
        byte age
    )
    {
        // Compute initial overall
        byte initialOverall = RatingCalculator.ComputeOverall(heightRating, ratings);
        // Compute potential based on initial overall and age
        byte computedPotential = RatingCalculator.ComputePotential(initialOverall, age);

        // Instantiate player
        var player = new Player
        {
            Height = height,
            HeightRating = heightRating,
            Weight = weight,
            Country = country,
            FullName = fullName,
            Ratings = ratings,
            Archetype = archetype,
            Position = position,
            Age = age,
            Potential = computedPotential
        };

        // Add first history entry
        player.SnapshotRatings(2025, "AAA");

        return player;
    }

    // Navigation property for rating history
    public List<PlayerRatingHistory> RatingHistory { get; init; } = new();

    /// <summary>
    /// Capture a snapshot of the player's ratings for a given season.
    /// </summary>
    /// <param name="year">Season year (e.g. 2025).</param>
    /// <param name="team">Team name or abbreviation.</param>
    public void SnapshotRatings(int year, string team)
    {
        var entry = new PlayerRatingHistory
        {
            Year = year,
            Team = team,
            Age = Age,
            Position = Position,
            Height = (byte)Height,
            HeightRating = HeightRating,
            Weight = Weight,
            Strength = Strength,
            Speed = Speed,
            Jumping = Jumping,
            Endurance = Endurance,
            Inside = Inside,
            DunksLayups = DunksLayups,
            FreeThrows = FreeThrows,
            MidRange = MidRange,
            ThreePointers = ThreePointers,
            OffensiveIQ = OffensiveIQ,
            DefensiveIQ = DefensiveIQ,
            Dribbling = Dribbling,
            Passing = Passing,
            Rebounding = Rebounding,
            Overall = Overall,
            Potential = Potential
        };

        RatingHistory.Add(entry);
    }

    /// <summary>
    /// Advances the player by one season: applies development,
    /// increments age, and snapshots the new ratings.
    /// </summary>
    /// <param name="coachingLevel">Coaching level for development.</param>
    /// <param name="nextYear">The upcoming season year.</param>
    /// <param name="team">Team name or abbreviation for the new season.</param>
    public void Develop(int coachingLevel, int nextYear, string team)
    {
        // Apply development to ratings and height
        PlayerDevelopmentService.DevelopSeason(this);

        // Bump age by one year (using reflection since Age is init-only)
        var ageProp = typeof(Player).GetProperty(nameof(Age))!;
        ageProp.SetValue(this, (byte)(Age + 1));

        // Snapshot the new state
        SnapshotRatings(nextYear, team);
    }
}
