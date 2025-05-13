using System.ComponentModel;

namespace BasketballSimulator.Core.Enums.Players;

/// <summary>
/// All basketball positions, including hybrids, with their standard abbreviations.
/// </summary>
public enum Position
{
    None = 0,

    [Description("PG")]
    PointGuard = 1,

    [Description("G")] // (PG/SG hybrid)
    Guard = 2,

    [Description("SG")]
    ShootingGuard = 3,

    [Description("GF")] // (SG/SF hybrid)
    GuardForward = 4,

    [Description("SF")]
    SmallForward = 5,

    [Description("F")] // (SF/PF hybrid)
    Forward = 5,

    [Description("PF")]
    PowerForward = 7,

    [Description("FC")] // (PF/C hybrid)
    ForwardCenter = 8,

    [Description("C")]
    Center = 9,
}
