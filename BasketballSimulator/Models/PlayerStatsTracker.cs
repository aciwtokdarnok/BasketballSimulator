public class PlayerStatsTracker
{
    public Dictionary<Player, PlayerStats> TeamAStats = new();
    public Dictionary<Player, PlayerStats> TeamBStats = new();

    public void InitializeStats(List<Player> teamA, List<Player> teamB)
    {
        foreach (var player in teamA)
            TeamAStats[player] = new PlayerStats();
        foreach (var player in teamB)
            TeamBStats[player] = new PlayerStats();
    }

    public PlayerStats GetStats(Player player)
    {
        if (TeamAStats.ContainsKey(player)) return TeamAStats[player];
        return TeamBStats[player];
    }

    public void DisplayStats(string teamName, List<Player> team, Dictionary<Player, PlayerStats> stats)
    {
        Console.WriteLine($"\nStatystyki drużyny {teamName}:");
        for (int i = 0; i < team.Count; i++)
        {
            var p = team[i];
            var s = stats[p];
            Console.WriteLine($"Gracz {teamName[teamName.Length - 1]}{i + 1}: {s.Points} pts, FG: {s.FieldGoalsMade}/{s.FieldGoalsAttempted}, 3PT: {s.ThreePointsMade}/{s.ThreePointsAttempted}, FT: {s.FreeThrowsMade}/{s.FreeThrowsAttempted}, AST: {s.Assists}, ORB: {s.OffensiveRebounds}, DRB: {s.DefensiveRebounds}, BLK: {s.Blocks}, STL: {s.Steals}, TO: {s.Turnovers}");
        }
    }

    // Aktualizacje podczas meczu
    public void AddPoints(Player p, int points)
    {
        var s = GetStats(p);
        s.Points += points;
    }

    public void AddFieldGoal(Player p, bool made, bool isThree)
    {
        var s = GetStats(p);
        s.FieldGoalsAttempted++;
        if (isThree) s.ThreePointsAttempted++;
        if (made)
        {
            s.FieldGoalsMade++;
            if (isThree) s.ThreePointsMade++;
        }
    }

    public void AddAssist(Player p)
    {
        GetStats(p).Assists++;
    }

    public void AddFreeThrow(Player p, bool made)
    {
        var s = GetStats(p);
        s.FreeThrowsAttempted++;
        if (made) s.FreeThrowsMade++;
    }

    public void AddRebound(Player p, bool offensive)
    {
        var s = GetStats(p);
        if (offensive) s.OffensiveRebounds++;
        else s.DefensiveRebounds++;
    }

    public void AddBlock(Player p)
    {
        GetStats(p).Blocks++;
    }

    public void AddSteal(Player p)
    {
        GetStats(p).Steals++;
    }

    public void AddTurnover(Player p)
    {
        GetStats(p).Turnovers++;
    }

    // Wywołać w BasketballGame po meczu
    public void DisplayAll(string teamAName, List<Player> teamA, string teamBName, List<Player> teamB)
    {
        DisplayStats(teamAName, teamA, TeamAStats);
        DisplayStats(teamBName, teamB, TeamBStats);
    }
}