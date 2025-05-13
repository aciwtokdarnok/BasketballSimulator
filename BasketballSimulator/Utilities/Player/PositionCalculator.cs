using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Models.Players;

namespace BasketballSimulator.Core.Utilities.Players;

public static class PositionCalculator
{
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
        // Point Guard (PG)
        double pgScore =
            0.18 * r.BallHandle +
            0.16 * r.PassAccuracy +
            0.12 * r.PassIQ +
            0.12 * r.Speed +
            0.10 * r.Agility +
            0.08 * r.ThreePointShot +
            0.08 * r.ShotIQ +
            0.06 * r.PerimeterDefense +
            0.05 * r.FreeThrow +
            0.05 * r.Intangibles;

        // Shooting Guard (SG)
        double sgScore =
            0.20 * r.ThreePointShot +
            0.15 * r.MidRangeShot +
            0.12 * r.Speed +
            0.10 * r.BallHandle +
            0.10 * r.ShotIQ +
            0.08 * r.Layup +
            0.08 * r.PerimeterDefense +
            0.07 * r.OffensiveConsistency +
            0.05 * r.Intangibles +
            0.05 * r.FreeThrow;

        // Small Forward (SF)
        double sfScore =
            0.15 * r.MidRangeShot +
            0.13 * r.DrivingDunk +
            0.12 * r.PerimeterDefense +
            0.10 * r.Strength +
            0.10 * r.Speed +
            0.08 * r.Layup +
            0.08 * r.Hustle +
            0.07 * r.OffensiveRebound +
            0.07 * r.ShotIQ +
            0.05 * r.Intangibles +
            0.05 * r.FreeThrow;

        // Power Forward (PF)
        double pfScore =
            0.15 * r.Strength +
            0.13 * r.PostControl +
            0.12 * r.StandingDunk +
            0.10 * r.Block +
            0.10 * r.OffensiveRebound +
            0.10 * r.DefensiveRebound +
            0.08 * r.HelpDefenseIQ +
            0.07 * r.Layup +
            0.05 * r.Intangibles +
            0.05 * r.MidRangeShot +
            0.05 * r.FreeThrow;

        // Center (C)
        double cScore =
            0.15 * heightRating +
            0.13 * r.Strength +
            0.12 * r.StandingDunk +
            0.12 * r.Block +
            0.10 * r.InteriorDefense +
            0.10 * r.OffensiveRebound +
            0.10 * r.DefensiveRebound +
            0.08 * r.OverallDurability +
            0.05 * r.Intangibles +
            0.05 * r.Layup;

        // Hybrid positions as averages
        double gScore = (pgScore + sgScore) / 2.0;
        double gfScore = (sgScore + sfScore) / 2.0;
        double fScore = (sfScore + pfScore) / 2.0;
        double fcScore = (pfScore + cScore) / 2.0;

        var scores = new Dictionary<Position, double>
        {
            [Position.PointGuard] = pgScore,
            [Position.ShootingGuard] = sgScore,
            [Position.SmallForward] = sfScore,
            [Position.PowerForward] = pfScore,
            [Position.Center] = cScore,
            [Position.Guard] = gScore,
            [Position.GuardForward] = gfScore,
            [Position.Forward] = fScore,
            [Position.ForwardCenter] = fcScore
        };

        return scores.OrderByDescending(kv => kv.Value).First().Key;
    }
}