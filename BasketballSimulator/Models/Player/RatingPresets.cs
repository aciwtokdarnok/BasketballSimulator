namespace BasketballSimulator.Core.Models.Player;

public class RatingPresets
{
    public byte Strength { get; set; }
    public byte Speed { get; set; }
    public byte Jumping { get; set; }
    public byte Endurance { get; set; }
    public byte Inside { get; set; }
    public byte DunksLayups { get; set; }
    public byte FreeThrows { get; set; }
    public byte MidRange { get; set; }
    public byte ThreePointers { get; set; }
    public byte OffensiveIQ { get; set; }
    public byte DefensiveIQ { get; set; }
    public byte Dribbling { get; set; }
    public byte Passing { get; set; }
    public byte Rebounding { get; set; }

    public RatingPresets()
    {
        Strength = RatingDefaultValues.Strength;
        Speed = RatingDefaultValues.Speed;
        Jumping = RatingDefaultValues.Jumping;
        Endurance = RatingDefaultValues.Endurance;
        Inside = RatingDefaultValues.Inside;
        DunksLayups = RatingDefaultValues.DunksLayups;
        FreeThrows = RatingDefaultValues.FreeThrows;
        MidRange = RatingDefaultValues.MidRange;
        ThreePointers = RatingDefaultValues.ThreePointers;
        OffensiveIQ = RatingDefaultValues.OffensiveIQ;
        DefensiveIQ = RatingDefaultValues.DefensiveIQ;
        Dribbling = RatingDefaultValues.Dribbling;
        Passing = RatingDefaultValues.Passing;
        Rebounding = RatingDefaultValues.Rebounding;
    }
}