using BasketballSimulator.Core.Enums.Team;
using BasketballSimulator.Core.Models.Team;

namespace BasketballSimulator.Core.Data.Team;

/// <summary>
/// Provides access to the complete list of NBA teams.
/// </summary>
public static class TeamRepository
{
    /// <summary>
    /// Returns a readonly list of all 30 NBA teams with their metadata.
    /// </summary>
    public static IReadOnlyList<Models.Team.Team> GetAllTeams() => new List<Models.Team.Team>
        {
            Models.Team.Team.Create("Celtics", "Boston", "TD Garden", 1946, Conference.Eastern, Division.Atlantic),
            Models.Team.Team.Create("Nets", "Brooklyn", "Barclays Center", 1967, Conference.Eastern, Division.Atlantic),
            Models.Team.Team.Create("Knicks", "Manhattan", "Madison Square Garden", 1946, Conference.Eastern, Division.Atlantic),
            Models.Team.Team.Create("76ers", "Philadelphia", "Wells Fargo Center", 1939, Conference.Eastern, Division.Atlantic),
            Models.Team.Team.Create("Raptors", "Toronto", "Scotiabank Arena", 1995, Conference.Eastern, Division.Atlantic),

            Models.Team.Team.Create("Bulls", "Chicago", "United Center", 1966, Conference.Eastern, Division.Central),
            Models.Team.Team.Create("Cavaliers", "Cleveland", "Rocket Mortgage FieldHouse", 1970, Conference.Eastern, Division.Central),
            Models.Team.Team.Create("Pistons", "Detroit", "Little Caesars Arena", 1941, Conference.Eastern, Division.Central),
            Models.Team.Team.Create("Pacers", "Indianapolis", "Bankers Life Fieldhouse", 1967, Conference.Eastern, Division.Central),
            Models.Team.Team.Create("Bucks", "Milwaukee", "Fiserv Forum", 1968, Conference.Eastern, Division.Central),

            Models.Team.Team.Create("Hawks", "Atlanta", "State Farm Arena", 1946, Conference.Eastern, Division.Southeast),
            Models.Team.Team.Create("Hornets", "Charlotte", "Spectrum Center", 1988, Conference.Eastern, Division.Southeast),
            Models.Team.Team.Create("Heat", "Miami", "American Airlines Arena", 1988, Conference.Eastern, Division.Southeast),
            Models.Team.Team.Create("Magic", "Orlando", "Amway Center", 1989, Conference.Eastern, Division.Southeast),
            Models.Team.Team.Create("Wizards", "Washington", "Capital One Arena", 1961, Conference.Eastern, Division.Southeast),

            Models.Team.Team.Create("Nuggets", "Denver", "Ball Arena", 1967, Conference.Western, Division.Northwest),
            Models.Team.Team.Create("Timberwolves", "Minneapolis", "Target Center", 1989, Conference.Western, Division.Northwest),
            Models.Team.Team.Create("Thunder", "Oklahoma City", "Chesapeake Energy Arena", 2008, Conference.Western, Division.Northwest),
            Models.Team.Team.Create("Trail Blazers", "Portland", "Moda Center", 1970, Conference.Western, Division.Northwest),
            Models.Team.Team.Create("Jazz", "Salt Lake City", "Vivint Smart Home Arena", 1974, Conference.Western, Division.Northwest),

            Models.Team.Team.Create("Warriors", "San Francisco", "Chase Center", 1946, Conference.Western, Division.Pacific),
            Models.Team.Team.Create("Clippers", "Los Angeles", "Intuit Dome", 1970, Conference.Western, Division.Pacific),
            Models.Team.Team.Create("Lakers", "Los Angeles", "Crypto.com Arena", 1946, Conference.Western, Division.Pacific),
            Models.Team.Team.Create("Suns", "Phoenix", "Footprint Center", 1968, Conference.Western, Division.Pacific),
            Models.Team.Team.Create("Kings", "Sacramento", "Golden 1 Center", 1945, Conference.Western, Division.Pacific),

            Models.Team.Team.Create("Mavericks", "Dallas", "American Airlines Center", 1980, Conference.Western, Division.Southwest),
            Models.Team.Team.Create("Rockets", "Houston", "Toyota Center", 1967, Conference.Western, Division.Southwest),
            Models.Team.Team.Create("Grizzlies", "Memphis", "FedExForum", 1995, Conference.Western, Division.Southwest),
            Models.Team.Team.Create("Pelicans", "New Orleans", "Smoothie King Center", 2003, Conference.Western, Division.Southwest),
            Models.Team.Team.Create("Spurs", "San Antonio", "AT&T Center", 1967, Conference.Western, Division.Southwest)
        };
}
