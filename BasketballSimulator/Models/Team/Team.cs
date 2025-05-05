using BasketballSimulator.Core.Enums.Team;

namespace BasketballSimulator.Core.Models.Team;

/// <summary>
/// Represents an NBA team with core metadata and organizational attributes.
/// </summary>
public class Team
{
    /// <summary>
    /// Team's official nickname.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// City where the team is based.
    /// </summary>
    public string City { get; init; } = string.Empty;

    /// <summary>
    /// Home arena or stadium of the team.
    /// </summary>
    public string Arena { get; init; } = string.Empty;

    /// <summary>
    /// Year the franchise was founded.
    /// </summary>
    public int FoundedYear { get; init; }

    /// <summary>
    /// Conference in which the team competes.
    /// </summary>
    public Conference Conference { get; init; }

    /// <summary>
    /// Division within the conference.
    /// </summary>
    public Division Division { get; init; }

    /// <summary>
    /// The list of players currently on the team.
    /// </summary>
    public List<Player.Player> Roster { get; set; } = new();

    private Team() { }

    /// <summary>
    /// Creates a new instance of <see cref="Team"/>.
    /// </summary>
    /// <param name="name">Official team nickname.</param>
    /// <param name="city">Home city.</param>
    /// <param name="arena">Home arena name.</param>
    /// <param name="foundedYear">Year of establishment.</param>
    /// <param name="conference">Conference affiliation.</param>
    /// <param name="division">Division within the conference.</param>
    /// <returns>A fully-initialized <see cref="Team"/> instance.</returns>
    public static Team Create(
        string name,
        string city,
        string arena,
        int foundedYear,
        Conference conference,
        Division division
    ) => new Team
    {
        Name =          name,
        City =          city,
        Arena =         arena,
        FoundedYear =   foundedYear,
        Conference =    conference,
        Division =      division,
        Roster =        new List<Player.Player>()
    };
}
