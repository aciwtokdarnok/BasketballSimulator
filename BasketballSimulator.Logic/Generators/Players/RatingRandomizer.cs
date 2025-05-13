using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Models.Players;
using BasketballSimulator.Infrastructure.Data.Players;
using BasketballSimulator.Logic.Extensions;
using System.Reflection;

namespace BasketballSimulator.Logic.Generators.Players;

/// <summary>
/// Applies archetype‐ and group‐based modifiers plus Gaussian noise
/// to a <see cref="RatingPresets"/>, producing randomized final ratings.
/// </summary>
public static class RatingRandomizer
{
    /// <summary>
    /// Take the base ratings, an archetype, a random seed,
    /// and global factors, then return a new RatingPresets
    /// with every field adjusted and clamped to [0,100].
    /// </summary>
    public static RatingPresets Randomize(
        RatingPresets baseRatings,
        Archetype archetype,
        RatingFactors factors,
        Random rng)
    {
        var result = new RatingPresets();
        // Map property name to group for factor selection
        static string GetGroup(string propName) => propName switch
        {
            // OutsideScoring
            nameof(RatingPresets.CloseShot) or
            nameof(RatingPresets.MidRangeShot) or
            nameof(RatingPresets.ThreePointShot) or
            nameof(RatingPresets.FreeThrow) or
            nameof(RatingPresets.ShotIQ) or
            nameof(RatingPresets.OffensiveConsistency)
                => "OutsideScoring",

            // Athleticism
            nameof(RatingPresets.HeightRating) or
            nameof(RatingPresets.Speed) or
            nameof(RatingPresets.Agility) or
            nameof(RatingPresets.Strength) or
            nameof(RatingPresets.Vertical) or
            nameof(RatingPresets.Stamina) or
            nameof(RatingPresets.Hustle) or
            nameof(RatingPresets.OverallDurability)
                => "Athleticism",

            // InsideScoring
            nameof(RatingPresets.Layup) or
            nameof(RatingPresets.StandingDunk) or
            nameof(RatingPresets.DrivingDunk) or
            nameof(RatingPresets.PostHook) or
            nameof(RatingPresets.PostFade) or
            nameof(RatingPresets.PostControl) or
            nameof(RatingPresets.DrawFoul) or
            nameof(RatingPresets.Hands)
                => "InsideScoring",

            // Playmaking
            nameof(RatingPresets.PassAccuracy) or
            nameof(RatingPresets.BallHandle) or
            nameof(RatingPresets.SpeedWithBall) or
            nameof(RatingPresets.PassIQ) or
            nameof(RatingPresets.PassVision)
                => "Playmaking",

            // Defense
            nameof(RatingPresets.InteriorDefense) or
            nameof(RatingPresets.PerimeterDefense) or
            nameof(RatingPresets.Steal) or
            nameof(RatingPresets.Block) or
            nameof(RatingPresets.HelpDefenseIQ) or
            nameof(RatingPresets.PassPerception) or
            nameof(RatingPresets.DefensiveConsistency)
                => "Defense",

            // Rebounding
            nameof(RatingPresets.OffensiveRebound) or
            nameof(RatingPresets.DefensiveRebound)
                => "Rebounding",

            // Miscellaneous
            nameof(RatingPresets.Intangibles)
                => "Intangibles",

            _ => "OutsideScoring"
        };

        double GetFactor(string group) => group switch
        {
            "OutsideScoring" => factors.OutsideScoring,
            "Athleticism" => factors.Athleticism,
            "InsideScoring" => factors.InsideScoring,
            "Playmaking" => factors.Playmaking,
            "Defense" => factors.Defense,
            "Rebounding" => factors.Rebounding,
            "Intangibles" => factors.Intangibles,
            _ => 1.0
        };

        var props = typeof(RatingPresets)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int) && p.CanRead && p.CanWrite);

        foreach (var prop in props)
        {
            var rawVal = (int)prop.GetValue(baseRatings)!;
            string group = GetGroup(prop.Name);
            double global = GetFactor(group);

            // get type‐factor by archetype+property name
            double typeFactor = TypeFactors.For(archetype, prop.Name);

            double noisy = rng.NextGaussian(rawVal, 3) * typeFactor * global;
            prop.SetValue(result, (int)Math.Round(Math.Clamp(noisy, 0, 100)));
        }

        return result;
    }
}