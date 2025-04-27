using System.ComponentModel;

/// <summary>
/// All basketball positions, including hybrids, with their standard abbreviations.
/// </summary>
public enum Position
{
    None = 0,

    [Description("PG")]
    PointGuard = 1,

    [Description("SG")]
    ShootingGuard = 2,

    [Description("G")]
    Guard = 3,

    [Description("GF")]
    GuardForward = 4,

    [Description("SF")]
    SmallForward = 5,

    [Description("PF")]
    PowerForward = 6,

    [Description("FC")]
    ForwardCenter = 7,

    [Description("C")]
    Center = 8,
}
}