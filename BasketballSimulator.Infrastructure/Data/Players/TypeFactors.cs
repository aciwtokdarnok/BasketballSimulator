using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Models.Players;

namespace BasketballSimulator.Infrastructure.Data.Players;

public static class TypeFactors
{
    private static readonly Dictionary<Archetype, Dictionary<string, double>> _data = new()
    {
        [Archetype.Point] = new Dictionary<string, double>
        {
            // OutsideScoring
            [nameof(RatingPresets.ThreePointShot)] = 1.4,
            [nameof(RatingPresets.MidRangeShot)] = 1.3,
            [nameof(RatingPresets.FreeThrow)] = 1.2,
            [nameof(RatingPresets.ShotIQ)] = 1.2,
            [nameof(RatingPresets.OffensiveConsistency)] = 1.1,
            // Athleticism
            [nameof(RatingPresets.Speed)] = 1.5,
            [nameof(RatingPresets.Agility)] = 1.4,
            [nameof(RatingPresets.Stamina)] = 1.2,
            [nameof(RatingPresets.Hustle)] = 1.1,
            // Playmaking
            [nameof(RatingPresets.PassAccuracy)] = 1.5,
            [nameof(RatingPresets.BallHandle)] = 1.5,
            [nameof(RatingPresets.SpeedWithBall)] = 1.3,
            [nameof(RatingPresets.PassIQ)] = 1.4,
            [nameof(RatingPresets.PassVision)] = 1.3,
            // Defense
            [nameof(RatingPresets.PerimeterDefense)] = 1.2,
            [nameof(RatingPresets.Steal)] = 1.1,
            // Misc
            [nameof(RatingPresets.Intangibles)] = 1.1,
        },
        [Archetype.Wing] = new Dictionary<string, double>
        {
            // OutsideScoring
            [nameof(RatingPresets.ThreePointShot)] = 1.3,
            [nameof(RatingPresets.MidRangeShot)] = 1.3,
            [nameof(RatingPresets.FreeThrow)] = 1.1,
            [nameof(RatingPresets.ShotIQ)] = 1.1,
            [nameof(RatingPresets.OffensiveConsistency)] = 1.2,
            // Athleticism
            [nameof(RatingPresets.Speed)] = 1.3,
            [nameof(RatingPresets.Agility)] = 1.3,
            [nameof(RatingPresets.Vertical)] = 1.2,
            [nameof(RatingPresets.Hustle)] = 1.2,
            // InsideScoring
            [nameof(RatingPresets.Layup)] = 1.3,
            [nameof(RatingPresets.DrivingDunk)] = 1.3,
            // Playmaking
            [nameof(RatingPresets.BallHandle)] = 1.2,
            [nameof(RatingPresets.PassAccuracy)] = 1.1,
            // Defense
            [nameof(RatingPresets.PerimeterDefense)] = 1.2,
            [nameof(RatingPresets.Steal)] = 1.1,
            [nameof(RatingPresets.Block)] = 1.1,
            // Rebounding
            [nameof(RatingPresets.OffensiveRebound)] = 1.1,
            [nameof(RatingPresets.DefensiveRebound)] = 1.1,
            // Misc
            [nameof(RatingPresets.Intangibles)] = 1.1,
        },
        [Archetype.Big] = new Dictionary<string, double>
        {
            // OutsideScoring
            [nameof(RatingPresets.FreeThrow)] = 0.9,
            [nameof(RatingPresets.ThreePointShot)] = 0.7,
            [nameof(RatingPresets.MidRangeShot)] = 0.8,
            // Athleticism
            [nameof(RatingPresets.Strength)] = 1.4,
            [nameof(RatingPresets.Vertical)] = 1.2,
            [nameof(RatingPresets.OverallDurability)] = 1.1,
            // InsideScoring
            [nameof(RatingPresets.Layup)] = 1.2,
            [nameof(RatingPresets.StandingDunk)] = 1.4,
            [nameof(RatingPresets.DrivingDunk)] = 1.2,
            [nameof(RatingPresets.PostHook)] = 1.3,
            [nameof(RatingPresets.PostFade)] = 1.2,
            [nameof(RatingPresets.PostControl)] = 1.4,
            [nameof(RatingPresets.DrawFoul)] = 1.1,
            [nameof(RatingPresets.Hands)] = 1.1,
            // Defense
            [nameof(RatingPresets.InteriorDefense)] = 1.4,
            [nameof(RatingPresets.Block)] = 1.4,
            [nameof(RatingPresets.HelpDefenseIQ)] = 1.2,
            [nameof(RatingPresets.DefensiveConsistency)] = 1.1,
            // Rebounding
            [nameof(RatingPresets.OffensiveRebound)] = 1.4,
            [nameof(RatingPresets.DefensiveRebound)] = 1.4,
            // Misc
            [nameof(RatingPresets.Intangibles)] = 1.1,
        },
    };

    public static double For(Archetype archetype, string propName)
        => _data.TryGetValue(archetype, out var dict) && dict.TryGetValue(propName, out var v) ? v : 1.0;
}
