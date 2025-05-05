using System;
using System.Collections.Generic;
using System.Linq;
using BasketballSimulator.Core.Data.Team;
using BasketballSimulator.Logic.Generators.Team;

namespace BasketballSimulator.Logic.Generators.League;

/// <summary>
/// Builds a full NBA league: 30 teams each with a randomly generated roster.
/// </summary>
public class LeagueGenerator
{
    private readonly TeamRosterGenerator _rosterGen;

    public LeagueGenerator(Random rng)
    {
        _rosterGen = new TeamRosterGenerator(rng);
    }

    /// <summary>
    /// Returns all 30 teams, each populated with 12–15 draft-age players.
    /// </summary>
    public List<Core.Models.Team.Team> GenerateLeague()
    {
        var teams = TeamRepository
            .GetAllTeams()
            .Select(t => t)  // clones if Team were a record; here we use same instances
            .ToList();

        foreach (var team in teams)
        {
            team.Roster = _rosterGen.GenerateRoster(team.Name).ToList();
        }

        return teams;
    }
}

