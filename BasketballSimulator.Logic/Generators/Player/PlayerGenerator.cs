using BasketballSimulator.Core.Enums.Player;
using BasketballSimulator.Core.Interfaces.Player;
using BasketballSimulator.Core.Models.Player;
using BasketballSimulator.Core.Utilities.Player;
using BasketballSimulator.Logic.Extensions;
using BasketballSimulator.Logic.Extensions.Player;

namespace BasketballSimulator.Logic.Generators.Player;

/// <summary>
/// Generates new <see cref="Player"/> instances with randomized ratings and attributes.
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
    public PlayerGenerator(Random rng)
    {
        _rng = rng;
    }

    /// <summary>
    /// Creates a fully populated <see cref="Player"/>.
    /// </summary>
    /// <returns>A new <see cref="Player"/> with height, ratings, overall, potential, and position set.</returns>
    public Core.Models.Player.Player Generate(byte age)
    {
        double rawHeight = HeightExtensions.NextHeightInches() + _rng.NextDouble() - 0.5;
        double wingspanAdj = rawHeight + _rng.NextInt(-1, 1);
        byte heightRating = HeightExtensions.HeightToRating(wingspanAdj);
        string country = CountrySelector.Select();
        string fullName = NameSelector.GenerateFullName(country);

        Archetype archetype = ArchetypeSelector.Select(heightRating);

        // Build global multipliers for athleticism, shooting, skill, and inside.
        var factors = new RatingFactors(
            Athleticism: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.25),
            Shooting: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.25),
            Skill: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.25),
            Inside: _rng.NextGaussian(1, 0.2).Clamp(0.5, 1.25)
        );

        // Randomize base ratings (from RatingPresets) using archetype & factors.
        RatingPresets finalRatings = RatingRandomizer.Randomize(
            baseRatings: new RatingPresets(),
            archetype: archetype,
            factors: factors,
            rng: _rng
        );

        byte weight = WeightHelper.GenerateWeight(heightRating, finalRatings.Strength);

        // Determine the best-fit on-court position.
        Position position = PositionCalculator.ComputePosition(heightRating, finalRatings);

        // Construct and return an immutable Player via the factory method.
        return Core.Models.Player.Player.Create(
            height:         (int)(rawHeight * 2.54),
            heightRating:   heightRating,
            weight:         weight,
            country:        country,
            fullName:       fullName,
            ratings:        finalRatings,
            archetype:      archetype,
            position:       position,
            age:            age        
        );
    }
}