/// <summary>
/// Manages and tracks statistics for basketball players across multiple teams.
/// </summary>
public class StatsManager
{
    private readonly Dictionary<string, Dictionary<Player, PlayerStats>> _teamStats;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketballStatsManager"/> class.
    /// </summary>
    public StatsManager()
    {
        _teamStats = new Dictionary<string, Dictionary<Player, PlayerStats>>();
    }

    /// <summary>
    /// Initializes statistics for each player in the provided teams.
    /// </summary>
    /// <param name="teams">A map from team name to its roster of players.</param>
    public void InitializeStats(Dictionary<string, List<Player>> teams)
    {
        foreach (var kvp in teams)
        {
            string teamName = kvp.Key;
            List<Player> roster = kvp.Value;
            var stats = new Dictionary<Player, PlayerStats>();

            foreach (var player in roster)
            {
                stats[player] = new PlayerStats();
            }

            _teamStats[teamName] = stats;
        }
    }

    /// <summary>
    /// Retrieves the <see cref="PlayerStats"/> for the specified player.
    /// </summary>
    /// <param name="player">The player whose stats are requested.</param>
    /// <returns>The <see cref="PlayerStats"/> object for the player.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the player is not found on any team.</exception>
    public PlayerStats GetStats(Player player)
    {
        foreach (var team in _teamStats.Values)
        {
            if (team.TryGetValue(player, out var stats))
                return stats;
        }

        throw new KeyNotFoundException($"Player not found in any team.");
    }

    /// <summary>
    /// Increments the specified player's points by the given amount.
    /// </summary>
    /// <param name="player">The player to credit.</param>
    /// <param name="points">The number of points scored.</param>
    public void AddPoints(Player player, int points)
    {
        GetStats(player).Points += points;
    }

    /// <summary>
    /// Records a field goal attempt, tracking makes and differentiating three-pointers.
    /// </summary>
    /// <param name="player">The player attempting the shot.</param>
    /// <param name="made">Whether the shot was made.</param>
    /// <param name="isThree">Whether the field goal attempt is a three-pointer.</param>
    public void AddFieldGoal(Player player, bool made, bool isThree)
    {
        var stats = GetStats(player);
        stats.FieldGoalsAttempted++;
        if (isThree)
            stats.ThreePointsAttempted++;

        if (made)
        {
            stats.FieldGoalsMade++;
            if (isThree)
                stats.ThreePointsMade++;
        }
    }

    /// <summary>
    /// Increments the specified player's free throw attempts and makes.
    /// </summary>
    /// <param name="player">The player attempting the free throw.</param>
    /// <param name="made">Whether the free throw was made.</param>
    public void AddFreeThrow(Player player, bool made)
    {
        var stats = GetStats(player);
        stats.FreeThrowsAttempted++;
        if (made)
            stats.FreeThrowsMade++;
    }

    /// <summary>
    /// Adds an assist to the specified player.
    /// </summary>
    /// <param name="player">The player credited with the assist.</param>
    public void AddAssist(Player player)
    {
        GetStats(player).Assists++;
    }

    /// <summary>
    /// Records a rebound for the specified player, distinguishing offensive and defensive.
    /// </summary>
    /// <param name="player">The player grabbing the rebound.</param>
    /// <param name="offensive">True for offensive rebound; false for defensive.</param>
    public void AddRebound(Player player, bool offensive)
    {
        var stats = GetStats(player);
        if (offensive)
            stats.OffensiveRebounds++;
        else
            stats.DefensiveRebounds++;
    }

    /// <summary>
    /// Increments the specified player's block count.
    /// </summary>
    /// <param name="player">The player making the block.</param>
    public void AddBlock(Player player)
    {
        GetStats(player).Blocks++;
    }

    /// <summary>
    /// Increments the specified player's steal count.
    /// </summary>
    /// <param name="player">The player making the steal.</param>
    public void AddSteal(Player player)
    {
        GetStats(player).Steals++;
    }

    /// <summary>
    /// Increments the specified player's turnover count.
    /// </summary>
    /// <param name="player">The player committing the turnover.</param>
    public void AddTurnover(Player player)
    {
        GetStats(player).Turnovers++;
    }

    /// <summary>
    /// Displays statistics for all teams in the console.
    /// </summary>
    public void DisplayAllStats()
    {
        foreach (var kvp in _teamStats)
        {
            string teamName = kvp.Key;
            var roster = kvp.Value.Keys.ToList();
            var stats = kvp.Value;
            DisplayTeamStats(teamName, roster, stats);
        }
    }

    /// <summary>
    /// Displays statistics for a single team in the console.
    /// </summary>
    /// <param name="teamName">The team's name.</param>
    /// <param name="roster">The list of players on the team.</param>
    /// <param name="stats">The stats dictionary for the team.</param>
    private void DisplayTeamStats(string teamName, List<Player> roster, Dictionary<Player, PlayerStats> stats)
    {
        Console.WriteLine($"\nStats for team {teamName}:\n");

        // Header
        string header = string.Format(
            "{0,3} {1,-10} {2,4} {3,7} {4,7} {5,7} {6,7} {7,4} {8,4} {9,4} {10,4} {11,4} {12,4}",
            "Idx", "Player", "Pts", "FGM/A", "2PM/A", "3PM/A", "FTM/A",
            "AST", "ORB", "DRB", "BLK", "STL", "TO");
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < roster.Count; i++)
        {
            var p = roster[i];
            var s = stats[p];

            // Ratios
            string fg = $"{s.FieldGoalsMade}/{s.FieldGoalsAttempted}";
            // compute two-pointers as FG minus 3PT
            int twoMade = s.FieldGoalsMade - s.ThreePointsMade;
            int twoAttempted = s.FieldGoalsAttempted - s.ThreePointsAttempted;
            string twoPt = $"{twoMade}/{twoAttempted}";
            string tp = $"{s.ThreePointsMade}/{s.ThreePointsAttempted}";
            string ft = $"{s.FreeThrowsMade}/{s.FreeThrowsAttempted}";

            Console.WriteLine(string.Format(
                "{0,3} {1,-10} {2,4} {3,7} {4,7} {5,7} {6,7} {7,4} {8,4} {9,4} {10,4} {11,4} {12,4}",
                i + 1,
                $"{teamName} {i + 1}",
                s.Points,
                fg,
                twoPt,
                tp,
                ft,
                s.Assists,
                s.OffensiveRebounds,
                s.DefensiveRebounds,
                s.Blocks,
                s.Steals,
                s.Turnovers));
        }
    }
}
