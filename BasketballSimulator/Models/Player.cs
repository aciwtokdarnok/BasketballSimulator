using BasketballSimulator.Core.Enums;
using BasketballSimulator.Core.Utilities;

namespace BasketballSimulator.Core.Models;

/// <summary>
/// Represents a basketball player with core ratings and metadata.
/// Overall and Potential are computed on-the-fly.
/// </summary>
public class Player
{
    // Raw data
    public int Height { get; init; }
    public byte HeightRating { get; init; }
    public byte Weight { get; init; }
    public string Country { get; init; }
    public required RatingPresets Ratings { get; init; }
    public Archetype Archetype { get; init; }
    public Position Position { get; init; }
    public byte Age { get; init; }

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
    /// Estimated potential, computed from Overall and Age.
    /// Always up-to-date if Overall or Age change.
    /// </summary>
    public byte Potential => RatingCalculator.ComputePotential(Overall, Age);

    /// <summary>
    /// Factory: create a new Player from raw inputs.
    /// </summary>
    public static Player Create(
        int height,
        byte heightRating,
        byte weight,
        string country,
        RatingPresets ratings,
        Archetype archetype,
        Position position,
        byte age
    ) => new Player
    {
        Height          = height,
        HeightRating    = heightRating,
        Weight          = weight,
        Country         = country,
        Ratings         = ratings,
        Archetype       = archetype,
        Position        = position,
        Age             = age
    };
}
