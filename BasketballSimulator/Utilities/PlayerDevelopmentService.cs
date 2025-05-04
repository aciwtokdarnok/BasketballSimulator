using System;
using System.Collections.Generic;
using System.Reflection;
using BasketballSimulator.Core.Enums;
using BasketballSimulator.Core.Extensions;
using BasketballSimulator.Core.Models.Player;

namespace BasketballSimulator.Core.Utilities
{
    /// <summary>
    /// Handles per-season development of player ratings based on age, coaching, and potential gap.
    /// </summary>
    public static class PlayerDevelopmentService
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Encapsulates age-based adjustment and change limits for a rating.
        /// </summary>
        private record RatingFormula(Func<int, double> AgeModifier, Func<int, (double Min, double Max)> ChangeLimits);

        // Mapping of rating property names to their development formulas
        private static readonly IReadOnlyDictionary<string, RatingFormula> _formulas = new Dictionary<string, RatingFormula>
        {
            [nameof(RatingPresets.Strength)] = new(AgeModifier: age => 0, ChangeLimits: age => (double.NegativeInfinity, double.PositiveInfinity)),
            [nameof(RatingPresets.Speed)] = new(AgeModifier: AgeModSpeed, ChangeLimits: age => (-12, 2)),
            [nameof(RatingPresets.Jumping)] = new(AgeModifier: AgeModJumping, ChangeLimits: age => (-12, 2)),
            [nameof(RatingPresets.Endurance)] = new(AgeModifier: AgeModEndurance, ChangeLimits: age => (-11, 19)),
            [nameof(RatingPresets.DunksLayups)] = new(AgeModifier: age => age > 27 ? 0.5 : 0, ChangeLimits: age => (-3, 13)),
            [nameof(RatingPresets.Inside)] = new(AgeModifier: shootingAgeMod, ChangeLimits: _ => (-3, 13)),
            [nameof(RatingPresets.FreeThrows)] = new(AgeModifier: shootingAgeMod, ChangeLimits: _ => (-3, 13)),
            [nameof(RatingPresets.MidRange)] = new(AgeModifier: shootingAgeMod, ChangeLimits: _ => (-3, 13)),
            [nameof(RatingPresets.ThreePointers)] = new(AgeModifier: shootingAgeMod, ChangeLimits: _ => (-3, 13)),
            [nameof(RatingPresets.OffensiveIQ)] = new(AgeModifier: iqAgeMod, ChangeLimits: age => age >= 24 ? (-3, 9) : (-3, 7 + 5 * (24 - age))),
            [nameof(RatingPresets.DefensiveIQ)] = new(AgeModifier: iqAgeMod, ChangeLimits: age => age >= 24 ? (-3, 9) : (-3, 7 + 5 * (24 - age))),
            [nameof(RatingPresets.Dribbling)] = new(AgeModifier: shootingAgeMod, ChangeLimits: age => (-2, 5)),
            [nameof(RatingPresets.Passing)] = new(AgeModifier: shootingAgeMod, ChangeLimits: age => (-2, 5)),
            [nameof(RatingPresets.Rebounding)] = new(AgeModifier: shootingAgeMod, ChangeLimits: age => (-2, 5))
        };

        /// <summary>
        /// Executes a full season's development on the specified player.
        /// </summary>
        /// <param name="player">Target player (will be mutated).</param>
        /// <param name="coachingLevel">Coaching quality, influences base change.</param>
        public static void DevelopSeason(Player player)
        {
            GrowHeightIfYoung(player);

            int age = player.Age;
            double baseChange = CalculateBaseChange(age);
            double bonusMean = age <= 30 ? (player.Potential - player.Overall) * 0.1 : 0;

            foreach (var (propName, formula) in _formulas)
            {
                ApplyRatingChange(player.Ratings, propName, formula, age, baseChange, bonusMean);
            }
            // Adjust potential after ratings have been updated
            AdjustPotential(player);

            // Ensure Overall never exceeds Potential
            if (player.Overall > player.Potential)
            {
                typeof(Player)
                    .GetProperty(nameof(Player.Potential), BindingFlags.Public | BindingFlags.Instance)!
                    .SetValue(player, player.Overall);
            }
        }

        #region Private Helpers

        /// <summary>
        /// Adjusts a player's potential with a small random variation if under 29.
        /// </summary>
        /// <summary>
        /// Adjusts a player's potential:
        /// - For ages <29: adds random noise [-2,2].
        /// - For ages >34: decays potential towards current overall, more steeply with age.
        /// </summary>
        private static void AdjustPotential(Player player)
        {
            int age = player.Age;
            int currentPot = player.Potential;
            int currentOvr = player.Overall;

            byte newPot = (byte)currentPot;

            if (age < 29)
            {
                // Small random variation for young players
                int noise = _random.NextInt(-2, 2);
                newPot = (byte)Math.Clamp(currentPot + noise, 0, 100);
            }
            else if (age > 34)
            {
                // Gradual decay of potential: drop down towards overall
                int diff = currentPot - currentOvr;
                if (diff > 0)
                {
                    // Decay factor increases by 10% per year over 34, capped at 50%
                    double decayFactor = Math.Min((age - 34) * 0.1, 0.5);
                    int reducedDiff = (int)Math.Round(diff * (1 - decayFactor));
                    newPot = (byte)Math.Clamp(currentOvr + reducedDiff, 0, 100);
                }
            }

            // Update only if changed
            if (newPot != player.Potential)
            {
                typeof(Player)
                    .GetProperty(nameof(Player.Potential), BindingFlags.Public | BindingFlags.Instance)!
                    .SetValue(player, newPot);
            }
        }

        private static void GrowHeightIfYoung(Player player)
        {
            if (player.Age > 21) return;

            int roll = _random.NextInt(1, 1000);
            if ((roll > 999 || (roll > 990 && player.Age <= 20)) && player.HeightRating < 100)
            {
                // Reflection to overwrite init-only property
                typeof(Player)
                    .GetProperty(nameof(Player.HeightRating), BindingFlags.Public | BindingFlags.Instance)!
                    .SetValue(player, (byte)(player.HeightRating + 1));
            }
        }

        private static void ApplyRatingChange(
            RatingPresets ratings,
            string propertyName,
            RatingFormula formula,
            int age,
            double baseChange,
            double bonusMean)
        {
            var prop = ratings.GetType().GetProperty(propertyName)!;
            byte current = (byte)prop.GetValue(ratings)!;

            // Sample a normally-distributed bonus around bonusMean (σ=1)
            double bonus = age <= 30 ? _random.NextGaussian(bonusMean, 1) : 0;

            // Combine base change, age modifier, and potential bonus
            double rawChange = baseChange + formula.AgeModifier(age) + bonus;
            double scaledChange = rawChange * (0.4 + _random.NextDouble());

            var (min, max) = formula.ChangeLimits(age);
            double bounded = Math.Clamp(scaledChange, min, max);

            byte updated = LimitRating((int)Math.Round(current + bounded));
            prop.SetValue(ratings, updated);
        }

        private static double CalculateBaseChange(int age)
        {
            double baseVal = age switch
            {
                <= 21 => 2,
                <= 25 => 1,
                <= 27 => 0,
                <= 29 => -1,
                <= 31 => -2,
                <= 34 => -3,
                <= 40 => -4,
                <= 43 => -5,
                _ => -6
            };

            double noise = age switch
            {
                <= 23 => Math.Clamp(_random.NextGaussian(0, 5), -4, 20),
                <= 25 => Math.Clamp(_random.NextGaussian(0, 5), -4, 10),
                _ => Math.Clamp(_random.NextGaussian(0, 3), -2, 4)
            };

            return baseVal + noise;
        }

        private static double shootingAgeMod(int age)
            => age switch
            {
                <= 27 => 0,
                <= 29 => 0.5,
                <= 31 => 1.5,
                _ => 2
            };

        private static double iqAgeMod(int age)
            => age switch
            {
                <= 21 => 4,
                <= 23 => 3,
                <= 27 => 0,
                <= 29 => 0.5,
                <= 31 => 1.5,
                _ => 2
            };

        private static double AgeModSpeed(int age)
            => age switch
            {
                <= 27 => 0,
                <= 30 => -2,
                <= 35 => -3,
                <= 40 => -4,
                _ => -8
            };

        private static double AgeModJumping(int age)
            => age switch
            {
                <= 26 => 0,
                <= 30 => -2,
                <= 35 => -3,
                <= 40 => -4,
                _ => -8
            };

        private static double AgeModEndurance(int age)
        {
            if (age <= 23) return _random.NextDouble() * 9;
            return age switch
            {
                <= 30 => 0,
                <= 35 => -2,
                <= 40 => -4,
                _ => -8
            };
        }

        private static byte LimitRating(int value)
            => (byte)Math.Clamp(value, 0, 100);

        #endregion
    }
}
