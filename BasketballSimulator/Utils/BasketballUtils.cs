public static class BasketballUtils
{
    public static string FormatTime(int totalSeconds) => $"{totalSeconds / 60:D2}:{totalSeconds % 60:D2}";

    public static string GetPosAbbrev(Position pos) => pos switch
    {
        Position.PointGuard => "PG",
        Position.ShootingGuard => "SG",
        Position.SmallForward => "SF",
        Position.PowerForward => "PF",
        Position.Center => "C",
        _ => pos.ToString().Substring(0, 2).ToUpper()
    };

    public static string GetHybAbbrev(HybridPosition hyb) => hyb switch
    {
        HybridPosition.None => "-",
        HybridPosition.ComboGuard => "CG",
        HybridPosition.Wing => "WG",
        HybridPosition.ForwardSwing => "FS",
        HybridPosition.BigManSwing => "BS",
        _ => "-"
    };

    public static string GetRoleAbbrev(PlayerRole role) => role switch
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
}
