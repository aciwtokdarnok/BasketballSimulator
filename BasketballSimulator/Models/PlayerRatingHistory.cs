using BasketballSimulator.Core.Enums;

namespace BasketballSimulator.Core.Models;

/// <summary>
/// Represents a snapshot of a player's ratings for one season.
/// </summary>
public class PlayerRatingHistory
{
    public int Year { get; init; }
    public string Team { get; init; }
    public byte Age { get; init; }
    public Position Position { get; init; }
    public byte Height { get; init; }
    public byte HeightRating { get; init; }
    public byte Weight { get; init; }
    public byte Strength { get; init; }
    public byte Speed { get; init; }
    public byte Jumping { get; init; }
    public byte Endurance { get; init; }
    public byte Inside { get; init; }
    public byte DunksLayups { get; init; }
    public byte FreeThrows { get; init; }
    public byte MidRange { get; init; }
    public byte ThreePointers { get; init; }
    public byte OffensiveIQ { get; init; }
    public byte DefensiveIQ { get; init; }
    public byte Dribbling { get; init; }
    public byte Passing { get; init; }
    public byte Rebounding { get; init; }
    public byte Overall { get; init; }
    public byte Potential { get; init; }
}