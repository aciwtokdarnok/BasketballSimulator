namespace BasketballSimulator.Core.Models;

/// <summary>
/// Holds the four global multipliers used when randomizing ratings.
/// </summary>
public record RatingFactors(
    double Athleticism,
    double Shooting,
    double Skill,
    double Inside
);