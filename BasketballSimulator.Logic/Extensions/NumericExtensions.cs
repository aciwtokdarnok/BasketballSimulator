namespace BasketballSimulator.Logic.Extensions;

/// <summary>
/// General-purpose numeric helper extensions.
/// </summary>
public static class NumericExtensions
{
    /// <summary>
    /// Clamps <paramref name="x"/> to the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    public static double Clamp(this double x, double min, double max)
        => x < min ? min : x > max ? max : x;
}