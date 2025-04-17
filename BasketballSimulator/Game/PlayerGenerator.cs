using System;
using System.Collections.Generic;

/// <summary>
/// Provides methods to generate basketball players with statistics sampled from Gaussian distributions,
/// applying positional and hybrid biases. PlayerRole only determines overall rating mean.
/// </summary>
public static class PlayerGenerator
{
    /// <summary>
    /// Generates a normally distributed random value using the Box-Muller transform.
    /// </summary>
    private static double NextGaussian(Random rand, double mean = 0, double stddev = 1)
    {
        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
        return mean + stddev * z0;
    }

    /// <summary>
    /// Generates a Player with statistics based on position and hybrid multipliers around a base overall.
    /// </summary>
    public static Player GeneratePlayerWithGaussian(Random rand, Position fixedPosition = Position.None)
    {
        // 1) Determine primary position
        Position pos = fixedPosition != Position.None
            ? fixedPosition
            : (Position)rand.Next(1, 6);

        // 2) Select hybrid for versatility
        HybridPosition[] allowedHybrids = pos switch
        {
            Position.PointGuard => new[] { HybridPosition.None, HybridPosition.ComboGuard },
            Position.ShootingGuard => new[] { HybridPosition.None, HybridPosition.ComboGuard, HybridPosition.Wing },
            Position.SmallForward => new[] { HybridPosition.None, HybridPosition.Wing, HybridPosition.ForwardSwing },
            Position.PowerForward => new[] { HybridPosition.None, HybridPosition.ForwardSwing, HybridPosition.BigManSwing },
            Position.Center => new[] { HybridPosition.None, HybridPosition.BigManSwing },
            _ => new[] { HybridPosition.None }
        };
        HybridPosition hyb = allowedHybrids[rand.Next(allowedHybrids.Length)];

        // 3) Role roll determines base overall only
        double roll = rand.NextDouble();
        double baseMean = roll switch
        {
            < 0.03 => 90,
            < 0.12 => 80,
            < 0.30 => 70,
            < 0.60 => 65,
            < 0.85 => 60,
            < 0.97 => 55,
            _ => 50
        };
        int overall = (int)Math.Round(NextGaussian(rand, baseMean, 5));
        overall = Math.Clamp(overall, 30, 99);

        // 4) Height by position
        var (minH, maxH) = pos switch
        {
            Position.PointGuard => (175, 190),
            Position.ShootingGuard => (185, 200),
            Position.SmallForward => (195, 210),
            Position.PowerForward => (200, 215),
            Position.Center => (205, 225),
            _ => (170, 221)
        };
        int height = rand.Next(minH, maxH + 1);

        // 5) Determine multiplier pair from hybrid or position
        (Position a, Position b) pair = hyb switch
        {
            HybridPosition.ComboGuard => (Position.PointGuard, Position.ShootingGuard),
            HybridPosition.Wing => (Position.ShootingGuard, Position.SmallForward),
            HybridPosition.ForwardSwing => (Position.SmallForward, Position.PowerForward),
            HybridPosition.BigManSwing => (Position.PowerForward, Position.Center),
            _ => (pos, pos)
        };

        // 6) Fetch skill multipliers and average
        var mA = GetSkillMultipliers(pair.a);
        var mB = GetSkillMultipliers(pair.b);
        var m = new SkillMultipliers
        {
            Strength = (mA.Strength + mB.Strength) * 0.5,
            Speed = (mA.Speed + mB.Speed) * 0.5,
            Jumping = (mA.Jumping + mB.Jumping) * 0.5,
            Endurance = (mA.Endurance + mB.Endurance) * 0.5,
            Inside = (mA.Inside + mB.Inside) * 0.5,
            DunksLayups = (mA.DunksLayups + mB.DunksLayups) * 0.5,
            FreeThrows = (mA.FreeThrows + mB.FreeThrows) * 0.5,
            MidRange = (mA.MidRange + mB.MidRange) * 0.5,
            ThreePointers = (mA.ThreePointers + mB.ThreePointers) * 0.5,
            OffensiveIQ = (mA.OffensiveIQ + mB.OffensiveIQ) * 0.5,
            DefensiveIQ = (mA.DefensiveIQ + mB.DefensiveIQ) * 0.5,
            Dribbling = (mA.Dribbling + mB.Dribbling) * 0.5,
            Passing = (mA.Passing + mB.Passing) * 0.5,
            Rebounding = (mA.Rebounding + mB.Rebounding) * 0.5
        };

        // 7) Generate stats around weighted overall
        Player p = new Player
        {
            Position = pos,
            HybridPosition = hyb,
            PlayerRole = PlayerRole.None, // role no longer affects stats
            Height = height,

            Strength = Clamp(rand, overall * m.Strength, 8),
            Speed = Clamp(rand, overall * m.Speed, 8),
            Jumping = Clamp(rand, overall * m.Jumping, 8),
            Endurance = Clamp(rand, overall * m.Endurance, 8),

            Inside = Clamp(rand, overall * m.Inside, 8),
            DunksLayups = Clamp(rand, overall * m.DunksLayups, 8),
            FreeThrows = Clamp(rand, overall * m.FreeThrows, 8),
            MidRange = Clamp(rand, overall * m.MidRange, 8),
            ThreePointers = Clamp(rand, overall * m.ThreePointers, 8),

            OffensiveIQ = Clamp(rand, overall * m.OffensiveIQ, 8),
            DefensiveIQ = Clamp(rand, overall * m.DefensiveIQ, 8),

            Dribbling = Clamp(rand, overall * m.Dribbling, 8),
            Passing = Clamp(rand, overall * m.Passing, 8),
            Rebounding = Clamp(rand, overall * m.Rebounding, 8)
        };

        // 8) Final calculations
        p.CalculateOverall();
        p.CalculatePotential();
        return p;
    }

    private struct SkillMultipliers
    {
        public double Strength, Speed, Jumping, Endurance;
        public double Inside, DunksLayups, FreeThrows, MidRange, ThreePointers;
        public double OffensiveIQ, DefensiveIQ, Dribbling, Passing, Rebounding;
    }

    private static SkillMultipliers GetSkillMultipliers(Position pos) => pos switch
    {
        Position.PointGuard => new SkillMultipliers
        {
            Strength = 0.7,
            Speed = 1.2,
            Jumping = 1.0,
            Endurance = 1.0,
            Inside = 0.7,
            DunksLayups = 0.9,
            FreeThrows = 1.0,
            MidRange = 1.1,
            ThreePointers = 1.3,
            OffensiveIQ = 1.2,
            DefensiveIQ = 1.0,
            Dribbling = 1.3,
            Passing = 1.3,
            Rebounding = 0.5
        },
        Position.ShootingGuard => new SkillMultipliers
        {
            Strength = 0.8,
            Speed = 1.1,
            Jumping = 1.0,
            Endurance = 0.9,
            Inside = 0.8,
            DunksLayups = 1.0,
            FreeThrows = 1.0,
            MidRange = 1.1,
            ThreePointers = 1.2,
            OffensiveIQ = 1.1,
            DefensiveIQ = 0.9,
            Dribbling = 1.2,
            Passing = 1.1,
            Rebounding = 0.6
        },
        Position.SmallForward => new SkillMultipliers
        {
            Strength = 1.0,
            Speed = 1.0,
            Jumping = 1.0,
            Endurance = 1.0,
            Inside = 1.0,
            DunksLayups = 1.1,
            FreeThrows = 0.9,
            MidRange = 1.0,
            ThreePointers = 1.0,
            OffensiveIQ = 1.0,
            DefensiveIQ = 1.0,
            Dribbling = 1.0,
            Passing = 1.0,
            Rebounding = 1.0
        },
        Position.PowerForward => new SkillMultipliers
        {
            Strength = 1.2,
            Speed = 0.9,
            Jumping = 1.0,
            Endurance = 1.1,
            Inside = 1.1,
            DunksLayups = 1.2,
            FreeThrows = 0.8,
            MidRange = 0.8,
            ThreePointers = 0.6,
            OffensiveIQ = 0.9,
            DefensiveIQ = 1.1,
            Dribbling = 0.8,
            Passing = 0.8,
            Rebounding = 1.2
        },
        Position.Center => new SkillMultipliers
        {
            Strength = 1.3,
            Speed = 0.8,
            Jumping = 1.1,
            Endurance = 1.1,
            Inside = 1.3,
            DunksLayups = 1.2,
            FreeThrows = 0.7,
            MidRange = 0.6,
            ThreePointers = 0.4,
            OffensiveIQ = 0.8,
            DefensiveIQ = 1.2,
            Dribbling = 0.7,
            Passing = 0.7,
            Rebounding = 1.3
        },
        _ => new SkillMultipliers
        {
            Strength = 1.0,
            Speed = 1.0,
            Jumping = 1.0,
            Endurance = 1.0,
            Inside = 1.0,
            DunksLayups = 1.0,
            FreeThrows = 1.0,
            MidRange = 1.0,
            ThreePointers = 1.0,
            OffensiveIQ = 1.0,
            DefensiveIQ = 1.0,
            Dribbling = 1.0,
            Passing = 1.0,
            Rebounding = 1.0
        }
    };

    private static int Clamp(Random rand, double mean, double stddev)
    {
        int val = (int)Math.Round(NextGaussian(rand, mean, stddev));
        return Math.Clamp(val, 1, 99);
    }
}
