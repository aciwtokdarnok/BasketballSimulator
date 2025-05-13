using BasketballSimulator.Core.Enums.Players;

namespace BasketballSimulator.Core.Models.Players;

/// <summary>
/// Represents a snapshot of a player's ratings for one season.
/// </summary>
public class PlayerRatingHistory
{
    public int Year { get; set; }
    public string Team { get; set; }
    public int Age { get; set; }
    public Position Position { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public OutsideScoring OutsideScoring { get; set; }
    public Athleticism Athleticism { get; set; }
    public InsideScoring InsideScoring { get; set; }
    public Playmaking Playmaking { get; set; }
    public Defense Defense { get; set; }
    public Rebounding Rebounding { get; set; }
    public int Intangibles { get; set; }
    public int TotalAttributes { get; set; }
    public int Overall { get; set; }
    public int Potential { get; set; }
}
