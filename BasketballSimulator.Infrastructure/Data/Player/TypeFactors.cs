using BasketballSimulator.Core.Enums.Player;
using BasketballSimulator.Core.Models.Player;

namespace BasketballSimulator.Infrastructure.Data.Player;

public static class TypeFactors
{
    private static readonly Dictionary<Archetype, Dictionary<string, double>> _data = new()
    {
        [Archetype.Point] = new Dictionary<string, double>
        {
            [nameof(RatingPresets.Jumping)] = 1.65,
            [nameof(RatingPresets.Speed)] = 1.65,
            [nameof(RatingPresets.Dribbling)] = 1.5,
            [nameof(RatingPresets.Passing)] = 1.5,
            [nameof(RatingPresets.FreeThrows)] = 1.4,
            [nameof(RatingPresets.MidRange)] = 1.4,
            [nameof(RatingPresets.ThreePointers)] = 1.4,
            [nameof(RatingPresets.OffensiveIQ)] = 1.2,
            [nameof(RatingPresets.Endurance)] = 1.4,
        },
        [Archetype.Wing] = new Dictionary<string, double>
        {
            [nameof(RatingPresets.Dribbling)] = 1.2,
            [nameof(RatingPresets.DunksLayups)] = 1.5,
            [nameof(RatingPresets.Jumping)] = 1.4,
            [nameof(RatingPresets.Speed)] = 1.4,
            [nameof(RatingPresets.FreeThrows)] = 1.2,
            [nameof(RatingPresets.MidRange)] = 1.2,
            [nameof(RatingPresets.ThreePointers)] = 1.2,
        },
        [Archetype.Big] = new Dictionary<string, double>
        {
            [nameof(RatingPresets.Strength)] = 1.2,
            [nameof(RatingPresets.Inside)] = 1.6,
            [nameof(RatingPresets.DunksLayups)] = 1.5,
            [nameof(RatingPresets.Rebounding)] = 1.4,
            [nameof(RatingPresets.FreeThrows)] = 0.8,
            [nameof(RatingPresets.MidRange)] = 0.8,
            [nameof(RatingPresets.ThreePointers)] = 0.8,
            [nameof(RatingPresets.DefensiveIQ)] = 1.2,
        },
    };

    public static double For(Archetype archetype, string propName)
        => _data[archetype].TryGetValue(propName, out var v) ? v : 1.0;
}