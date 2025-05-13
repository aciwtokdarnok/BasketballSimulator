using BasketballSimulator.Core.Extensions;
using BasketballSimulator.Core.Models.Players;

namespace BasketballSimulator.Core.Utilities.Players;

/// <summary>
/// Domain logic to compute composite ratings (e.g. overall rating).
/// </summary>

public static class RatingCalculator
{
    /// <summary>
    /// Computes the overall rating using all grouped attributes from Player.
    /// Calibrated so a player with the provided 2K25-style attributes gets an overall ~98.
    /// </summary>
    public static int ComputeOverall(Player player)
    {
        // Outside Scoring (6 attributes)
        double outsideScoring = (
            player.OutsideScoring.CloseShot +
            player.OutsideScoring.MidRangeShot +
            player.OutsideScoring.ThreePointShot +
            player.OutsideScoring.FreeThrow +
            player.OutsideScoring.ShotIQ +
            player.OutsideScoring.OffensiveConsistency
        ) / 6.0;

        // Athleticism (7 attributes, excluding HeightRating)
        double athleticism = (
            player.Athleticism.Speed +
            player.Athleticism.Agility +
            player.Athleticism.Strength +
            player.Athleticism.Vertical +
            player.Athleticism.Stamina +
            player.Athleticism.Hustle +
            player.Athleticism.OverallDurability
        ) / 7.0;

        // Inside Scoring (8 attributes)
        double insideScoring = (
            player.InsideScoring.Layup +
            player.InsideScoring.StandingDunk +
            player.InsideScoring.DrivingDunk +
            player.InsideScoring.PostHook +
            player.InsideScoring.PostFade +
            player.InsideScoring.PostControl +
            player.InsideScoring.DrawFoul +
            player.InsideScoring.Hands
        ) / 8.0;

        // Playmaking (5 attributes)
        double playmaking = (
            player.Playmaking.PassAccuracy +
            player.Playmaking.BallHandle +
            player.Playmaking.SpeedWithBall +
            player.Playmaking.PassIQ +
            player.Playmaking.PassVision
        ) / 5.0;

        // Defense (7 attributes)
        double defense = (
            player.Defense.InteriorDefense +
            player.Defense.PerimeterDefense +
            player.Defense.Steal +
            player.Defense.Block +
            player.Defense.HelpDefenseIQ +
            player.Defense.PassPerception +
            player.Defense.DefensiveConsistency
        ) / 7.0;

        // Rebounding (2 attributes)
        double rebounding = (
            player.Rebounding.OffensiveRebound +
            player.Rebounding.DefensiveRebound
        ) / 2.0;

        // Intangibles
        double intangibles = player.Intangibles;

        // Example weights
        double raw =
            0.22 * outsideScoring +
            0.18 * athleticism +
            0.14 * insideScoring +
            0.16 * playmaking +
            0.16 * defense +
            0.06 * rebounding +
            0.08 * intangibles;

        int ovr = (int)Math.Round(raw);
        return Math.Clamp(ovr, 0, 100);
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
    public static int ComputePotential(int overall, int age)
    {
        if (age >= 29)
        {
            return overall;
        }

        double raw = 72 - 2.3 * age + 0.8 * overall;

        var rng = new Random();
        double noise = rng.NextGaussian(0, 3);
        raw += noise;

        int pot = (int)Math.Round(raw);
        return Math.Clamp(pot, 0, 100);
    }
}