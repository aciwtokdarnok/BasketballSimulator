using BasketballSimulator.Core.Extensions;
using BasketballSimulator.Core.Models.Player;

namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Domain logic to compute composite ratings (e.g. overall rating).
/// </summary>
public static class RatingCalculator
{
    /// <summary>
    /// OVR = 0.3 * avg(Physical) + 0.3 * avg(Shooting) + 0.4 * avg(Skill)
    /// </summary>
    public static byte ComputeOverall(byte heightRating, RatingPresets r)
    {
        double physical = (heightRating + r.Strength + r.Speed + r.Jumping + r.Endurance) / 5.0;
        double shooting = (r.Inside + r.DunksLayups + r.FreeThrows + r.MidRange + r.ThreePointers) / 5.0;
        double skill = (r.OffensiveIQ + r.DefensiveIQ + r.Dribbling + r.Passing + r.Rebounding) / 5.0;

        double raw = 0.3 * physical + 0.3 * shooting + 0.4 * skill;

        int ovr = (int)Math.Round(raw);
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

        double raw = 72.3 - 2.33 * age + 0.83 * overall;

        var rng = new Random();
        double noise = rng.NextGaussian(0, 3);
        raw += noise;

        int pot = (int)Math.Round(raw);
        return (byte)Math.Clamp(pot, 0, 100);
    }
}