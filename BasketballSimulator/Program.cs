using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        const int simulations = 20;
        int winsA = 0, winsB = 0;
        int totalA = 0, totalB = 0;

        var game = new BasketballGame();
        game.SetupTeams();
        game.DisplayTeams();

        for (int i = 1; i <= simulations; i++)
        {
            game.ResetForSimulation();
            game.Simulate(false);
            game.DisplayResult();
            totalA += game.ScoreA; totalB += game.ScoreB;
            if (game.ScoreA > game.ScoreB) winsA++; else if (game.ScoreB > game.ScoreA) winsB++;
        }

        Console.WriteLine($"Simulated {simulations} games");
        Console.WriteLine($"Team A wins: {winsA}, Team B wins: {winsB}");
        Console.WriteLine($"Average score - Team A: {totalA / (double)simulations:F1}, Team B: {totalB / (double)simulations:F1}");
    }
}