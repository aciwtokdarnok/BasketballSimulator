using BasketballSimulator.Core.Enums.Player;
using BasketballSimulator.Core.Models.Player;

namespace BasketballSimulator.Core.Utilities.Player;

public static class PositionCalculator
{
    // Reference values for each position
    private static readonly Dictionary<Position, double> PosValues = new()
    {
        [Position.PointGuard] = 0.0,  // PG
        [Position.ShootingGuard] = 1.0,  // SG
        [Position.SmallForward] = 2.0,  // SF
        [Position.PowerForward] = 3.0,  // PF
        [Position.Center] = 4.0,  // C
        [Position.Guard] = 0.5,  // G  (PG/SG hybrid)
        [Position.GuardForward] = 1.5,  // GF (SG/SF hybrid)
        [Position.ForwardCenter] = 3.5   // FC (PF/C hybrid)
    };

    /// <summary>
    /// Computes the best‐fit <see cref="Position"/> for a player.
    /// </summary>
    /// <param name="heightRating">Height rating (0–100).</param>
    /// <param name="r">All other court ratings in a <see cref="RatingPresets"/>.</param>
    /// <returns>
    /// The <see cref="Position"/> whose reference value is closest 
    /// to the regression result.
    /// </returns>
    public static Position ComputePosition(int heightRating, RatingPresets r)
    {
        // Regression formula
        double value =
              -0.922949
            + 0.073339 * heightRating
            + 0.009744 * r.Strength
            - 0.002215 * r.Speed
            - 0.005438 * r.Jumping
            + 0.003006 * r.Endurance
            - 0.003516 * r.Inside
            - 0.008239 * r.DunksLayups
            + 0.001647 * r.FreeThrows
            - 0.001404 * r.MidRange
            - 0.004599 * r.ThreePointers
            + 0.001407 * r.DefensiveIQ
            + 0.002433 * r.OffensiveIQ
            - 0.000753 * r.Dribbling
            - 0.021888 * r.Passing
            + 0.016867 * r.Rebounding;

        // Find the position with the minimal absolute difference
        Position bestPos = Position.None;
        double bestDiff = double.MaxValue;

        foreach (var kv in PosValues)
        {
            double diff = Math.Abs(value - kv.Value);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                bestPos = kv.Key;
            }
        }

        return bestPos;
    }
}