using BasketballSimulator.Core.Enums;

namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Encapsulates the logic for choosing an Archetype (Point/Wing/Big)
/// based on a height rating and a bit of randomness.
/// </summary>
public static class ArchetypeSelector
{
    private static readonly Random _rng = new();

    /// <summary>
    /// Decides a player’s archetype from their height rating (0–100).
    /// Taller players skew toward Big, shorter toward Point, middle heights mix.
    /// </summary>
    /// <param name="heightRating">Height rating in [0,100].</param>
    /// <returns>The chosen <see cref="Archetype"/>.</returns>
    public static Archetype Select(int heightRating)
    {
        double roll = _rng.NextDouble();

        return heightRating switch
        {
            // Very tall: almost always Big
            >= 59 => roll < 0.01 ? Archetype.Point
                   : roll < 0.05 ? Archetype.Wing
                                 : Archetype.Big,

            // Very short: almost always Point
            <= 33 => roll < 0.1 ? Archetype.Wing
                                : Archetype.Point,

            // Everything else: mix (70% wing)
            _ => roll < 0.03 ? Archetype.Point
               : roll < 0.30 ? Archetype.Big
                             : Archetype.Wing
        };
    }
}