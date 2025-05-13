using System;
using System.Reflection;
using BasketballSimulator.Core.Extensions;
using BasketballSimulator.Core.Models.Players;

namespace BasketballSimulator.Core.Utilities.Players
{
    /// <summary>
    /// Handles per-season development of player ratings based on age, potential, and attribute group.
    /// </summary>
    public static class PlayerDevelopmentService
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Executes a full season's development on the specified player.
        /// Develops all attribute groups, intangibles, and adjusts potential.
        /// </summary>
        /// <param name="player">Target player (will be mutated).</param>
        public static void DevelopSeason(Player player)
        {
            // 1. Young players may grow in height (rare, only up to age 21)
            GrowHeightIfYoung(player);

            int age = player.Age;
            // 2. Calculate the base change for this age (youth = growth, veteran = decline)
            double baseChange = CalculateBaseChange(age);
            // 3. Calculate a bonus based on the gap between potential and overall (bigger gap = more growth possible)
            double bonusMean = age <= 30 ? (player.Potential - player.Overall) * 0.1 : 0;

            // 4. Develop each attribute group using reflection and group-specific logic
            DevelopGroup(player.OutsideScoring, age, baseChange, bonusMean, AttributeGroup.OutsideScoring);
            DevelopGroup(player.Athleticism, age, baseChange, bonusMean, AttributeGroup.Athleticism);
            DevelopGroup(player.InsideScoring, age, baseChange, bonusMean, AttributeGroup.InsideScoring);
            DevelopGroup(player.Playmaking, age, baseChange, bonusMean, AttributeGroup.Playmaking);
            DevelopGroup(player.Defense, age, baseChange, bonusMean, AttributeGroup.Defense);
            DevelopGroup(player.Rebounding, age, baseChange, bonusMean, AttributeGroup.Rebounding);

            // 5. Develop intangibles (single value, not a group)
            // Intangibles: Steadily increase with age, plateau or slight decline after late 30s
            double intangiblesAgeMod = age switch
            {
                <= 22 => 0,
                <= 26 => 1,
                <= 30 => 2,
                <= 34 => 3,
                <= 38 => 4,
                _ => 2 // slight decline after late 30s
            };
            player.Intangibles = DevelopSingle(player.Intangibles, age, baseChange + intangiblesAgeMod, bonusMean, -3, 6);

            // 6. Update total attributes (sum of all ratings, for stats/record-keeping)
            player.TotalAttributes = CalculateTotalAttributes(player);

            // 7. Adjust potential after ratings have been updated (see method for details)
            AdjustPotential(player);

            // 8. Ensure Overall never exceeds Potential (hard cap)
            if (player.Overall > player.Potential)
            {
                player.Potential = player.Overall;
            }
        }

        #region Attribute Development

        /// <summary>
        /// Enum for attribute group identification.
        /// </summary>
        private enum AttributeGroup
        {
            OutsideScoring,
            Athleticism,
            InsideScoring,
            Playmaking,
            Defense,
            Rebounding
        }

        /// <summary>
        /// Develops all int properties in a given attribute group using age and group-specific logic.
        /// Uses reflection to generically update all group properties.
        /// </summary>
        private static void DevelopGroup(object group, int age, double baseChange, double bonusMean, AttributeGroup groupType)
        {
            foreach (var prop in group.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // HeightRating is only developed by GrowHeightIfYoung, not here
                if (groupType == AttributeGroup.Athleticism && prop.Name == "HeightRating")
                    continue;

                int current = (int)prop.GetValue(group)!;
                var (min, max) = GetChangeLimits(groupType, prop.Name, age);
                double ageMod = GetAgeModifier(groupType, prop.Name, age);

                // Bonus is higher if potential is much higher than overall (young, high-potential players)
                double bonus = age <= 30 ? _random.NextGaussian(bonusMean, 1) : 0;
                // Raw change is the sum of base, age modifier, and bonus
                double rawChange = baseChange + ageMod + bonus;
                // Random scaling to add variance between players
                double scaledChange = rawChange * (0.4 + _random.NextDouble());
                // Clamp to group/attribute-specific min/max
                double bounded = Math.Clamp(scaledChange, min, max);

                // Clamp final value to [0, 100] and set property
                int updated = LimitRating((int)Math.Round(current + bounded));
                prop.SetValue(group, updated);
            }
        }

        /// <summary>
        /// Develops a single int property (e.g., Intangibles) using age and base change.
        /// </summary>
        private static int DevelopSingle(int value, int age, double baseChange, double bonusMean, int min, int max)
        {
            double bonus = age <= 30 ? _random.NextGaussian(bonusMean, 1) : 0;
            double rawChange = baseChange + bonus;
            double scaledChange = rawChange * (0.4 + _random.NextDouble());
            double bounded = Math.Clamp(scaledChange, min, max);
            return LimitRating((int)Math.Round(value + bounded));
        }

        /// <summary>
        /// Returns min/max change limits for a property in a group, can be fine-tuned per attribute.
        /// </summary>
        private static (double min, double max) GetChangeLimits(AttributeGroup group, string prop, int age)
        {
            // These limits are based on NBA 2K-style attribute progression and can be tuned per attribute
            return group switch
            {
                AttributeGroup.Athleticism when prop is "Speed" or "Agility" or "Vertical" => (-12, 2),
                AttributeGroup.Athleticism when prop == "Strength" => (-4, 4),
                AttributeGroup.Athleticism => (-6, 4),
                AttributeGroup.OutsideScoring or AttributeGroup.InsideScoring => (-3, 13),
                AttributeGroup.Playmaking => (-2, 7),
                AttributeGroup.Defense => (-4, 8),
                AttributeGroup.Rebounding => (-3, 7),
                _ => (-3, 7)
            };
        }

        /// <summary>
        /// Returns an age-based modifier for a property in a group, can be fine-tuned per attribute.
        /// </summary>
        private static double GetAgeModifier(AttributeGroup group, string prop, int age)
        {
            switch (group)
            {
                case AttributeGroup.Athleticism:
                    // Athleticism peaks early, then declines
                    return prop switch
                    {
                        "Speed" or "Agility" => age switch
                        {
                            <= 24 => 1.0,
                            <= 27 => 0,
                            <= 30 => -2,
                            <= 35 => -3,
                            <= 40 => -4,
                            _ => -8
                        },
                        "Vertical" => age switch
                        {
                            <= 24 => 1.0,
                            <= 27 => 0,
                            <= 30 => -2,
                            <= 35 => -3,
                            <= 40 => -4,
                            _ => -8
                        },
                        "Stamina" => age switch
                        {
                            <= 23 => 1.0,
                            <= 30 => 0,
                            <= 35 => -2,
                            <= 40 => -4,
                            _ => -8
                        },
                        "Strength" => age switch
                        {
                            <= 25 => 1.0,
                            <= 32 => 2.0,
                            <= 36 => 1.0,
                            _ => 0
                        },
                        "Hustle" => age switch
                        {
                            <= 25 => 0,
                            <= 32 => 1.0,
                            <= 36 => 0.5,
                            _ => 0
                        },
                        "OverallDurability" => age switch
                        {
                            <= 28 => 0,
                            <= 34 => -1.0,
                            _ => -2.0
                        },
                        _ => 0
                    };

                case AttributeGroup.OutsideScoring:
                    // Outside scoring improves with experience, then plateaus/declines
                    return age switch
                    {
                        <= 22 => 0,
                        <= 27 => 1.0,
                        <= 32 => 2.0,
                        <= 36 => 1.0,
                        _ => -1.0
                    };

                case AttributeGroup.InsideScoring:
                    // Inside scoring improves with experience, peaks late 20s/early 30s
                    return age switch
                    {
                        <= 22 => 0,
                        <= 27 => 1.0,
                        <= 32 => 2.0,
                        <= 36 => 1.0,
                        _ => -1.0
                    };

                case AttributeGroup.Playmaking:
                    // Playmaking improves with age, peaks early 30s, then plateaus
                    return age switch
                    {
                        <= 22 => 0,
                        <= 27 => 1.0,
                        <= 32 => 2.0,
                        <= 36 => 2.0,
                        _ => 1.0
                    };

                case AttributeGroup.Defense:
                    // Defense peaks late 20s, then slow decline
                    return age switch
                    {
                        <= 22 => 0,
                        <= 27 => 1.0,
                        <= 32 => 2.0,
                        <= 36 => 1.0,
                        _ => -1.0
                    };

                case AttributeGroup.Rebounding:
                    // Rebounding peaks mid/late 20s, then slow decline
                    return age switch
                    {
                        <= 22 => 0,
                        <= 27 => 1.0,
                        <= 32 => 1.5,
                        <= 36 => 1.0,
                        _ => -1.0
                    };

                default:
                    return 0;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Adjusts a player's potential:
        /// - For ages <29: adds random noise [-2,2] (simulates uncertainty in young players).
        /// - For ages >34: decays potential towards current overall, more steeply with age (older players lose upside).
        /// </summary>
        private static void AdjustPotential(Player player)
        {
            int age = player.Age;
            int currentPot = player.Potential;
            int currentOvr = player.Overall;
            int newPot = currentPot;

            if (age < 29)
            {
                // Young players: add a little random noise to potential
                int noise = _random.NextInt(-2, 2);
                newPot = Math.Clamp(currentPot + noise, 0, 100);
            }
            else if (age > 34)
            {
                // Old players: potential decays towards overall, faster with age
                int diff = currentPot - currentOvr;
                if (diff > 0)
                {
                    // Decay factor increases by 10% per year over 34, capped at 50%
                    double decayFactor = Math.Min((age - 34) * 0.1, 0.5);
                    int reducedDiff = (int)Math.Round(diff * (1 - decayFactor));
                    newPot = Math.Clamp(currentOvr + reducedDiff, 0, 100);
                }
            }

            // Only update if changed (avoids unnecessary property sets)
            if (newPot != player.Potential)
            {
                player.Potential = newPot;
            }
        }

        /// <summary>
        /// Allows young players to grow in height (HeightRating) with a small probability.
        /// Only applies up to age 21 and if HeightRating < 100.
        /// </summary>
        private static void GrowHeightIfYoung(Player player)
        {
            if (player.Athleticism.HeightRating >= 100) return;
            if (player.Age > 21) return;

            int roll = _random.NextInt(1, 1000);
            // Very rare: 0.1% chance, or 1% if age <= 20
            if ((roll > 999 || (roll > 990 && player.Age <= 20)) && player.Athleticism.HeightRating < 100)
            {
                // Create a new Athleticism object to update HeightRating (if it's init-only)
                player.Athleticism = new Athleticism
                {
                    HeightRating = player.Athleticism.HeightRating + 1,
                    Speed = player.Athleticism.Speed,
                    Agility = player.Athleticism.Agility,
                    Strength = player.Athleticism.Strength,
                    Vertical = player.Athleticism.Vertical,
                    Stamina = player.Athleticism.Stamina,
                    Hustle = player.Athleticism.Hustle,
                    OverallDurability = player.Athleticism.OverallDurability
                };
            }
        }

        /// <summary>
        /// Calculates the base change for attribute development based on age and random noise.
        /// Younger players grow, older players decline.
        /// </summary>
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

            // Add random noise: more variance for young players
            double noise = age switch
            {
                <= 23 => Math.Clamp(_random.NextGaussian(0, 5), -4, 20),
                <= 25 => Math.Clamp(_random.NextGaussian(0, 5), -4, 10),
                _ => Math.Clamp(_random.NextGaussian(0, 3), -2, 4)
            };

            return baseVal + noise;
        }

        /// <summary>
        /// Clamps a rating to the [0, 100] range.
        /// </summary>
        private static int LimitRating(int value)
            => Math.Clamp(value, 0, 100);

        /// <summary>
        /// Sums all int properties in all attribute groups and intangibles for total attributes.
        /// </summary>
        private static int CalculateTotalAttributes(Player player)
        {
            int sum = 0;
            foreach (var group in new object[] {
                player.OutsideScoring, player.Athleticism, player.InsideScoring,
                player.Playmaking, player.Defense, player.Rebounding
            })
            {
                foreach (var prop in group.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (prop.PropertyType == typeof(int))
                        sum += (int)prop.GetValue(group)!;
                }
            }
            sum += player.Intangibles;
            return sum;
        }

        #endregion
    }
}
