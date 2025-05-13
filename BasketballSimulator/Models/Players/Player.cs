using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Utilities.Players;

namespace BasketballSimulator.Core.Models.Players;

public class OutsideScoring
{
    public int CloseShot { get; set; }
    public int MidRangeShot { get; set; }
    public int ThreePointShot { get; set; }
    public int FreeThrow { get; set; }
    public int ShotIQ { get; set; }
    public int OffensiveConsistency { get; set; }
}

public class Athleticism
{
    public int HeightRating { get; init; }
    public int Speed { get; set; }
    public int Agility { get; set; }
    public int Strength { get; set; }
    public int Vertical { get; set; }
    public int Stamina { get; set; }
    public int Hustle { get; set; }
    public int OverallDurability { get; set; }
}

public class InsideScoring
{
    public int Layup { get; set; }
    public int StandingDunk { get; set; }
    public int DrivingDunk { get; set; }
    public int PostHook { get; set; }
    public int PostFade { get; set; }
    public int PostControl { get; set; }
    public int DrawFoul { get; set; }
    public int Hands { get; set; }
}

public class Playmaking
{
    public int PassAccuracy { get; set; }
    public int BallHandle { get; set; }
    public int SpeedWithBall { get; set; }
    public int PassIQ { get; set; }
    public int PassVision { get; set; }
}

public class Defense
{
    public int InteriorDefense { get; set; }
    public int PerimeterDefense { get; set; }
    public int Steal { get; set; }
    public int Block { get; set; }
    public int HelpDefenseIQ { get; set; }
    public int PassPerception { get; set; }
    public int DefensiveConsistency { get; set; }
}

public class Rebounding
{
    public int OffensiveRebound { get; set; }
    public int DefensiveRebound { get; set; }
}

/// <summary>
/// Represents a basketball player with core ratings and metadata.
/// Overall is computed on-the-fly. Potential is calculated once on creation.
/// </summary>
public class Player
{
    // Metadata
    public int Height { get; init; }
    public int Weight { get; init; }
    public string Country { get; init; }
    public string FullName { get; init; }
    public Archetype Archetype { get; init; }
    public Position Position { get; init; }
    public int Age { get; set; }
    public int Potential { get; set; }

    // Attribute groups
    public OutsideScoring OutsideScoring { get; set; } = new();
    public Athleticism Athleticism { get; set; } = new();
    public InsideScoring InsideScoring { get; set; } = new();
    public Playmaking Playmaking { get; set; } = new();
    public Defense Defense { get; set; } = new();
    public Rebounding Rebounding { get; set; } = new();

    // Miscellaneous
    public int Intangibles { get; set; }
    public int TotalAttributes { get; set; }


    /// <summary>
    /// Overall rating, computed as a weighted regression + fudge factor.
    /// Always up-to-date with ratings.
    /// </summary>
    public int Overall => RatingCalculator.ComputeOverall(this);

    /// <summary>
    /// Factory: create a new Player from raw inputs.
    /// Potential is calculated once here.
    /// </summary>
    public static Player Create(
        int height,
        int weight,
        string country,
        string fullName,
        Archetype archetype,
        Position position,
        int age,
        OutsideScoring outsideScoring,
        Athleticism athleticism,
        InsideScoring insideScoring,
        Playmaking playmaking,
        Defense defense,
        Rebounding rebounding,
        int intangibles,
        int totalAttributes
    )
    {
        // Create the player instance with all grouped attributes
        var player = new Player
        {
            Height = height,
            Weight = weight,
            Country = country,
            FullName = fullName,
            Archetype = archetype,
            Position = position,
            Age = age,
            OutsideScoring = outsideScoring,
            Athleticism = athleticism,
            InsideScoring = insideScoring,
            Playmaking = playmaking,
            Defense = defense,
            Rebounding = rebounding,
            Intangibles = intangibles,
            TotalAttributes = totalAttributes,
            // Potential will be set below
        };

        // Compute potential based on initial overall and age
        player.Potential = RatingCalculator.ComputePotential(player.Overall, player.Age);

        // Add first history entry
        player.SnapshotRatings(2025, "AAA");

        return player;
    }


    // Navigation property for rating history
    public List<PlayerRatingHistory> RatingHistory { get; init; } = new();

    /// <summary>
    /// Capture a snapshot of the player's ratings for a given season.
    /// </summary>
    /// <param name="year">Season year (e.g. 2025).</param>
    /// <param name="team">Team name or abbreviation.</param>
    public void SnapshotRatings(int year, string team)
    {
        var entry = new PlayerRatingHistory
        {
            Year = year,
            Team = team,
            Age = Age,
            Position = Position,
            Height = Height,
            Weight = Weight,
            OutsideScoring = new OutsideScoring
            {
                CloseShot = OutsideScoring.CloseShot,
                MidRangeShot = OutsideScoring.MidRangeShot,
                ThreePointShot = OutsideScoring.ThreePointShot,
                FreeThrow = OutsideScoring.FreeThrow,
                ShotIQ = OutsideScoring.ShotIQ,
                OffensiveConsistency = OutsideScoring.OffensiveConsistency
            },
            Athleticism = new Athleticism
            {
                HeightRating = Athleticism.HeightRating,
                Speed = Athleticism.Speed,
                Agility = Athleticism.Agility,
                Strength = Athleticism.Strength,
                Vertical = Athleticism.Vertical,
                Stamina = Athleticism.Stamina,
                Hustle = Athleticism.Hustle,
                OverallDurability = Athleticism.OverallDurability
            },
            InsideScoring = new InsideScoring
            {
                Layup = InsideScoring.Layup,
                StandingDunk = InsideScoring.StandingDunk,
                DrivingDunk = InsideScoring.DrivingDunk,
                PostHook = InsideScoring.PostHook,
                PostFade = InsideScoring.PostFade,
                PostControl = InsideScoring.PostControl,
                DrawFoul = InsideScoring.DrawFoul,
                Hands = InsideScoring.Hands
            },
            Playmaking = new Playmaking
            {
                PassAccuracy = Playmaking.PassAccuracy,
                BallHandle = Playmaking.BallHandle,
                SpeedWithBall = Playmaking.SpeedWithBall,
                PassIQ = Playmaking.PassIQ,
                PassVision = Playmaking.PassVision
            },
            Defense = new Defense
            {
                InteriorDefense = Defense.InteriorDefense,
                PerimeterDefense = Defense.PerimeterDefense,
                Steal = Defense.Steal,
                Block = Defense.Block,
                HelpDefenseIQ = Defense.HelpDefenseIQ,
                PassPerception = Defense.PassPerception,
                DefensiveConsistency = Defense.DefensiveConsistency
            },
            Rebounding = new Rebounding
            {
                OffensiveRebound = Rebounding.OffensiveRebound,
                DefensiveRebound = Rebounding.DefensiveRebound
            },
            Intangibles = Intangibles,
            TotalAttributes = TotalAttributes,
            Overall = Overall,
            Potential = Potential
        };

        RatingHistory.Add(entry);
    }


    /// <summary>
    /// Advances the player by one season: applies development,
    /// increments age, and snapshots the new ratings.
    /// </summary>
    /// <param name="nextYear">The upcoming season year.</param>
    /// <param name="team">Team name or abbreviation for the new season.</param>
    public void Develop(int nextYear, string team)
    {
        // Apply development to ratings and height
        PlayerDevelopmentService.DevelopSeason(this);

        // Bump age by one year
        Age += 1;

        // Snapshot the new state
        SnapshotRatings(nextYear, team);
    }
}
