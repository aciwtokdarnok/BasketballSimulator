public static class RatingGroups
{
    public static readonly IReadOnlySet<string> Athleticism = new HashSet<string>
    {
        nameof(RatingPresets.Strength),
        nameof(RatingPresets.Speed),
        nameof(RatingPresets.Jumping),
        nameof(RatingPresets.Endurance),
        nameof(RatingPresets.DunksLayups),
    };

    public static readonly IReadOnlySet<string> Shooting = new HashSet<string>
    {
        nameof(RatingPresets.FreeThrows),
        nameof(RatingPresets.MidRange),
        nameof(RatingPresets.ThreePointers),
    };

    public static readonly IReadOnlySet<string> Skill = new HashSet<string>
    {
        nameof(RatingPresets.OffensiveIQ),
        nameof(RatingPresets.DefensiveIQ),
        nameof(RatingPresets.Dribbling),
        nameof(RatingPresets.Passing),
        nameof(RatingPresets.Rebounding),
    };
}