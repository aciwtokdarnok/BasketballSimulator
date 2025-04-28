namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Generates a realistic basketball player weight (in kilograms)
/// based on height and strength ratings, with a bit of randomness.
/// </summary>
public static class WeightHelper
{
    private const int MinWeightLb = 155;
    private const double LbToKg = 0.45359237;

    /// <summary>
    /// Generate a weight (kg) for a basketball player.
    /// </summary>
    /// <param name="heightRating">
    ///   Height rating (0–100), as produced by your HeightToRating.
    /// </param>
    /// <param name="strengthRating">
    ///   Strength rating (0–100). If omitted, heightRating will be used.
    /// </param>
    /// <param name="rng">
    ///   A <see cref="Random"/> instance for jitter. Pass your injected RNG.
    /// </param>
    /// <returns>
    ///   Weight in kilograms, rounded to the nearest whole number.
    /// </returns>
    public static byte GenerateWeight(byte heightRating, byte strengthRating)
    {
        var rng = new Random();
        // add +/- up to 20 lb jitter
        int jitterLb = rng.Next(-20, 21);

        double weightLb = 0.5 * strengthRating + heightRating + MinWeightLb + jitterLb;

        // Convert to kilograms and round
        double weightKg = weightLb * LbToKg;
        return (byte)Math.Round(weightKg);
    }
}
