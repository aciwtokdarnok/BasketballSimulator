namespace BasketballSimulator.Core.Models.Players;

/// <summary>
/// Holds multipliers for each attribute group.
/// </summary>
public record RatingFactors(
    double OutsideScoring,
    double Athleticism,
    double InsideScoring,
    double Playmaking,
    double Defense,
    double Rebounding,
    double Intangibles
);