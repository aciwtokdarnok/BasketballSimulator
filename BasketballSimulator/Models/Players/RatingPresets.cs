namespace BasketballSimulator.Core.Models.Players;

public class RatingPresets
{
    // OutsideScoring
    public int CloseShot { get; set; }
    public int MidRangeShot { get; set; }
    public int ThreePointShot { get; set; }
    public int FreeThrow { get; set; }
    public int ShotIQ { get; set; }
    public int OffensiveConsistency { get; set; }

    // Athleticism
    public int HeightRating { get; set; }
    public int Speed { get; set; }
    public int Agility { get; set; }
    public int Strength { get; set; }
    public int Vertical { get; set; }
    public int Stamina { get; set; }
    public int Hustle { get; set; }
    public int OverallDurability { get; set; }

    // InsideScoring
    public int Layup { get; set; }
    public int StandingDunk { get; set; }
    public int DrivingDunk { get; set; }
    public int PostHook { get; set; }
    public int PostFade { get; set; }
    public int PostControl { get; set; }
    public int DrawFoul { get; set; }
    public int Hands { get; set; }

    // Playmaking
    public int PassAccuracy { get; set; }
    public int BallHandle { get; set; }
    public int SpeedWithBall { get; set; }
    public int PassIQ { get; set; }
    public int PassVision { get; set; }

    // Defense
    public int InteriorDefense { get; set; }
    public int PerimeterDefense { get; set; }
    public int Steal { get; set; }
    public int Block { get; set; }
    public int HelpDefenseIQ { get; set; }
    public int PassPerception { get; set; }
    public int DefensiveConsistency { get; set; }

    // Rebounding
    public int OffensiveRebound { get; set; }
    public int DefensiveRebound { get; set; }

    // Miscellaneous
    public int Intangibles { get; set; }

    public RatingPresets()
    {
        // OutsideScoring
        CloseShot = DefautRatingValues.CloseShot;
        MidRangeShot = DefautRatingValues.MidRangeShot;
        ThreePointShot = DefautRatingValues.ThreePointShot;
        FreeThrow = DefautRatingValues.FreeThrow;
        ShotIQ = DefautRatingValues.ShotIQ;
        OffensiveConsistency = DefautRatingValues.OffensiveConsistency;

        // Athleticism
        HeightRating = DefautRatingValues.HeightRating;
        Speed = DefautRatingValues.Speed;
        Agility = DefautRatingValues.Agility;
        Strength = DefautRatingValues.Strength;
        Vertical = DefautRatingValues.Vertical;
        Stamina = DefautRatingValues.Stamina;
        Hustle = DefautRatingValues.Hustle;
        OverallDurability = DefautRatingValues.OverallDurability;

        // InsideScoring
        Layup = DefautRatingValues.Layup;
        StandingDunk = DefautRatingValues.StandingDunk;
        DrivingDunk = DefautRatingValues.DrivingDunk;
        PostHook = DefautRatingValues.PostHook;
        PostFade = DefautRatingValues.PostFade;
        PostControl = DefautRatingValues.PostControl;
        DrawFoul = DefautRatingValues.DrawFoul;
        Hands = DefautRatingValues.Hands;

        // Playmaking
        PassAccuracy = DefautRatingValues.PassAccuracy;
        BallHandle = DefautRatingValues.BallHandle;
        SpeedWithBall = DefautRatingValues.SpeedWithBall;
        PassIQ = DefautRatingValues.PassIQ;
        PassVision = DefautRatingValues.PassVision;

        // Defense
        InteriorDefense = DefautRatingValues.InteriorDefense;
        PerimeterDefense = DefautRatingValues.PerimeterDefense;
        Steal = DefautRatingValues.Steal;
        Block = DefautRatingValues.Block;
        HelpDefenseIQ = DefautRatingValues.HelpDefenseIQ;
        PassPerception = DefautRatingValues.PassPerception;
        DefensiveConsistency = DefautRatingValues.DefensiveConsistency;

        // Rebounding
        OffensiveRebound = DefautRatingValues.OffensiveRebound;
        DefensiveRebound = DefautRatingValues.DefensiveRebound;

        // Miscellaneous
        Intangibles = DefautRatingValues.Intangibles;
    }
}
