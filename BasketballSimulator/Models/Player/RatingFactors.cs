namespace BasketballSimulator.Core.Models.Player;

/// <summary>
/// Holds the four global multipliers used when randomizing ratings.
/// </summary>
public record RatingFactors(
    double Athleticism,
    double Shooting,
    double Skill,
    double Inside
);