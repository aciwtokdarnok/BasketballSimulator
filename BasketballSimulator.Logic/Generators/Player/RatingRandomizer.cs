using BasketballSimulator.Core.Enums.Player;
using BasketballSimulator.Core.Models.Player;
using BasketballSimulator.Infrastructure.Data.Player;
using BasketballSimulator.Logic.Extensions;
using System.Reflection;

namespace BasketballSimulator.Logic.Generators.Player;

/// <summary>
/// Applies archetype‐ and group‐based modifiers plus Gaussian noise
/// to a <see cref="RatingPresets"/>, producing randomized final ratings.
/// </summary>
public static class RatingRandomizer
{
    /// <summary>
    /// Take the base ratings, an archetype, a random seed,
    /// and the four global factors, then return a new RatingPresets
    /// with every field adjusted and clamped to [0,100].
    /// </summary>
    public static RatingPresets Randomize(
        RatingPresets baseRatings,
        Archetype archetype,
        RatingFactors factors,
        Random rng)
    {
        var result = new RatingPresets();

        byte Compute(byte rawValue, string propName)
        {
            // get type‐factor by archetype+property name
            double typeFactor = TypeFactors.For(archetype, propName);

            // pick the right global factor
            double global = RatingGroups.Athleticism.Contains(propName) ? factors.Athleticism
                          : RatingGroups.Shooting.Contains(propName)    ? factors.Shooting
                          : RatingGroups.Skill.Contains(propName)       ? factors.Skill
                                                                        : factors.Inside;

            // apply Box–Muller noise and both factors
            double noisy = rng.NextGaussian(rawValue, 3) * typeFactor * global;

            // clamp to [0–100], round and cast to byte
            return (byte)Math.Round(Math.Clamp(noisy, 0, 100));
        }

        // reflect over every byte‐typed property
        var props = typeof(RatingPresets)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(byte) && p.CanRead && p.CanWrite);

        foreach (var prop in props)
        {
            var rawVal = (byte)prop.GetValue(baseRatings)!;
            var newVal = Compute(rawVal, prop.Name);
            prop.SetValue(result, newVal);
        }

        return result;
    }
}