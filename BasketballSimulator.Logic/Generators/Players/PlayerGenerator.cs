using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Interfaces.Players;
using BasketballSimulator.Core.Models.Players;
using BasketballSimulator.Core.Utilities.Players;
using BasketballSimulator.Logic.Extensions;
using BasketballSimulator.Logic.Extensions.Players;

namespace BasketballSimulator.Logic.Generators.Players;

/// <summary>
/// Generates new <see cref="Players"/> instances with randomized ratings and attributes.
/// </summary>
public class PlayerGenerator : IPlayerGenerator
{
    private readonly Random _rng;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerGenerator"/> class.
    /// </summary>
    /// <param name="rng">
    /// A <see cref="Random"/> number generator, ideally injected for testability.
    /// </param>
    public PlayerGenerator(Random rng) => _rng = rng;

    /// <summary>
    /// Creates a fully populated <see cref="Players"/>.
    /// </summary>
    /// <returns>A new <see cref="Players"/> with height, ratings, overall, potential, and position set.</returns>
    public Player Generate(int age, Archetype? targetArchetype = null)
    {
        while (true) // to ensure we get a valid archetype
        {
            double rawHeight = HeightExtensions.NextHeightInches() + _rng.NextDouble() - 0.5;
            double wingspanAdj = rawHeight + _rng.NextInt(-1, 1);
            byte heightRating = HeightExtensions.HeightToRating(wingspanAdj);
            string country = CountrySelector.Select();
            string fullName = NameSelector.GenerateFullName(country);

            Archetype archetype = ArchetypeSelector.Select(heightRating);
            if (targetArchetype.HasValue && targetArchetype != archetype)
                continue;

            // Build group multipliers
            var factors = new RatingFactors(
                OutsideScoring: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                Athleticism: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                InsideScoring: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                Playmaking: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                Defense: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                Rebounding: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3),
                Intangibles: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.3)
            );

            // Randomize base ratings (from RatingPresets) using archetype & factors.
            // Randomize all ratings
            RatingPresets finalRatings = RatingRandomizer.Randomize(
                baseRatings: new RatingPresets(),
                archetype: archetype,
                factors: factors,
                rng: _rng
            );

            int weight = WeightHelper.GenerateWeight(heightRating, finalRatings.Strength);

            // Determine the best-fit on-court position.
            Position position = PositionCalculator.ComputePosition(heightRating, finalRatings);

            // Map RatingPresets to attribute groups
            var outside = new OutsideScoring
            {
                CloseShot = finalRatings.CloseShot,
                MidRangeShot = finalRatings.MidRangeShot,
                ThreePointShot = finalRatings.ThreePointShot,
                FreeThrow = finalRatings.FreeThrow,
                ShotIQ = finalRatings.ShotIQ,
                OffensiveConsistency = finalRatings.OffensiveConsistency
            };
            var athleticism = new Athleticism
            {
                HeightRating = finalRatings.HeightRating,
                Speed = finalRatings.Speed,
                Agility = finalRatings.Agility,
                Strength = finalRatings.Strength,
                Vertical = finalRatings.Vertical,
                Stamina = finalRatings.Stamina,
                Hustle = finalRatings.Hustle,
                OverallDurability = finalRatings.OverallDurability
            };
            var inside = new InsideScoring
            {
                Layup = finalRatings.Layup,
                StandingDunk = finalRatings.StandingDunk,
                DrivingDunk = finalRatings.DrivingDunk,
                PostHook = finalRatings.PostHook,
                PostFade = finalRatings.PostFade,
                PostControl = finalRatings.PostControl,
                DrawFoul = finalRatings.DrawFoul,
                Hands = finalRatings.Hands
            };
            var playmaking = new Playmaking
            {
                PassAccuracy = finalRatings.PassAccuracy,
                BallHandle = finalRatings.BallHandle,
                SpeedWithBall = finalRatings.SpeedWithBall,
                PassIQ = finalRatings.PassIQ,
                PassVision = finalRatings.PassVision
            };
            var defense = new Defense
            {
                InteriorDefense = finalRatings.InteriorDefense,
                PerimeterDefense = finalRatings.PerimeterDefense,
                Steal = finalRatings.Steal,
                Block = finalRatings.Block,
                HelpDefenseIQ = finalRatings.HelpDefenseIQ,
                PassPerception = finalRatings.PassPerception,
                DefensiveConsistency = finalRatings.DefensiveConsistency
            };
            var rebounding = new Rebounding
            {
                OffensiveRebound = finalRatings.OffensiveRebound,
                DefensiveRebound = finalRatings.DefensiveRebound
            };

            return Player.Create(
                height: (int)(rawHeight * 2.54),
                weight: weight,
                country: country,
                fullName: fullName,
                archetype: archetype,
                position: position,
                age: age,
                outsideScoring: outside,
                athleticism: athleticism,
                insideScoring: inside,
                playmaking: playmaking,
                defense: defense,
                rebounding: rebounding,
                intangibles: finalRatings.Intangibles,
                totalAttributes: 0 // You may want to sum all attributes here
            );
        }
    }
}