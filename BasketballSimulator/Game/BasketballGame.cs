/// <summary>
/// Simulates a basketball game with realistic NBA pace, turnovers, and scoring.
/// </summary>
public class BasketballGame
{
    private const int GameDurationSeconds = 48 * 60;
    private const int QuarterDurationSeconds = 12 * 60;
    private readonly List<Player> _teamA = new();
    private readonly List<Player> _teamB = new();
    private int _scoreA;
    private int _scoreB;
    private readonly Random _random = new();
    private PlayerStatsTracker _statsTracker;

    // Expose final scores
    public int ScoreA => _scoreA;
    public int ScoreB => _scoreB;

    // League average possession time (~15s)
    private readonly double _avgPossessionTime = 15.0;

    public BasketballGame()
    {
        _statsTracker = new PlayerStatsTracker();
    }

    private string FormatTime(int totalSeconds) => $"{totalSeconds / 60:D2}:{totalSeconds % 60:D2}";

    public void SetupTeams()
    {
        Position[] positions = new[]
        {
        Position.PointGuard,
        Position.ShootingGuard,
        Position.SmallForward,
        Position.PowerForward,
        Position.Center
    };

        foreach (var pos in positions)
        {
            _teamA.Add(PlayerGenerator.GeneratePlayerWithGaussian(_random, pos));
            _teamB.Add(PlayerGenerator.GeneratePlayerWithGaussian(_random, pos));
        }
    }

    /// <summary>
    /// Resets scores and player stats but keeps the same teams.
    /// </summary>
    public void ResetForSimulation()
    {
        _scoreA = 0;
        _scoreB = 0;
        _statsTracker.InitializeStats(_teamA, _teamB);
    }

    public void DisplayTeams()
    {
        Console.WriteLine("Team rosters:");
        PrintTeam("Team A", _teamA);
        PrintTeam("Team B", _teamB);

        var avgA = ComputeAverages(_teamA);
        var avgB = ComputeAverages(_teamB);
        Console.WriteLine($"\nTeam A averages: Overall {avgA.overall:F1}, 3PT {avgA.three:F1}, Mid {avgA.mid:F1}, Inside {avgA.inside:F1}");
        Console.WriteLine($"Team B averages: Overall {avgB.overall:F1}, 3PT {avgB.three:F1}, Mid {avgB.mid:F1}, Inside {avgB.inside:F1}");

        var (predA, predB) = PredictScore(avgA.overall, avgB.overall);
        Console.WriteLine($"\nPredicted final score: Team A {predA} - Team B {predB}\n");

        Console.WriteLine("Starting game simulation:\n");
    }

    private void PrintTeam(string name, List<Player> team)
    {
        Console.WriteLine($"\n{name}:\n");
        // Header with column widths
        string header = string.Format(
            "{0,3} {1,-3} {2,-3} {3,-2} {4,5} {5,4} {6,4} {7,4} {8,4} {9,4} {10,4} {11,4} {12,4} {13,4} {14,4} {15,4} {16,4} {17,4} {18,4} {19,4} {20,4}",
            "Idx", "Pos", "Hy", "R", "Ht", "OVR", "POT", "Str", "Spd", "Jmp", "End", "Ins", "D/L", "FT", "Mi", "3PT", "OIQ", "DIQ", "Drb", "Pas", "Reb");
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < team.Count; i++)
        {
            var p = team[i];
            // Format height as right-aligned within 5 spaces (e.g., "190cm")
            string ht = p.Height.ToString() + "cm";
            ht = ht.PadLeft(5);

            Console.WriteLine(string.Format(
                "{0,3} {1,-3} {2,-3} {3,-2} {4,5} {5,4} {6,4} {7,4} {8,4} {9,4} {10,4} {11,4} {12,4} {13,4} {14,4} {15,4} {16,4} {17,4} {18,4} {19,4} {20,4}",
                i + 1,
                GetPosAbbrev(p.Position),
                GetHybAbbrev(p.HybridPosition),
                GetRoleAbbrev(p.PlayerRole),
                ht,
                p.Overall,
                p.Potential,
                p.Strength,
                p.Speed,
                p.Jumping,
                p.Endurance,
                p.Inside,
                p.DunksLayups,
                p.FreeThrows,
                p.MidRange,
                p.ThreePointers,
                p.OffensiveIQ,
                p.DefensiveIQ,
                p.Dribbling,
                p.Passing,
                p.Rebounding));
        }
    }

    private string GetPosAbbrev(Position pos) => pos switch
    {
        Position.PointGuard => "PG",
        Position.ShootingGuard => "SG",
        Position.SmallForward => "SF",
        Position.PowerForward => "PF",
        Position.Center => "C",
        _ => pos.ToString().Substring(0, 2).ToUpper()
    };

    private string GetHybAbbrev(HybridPosition hyb) => hyb switch
    {
        HybridPosition.None => "-",
        HybridPosition.ComboGuard => "CG",
        HybridPosition.Wing => "WG",
        HybridPosition.ForwardSwing => "FS",
        HybridPosition.BigManSwing => "BS",
        _ => "-"
    };

    private string GetRoleAbbrev(PlayerRole role) => role switch
    {
        PlayerRole.Superstar => "SS",
        PlayerRole.AllStar => "AS",
        PlayerRole.Starter => "ST",
        PlayerRole.RolePlayer => "RP",
        PlayerRole.Rotation => "RT",
        PlayerRole.BenchWarmer => "BW",
        PlayerRole.Prospect => "PR",
        _ => role.ToString().Substring(0, 2).ToUpper()
    };

    private (double overall, double three, double mid, double inside) ComputeAverages(List<Player> team) =>
        (team.Average(p => p.Overall), team.Average(p => p.ThreePointers), team.Average(p => p.MidRange), team.Average(p => p.Inside));

    private (int predA, int predB) PredictScore(double avgA, double avgB)
    {
        double diff = avgA - avgB;
        int baseScore = 110;
        int margin = (int)Math.Round(diff * 1.5);
        return (baseScore + margin, baseScore - margin);
    }

    public void Simulate(bool verbose = true)
    {
        int elapsed = 0, quarter = 1;
        int nextQuarter = QuarterDurationSeconds;

        while (elapsed < GameDurationSeconds)
        {
            bool isA = ((elapsed / 24) % 2) == 0;
            var offense = isA ? _teamA : _teamB;
            var defense = isA ? _teamB : _teamA;
            var teamName = isA ? "Team A" : "Team B";

            int shooterIdx = GetWeightedRandomIndex(offense);
            var shooter = offense[shooterIdx];
            // Defender must be within one position difference from shooter
            var defenderCandidates = defense
                .Where(d => Math.Abs((int)d.Position - (int)shooter.Position) <= 1)
                .ToList();
            if (!defenderCandidates.Any())
                defenderCandidates = defense;
            var defender = defenderCandidates[_random.Next(defenderCandidates.Count)];

            int possessionTime = Math.Clamp((int)Math.Round(_random.NextGaussian(_avgPossessionTime, 4)), 6, 24);
            elapsed += possessionTime;
            if (elapsed >= nextQuarter)
            {
                quarter++;
                nextQuarter += QuarterDurationSeconds;
                if (verbose) Console.WriteLine($"\n--- Start of Quarter {quarter} ---\n");
            }
            if (elapsed >= GameDurationSeconds) break;

            ProcessPossession(isA, shooterIdx, shooter, defender, teamName, elapsed, verbose);
        }
    }

    private void ProcessPossession(bool isTeamA, int idx, Player shooter, Player defender, string team, int time, bool verbose)
    {
        // Turnover (~14% league avg) modified by player overall
        double baseTO = 0.14;
        double toChance = Math.Clamp(baseTO + (50 - shooter.Overall) / 1000.0, 0.05, 0.25);
        if (_random.NextDouble() < toChance)
        {
            _statsTracker.AddTurnover(shooter);
            if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} turnover!");
            return;
        }

        // Steal chance (~1.7% league avg)
        double stealBase = 0.017;
        double stealMod = (defender.DefensiveIQ - 50) / 1000.0 + (defender.Speed - 50) / 2000.0;
        double stealChance = Math.Clamp(stealBase + stealMod, 0.005, 0.10);
        if (_random.NextDouble() < stealChance)
        {
            _statsTracker.AddSteal(defender);
            if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} loses the ball, steal by defender!");
            return;
        }

        var action = ChooseAction(shooter);

        // Fouled on shot (~14% league avg)
        double foulRate = 0.14;
        if (_random.NextDouble() < foulRate)
        {
            ExecuteFreeThrows(isTeamA, idx, shooter, team, time, action, verbose);
            return;
        }

        // Block chance (inside/mid only, ~5% avg)
        if (action != ActionType.ThreePoint)
        {
            double blockChance = Math.Clamp((defender.DefensiveIQ + defender.Speed) / 2000.0, 0.02, 0.12);
            if (_random.NextDouble() < blockChance)
            {
                _statsTracker.AddBlock(defender);
                if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} shot blocked by defender!");
                return;
            }
        }

        double success = CalculateSuccessChance(shooter, defender, action);
        if (_random.NextDouble() < success)
            ExecuteMadeShot(isTeamA, idx, shooter, team, time, action, verbose);
        else
            ExecuteMissedShot(isTeamA, idx, shooter, defender, team, time, action, verbose);
    }

    private ActionType ChooseAction(Player shooter)
    {
        // 1) Base “raw” weights from stats
        double insideWeight = shooter.Inside;
        double midWeight = shooter.MidRange;
        double threeWeight = shooter.ThreePointers;

        // 2) Position multipliers (guards shoot more 3/MR, bigs more inside)
        switch (shooter.Position)
        {
            case Position.PointGuard:
                insideWeight *= 0.8;
                midWeight *= 1.1;
                threeWeight *= 1.2;
                break;
            case Position.ShootingGuard:
                insideWeight *= 0.9;
                midWeight *= 1.05;
                threeWeight *= 1.15;
                break;
            case Position.SmallForward:
                // pretty balanced
                insideWeight *= 1.0;
                midWeight *= 1.0;
                threeWeight *= 1.0;
                break;
            case Position.PowerForward:
                insideWeight *= 1.1;
                midWeight *= 0.9;
                threeWeight *= 0.7;
                break;
            case Position.Center:
                insideWeight *= 1.2;
                midWeight *= 0.8;
                threeWeight *= 0.5;
                break;
        }

        // 3) Height adjustment: taller players +10% inside per full height range
        //    (170cm→220cm maps to +0…+10% inside, and flips on 3PT)
        double heightFactor = (shooter.Height - 170) / 50.0;
        heightFactor = Math.Clamp(heightFactor, 0.0, 1.0);
        insideWeight *= 1 + heightFactor * 0.1;
        threeWeight *= 1 - heightFactor * 0.1;

        // 4) Clean up any negative weights
        insideWeight = Math.Max(0, insideWeight);
        midWeight = Math.Max(0, midWeight);
        threeWeight = Math.Max(0, threeWeight);

        // 5) Roll to choose
        double total = insideWeight + midWeight + threeWeight;
        double roll = _random.NextDouble() * total;
        if (roll < insideWeight)
            return ActionType.Inside;
        else if (roll < insideWeight + midWeight)
            return ActionType.MidRange;
        else
            return ActionType.ThreePoint;
    }

    private double CalculateSuccessChance(Player shooter, Player defender, ActionType action)
    {
        // Base probability = shooter stat normalized
        double shooterStat = action switch
        {
            ActionType.ThreePoint => shooter.ThreePointers,
            ActionType.MidRange => shooter.MidRange,
            _ => shooter.Inside
        } / 100.0;
        // Defense reduction
        double defenseMod = (defender.DefensiveIQ + defender.Speed) / 5000.0;
        double raw = shooterStat - defenseMod;
        // Clamp to realistic NBA ranges
        double min = action switch { ActionType.ThreePoint => 0.28, ActionType.MidRange => 0.33, _ => 0.45 };
        double max = action switch { ActionType.ThreePoint => 0.42, ActionType.MidRange => 0.50, _ => 0.65 };
        return Math.Clamp(raw, min, max);
    }

    private double GetFreeThrowChance(Player shooter)
    {
        // Shooter FT% directly
        return Math.Clamp(shooter.FreeThrows / 100.0, 0.50, 0.95);
    }

    private void ExecuteFreeThrows(bool isA, int idx, Player shooter, string team, int time, ActionType action, bool verbose)
    {
        int shots = action == ActionType.ThreePoint ? 3 : 2;
        int made = 0;
        double ftChance = GetFreeThrowChance(shooter);
        for (int i = 0; i < shots; i++)
        {
            bool success = _random.NextDouble() < ftChance;
            _statsTracker.AddFreeThrow(shooter, success);
            if (success) made++;
        }
        if (isA) _scoreA += made; else _scoreB += made;
        if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} fouled on shot. Free throws: {made}/{shots}");
    }

    private void ExecuteMadeShot(bool isA, int idx, Player shooter, string team, int time, ActionType action, bool verbose)
    {
        int pts = action == ActionType.ThreePoint ? 3 : 2;
        if (isA) _scoreA += pts; else _scoreB += pts;
        _statsTracker.AddPoints(shooter, pts);
        _statsTracker.AddFieldGoal(shooter, true, action == ActionType.ThreePoint);

        int aIdx;
        do { aIdx = _random.Next(0, 5); } while (aIdx == idx);
        _statsTracker.AddAssist((isA ? _teamA : _teamB)[aIdx]);
        if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} scores {pts}! Assist: Player {aIdx + 1}");
    }

    private void ExecuteMissedShot(bool isA, int idx, Player shooter, Player defender, string team, int time, ActionType action, bool verbose)
    {
        // Register missed field goal (assuming two-point miss)
        _statsTracker.AddFieldGoal(shooter, false, action == ActionType.ThreePoint);

        // 1) Attempt offensive rebound
        double posBonusOff = shooter.Position switch
        {
            Position.PowerForward or Position.Center => 0.10,
            Position.SmallForward => 0.05,
            _ => 0.00
        };
        double heightDiffOff = Math.Clamp((shooter.Height - defender.Height) / 200.0, -0.05, 0.05);
        double offRebRate = Math.Clamp(shooter.Rebounding / 150.0 + posBonusOff + heightDiffOff, 0.10, 0.50);

        if (_random.NextDouble() < offRebRate)
        {
            // Offensive rebound by shooter
            _statsTracker.AddRebound(shooter, true);
            if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} misses but gets the offensive rebound!");
            return;
        }

        // 2) Defensive rebound: choose rebounder from defense (frontcourt bias)
        var defense = isA ? _teamB : _teamA;
        var frontcourt = defense
            .Where(p => p.Position == Position.PowerForward || p.Position == Position.Center)
            .ToList();
        double frontcourtBias = 0.75;
        Player rebounder;
        if (_random.NextDouble() < frontcourtBias && frontcourt.Any())
        {
            // Most often a PF/C grabs the board
            rebounder = frontcourt[_random.Next(frontcourt.Count)];
        }
        else
        {
            // Otherwise the nearest defender collects
            rebounder = defender;
        }

        _statsTracker.AddRebound(rebounder, false);
        if (verbose)
        {
            if (rebounder == defender)
            {
                if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} misses. Defensive rebound by defender!");
            }
            else
            {
                int rebIdx = defense.IndexOf(rebounder) + 1;
                if (verbose) Console.WriteLine($"[{FormatTime(time)}] {team} - Player {idx + 1} misses. Defensive rebound by Player {rebIdx}!");
            }
        }
    }

    private int GetWeightedRandomIndex(List<Player> team)
    {
        // Weight by shot profile
        double[] weights = team.Select(p => p.Inside * 0.4 + p.MidRange * 0.3 + p.ThreePointers * 0.3).ToArray();
        double total = weights.Sum();
        double roll = _random.NextDouble() * total;
        double cum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cum += weights[i];
            if (roll <= cum) return i;
        }
        return _random.Next(team.Count);
    }

    public void DisplayResult()
    {
        Console.WriteLine("\nEnd of game!");
        Console.WriteLine($"Final Score: Team A {_scoreA} - Team B {_scoreB}");
        _statsTracker.DisplayAll("Team A", _teamA, "Team B", _teamB);
    }
}
