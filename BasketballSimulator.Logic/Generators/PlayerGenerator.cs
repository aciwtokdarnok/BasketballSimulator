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
    public Player Generate(byte age)
    {
        // 1) Sample raw height in inches and add a small random jitter (-0.5 to +0.5 in).
        double rawHeight = HeightExtensions.NextHeightInches() + _rng.NextDouble() - 0.5;

        // 2) Compute height rating from adjusted wingspan, mapped to [0–100].
        double wingspanAdj = rawHeight + _rng.NextInt(-1, 1);
        byte heightRating = HeightExtensions.HeightToRating(wingspanAdj);

        // 3) Select archetype (Point/Wing/Big) based on height rating.
        Archetype archetype = ArchetypeSelector.Select(heightRating);

        // 4) Build global multipliers for athleticism, shooting, skill, and inside.
        var factors = new RatingFactors(
            Athleticism: _rng.NextGaussian(1, 0.2).Clamp(0.2, 1.2),
            Shooting: _rng.NextGaussian(1, 0.2).Clamp(0.2, 1.2),
            Skill: _rng.NextGaussian(1, 0.2).Clamp(0.2, 1.2),
            Inside: _rng.NextGaussian(1, 0.2).Clamp(0.2, 1.2)
        );

        // 5) Randomize base ratings (from RatingPresets) using archetype & factors.
        RatingPresets finalRatings = RatingRandomizer.Randomize(
            baseRatings: new RatingPresets(),
            archetype: archetype,
            factors: factors,
            rng: _rng
        );

        // 6) Determine the best-fit on-court position.
        Position position = PositionCalculator.ComputePosition(heightRating, finalRatings);

        // 7) Construct and return an immutable Player via the factory method.
        return Player.Create(
            height:    heightRating,
            ratings:   finalRatings,
            archetype: archetype,
            position:  position,
            age:       age        
        );
    }
}