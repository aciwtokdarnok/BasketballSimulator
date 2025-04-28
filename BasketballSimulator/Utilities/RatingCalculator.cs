using BasketballSimulator.Core.Models;

namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Domain logic to compute composite ratings (e.g. overall rating).
/// </summary>
public static class RatingCalculator
{
    /// <summary>
    /// Compute a player’s overall rating by applying weighted averages
    /// and a fudge factor.
    /// </summary>
    /// <param name="heightRating">
    ///   The “height” rating (0–100) produced by height‐to‐rating mapping.
    /// </param>
    /// <param name="r">All the other in‐court ratings (0–100).</param>
    /// <returns>The overall rating, clamped to [0,100].</returns>
    public static byte ComputeOverall(byte heightRating, RatingPresets r)
    {
        // 1) Weighted sum
        double raw =
              0.159 * (heightRating - 47.5)
            + 0.0777 * (r.Strength - 50.2)
            + 0.123 * (r.Speed - 50.8)
            + 0.051 * (r.Jumping - 48.7)
            + 0.0632 * (r.Endurance - 39.9)
            + 0.0126 * (r.Inside - 42.4)
            + 0.0286 * (r.DunksLayups - 49.5)
            + 0.0202 * (r.FreeThrows - 47.0)
            + 0.0726 * (r.ThreePointers - 47.1)
            + 0.133 * (r.OffensiveIQ - 46.8)
            + 0.159 * (r.DefensiveIQ - 46.7)
            + 0.059 * (r.Dribbling - 54.8)
            + 0.062 * (r.Passing - 51.3)
            + 0.01 * (r.MidRange - 47.0)
            + 0.01 * (r.Rebounding - 51.4)
            + 48.5;

        // 2) Fudge factor
        double fudge;
        if (raw >= 68) fudge = 8;
        else if (raw >= 50) fudge = 4 + (raw - 50) * (4.0 / 18.0);
        else if (raw >= 42) fudge = -5 + (raw - 42) * (9.0 / 8.0);
        else if (raw >= 31) fudge = -5 - (42 - raw) * (5.0 / 11.0);
        else fudge = -10;

        // 3) Round and clamp to [0,100]
        int ovr = (int)Math.Round(raw + fudge);
        return (byte)Math.Clamp(ovr, 0, 100);
    }

    /// <summary>
    /// Estimate a player’s potential based on current overall and age.
    /// </summary>
    /// <param name="overall">Current overall rating (0–100).</param>
    /// <param name="age">Player’s age in years.</param>
    /// <returns>
    ///   The estimated potential rating, rounded and clamped to [0,100].
    ///   If age ≥ 29, returns the current overall.
    /// </returns>
    public static byte ComputePotential(byte overall, byte age)
    {
        if (age >= 29)
        {
            return overall;
        }

        double raw = 72.31428908571982
                   - 2.33062761 * age
                   + 0.83308748 * overall;

        int pot = (int)Math.Round(raw);
        return (byte)Math.Clamp(pot, 0, 100);
    }
}