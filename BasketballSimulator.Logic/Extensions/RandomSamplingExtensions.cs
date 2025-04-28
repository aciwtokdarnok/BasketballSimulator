namespace BasketballSimulator.Logic.Extensions;

/// <summary>
/// Adds sampling-related extensions to <see cref="Random"/>,
/// e.g. Gaussian draws and inclusive integer ranges.
/// </summary>
public static class RandomSamplingExtensions
{
    /// <summary>
    /// Returns a normally (Gaussian) distributed random value
    /// with the specified <paramref name="mean"/> and <paramref name="stdDev"/>,
    /// implemented via the Box–Muller transform.
    /// </summary>
    /// <param name="rng">The <see cref="Random"/> instance.</param>
    /// <param name="mean">The desired mean of the distribution.</param>
    /// <param name="stdDev">The desired standard deviation.</param>
    /// <returns>
    /// A random <see cref="double"/> drawn from N(mean, stdDev²).
    /// </returns>
    public static double NextGaussian(this Random rng, double mean = 0, double stdDev = 1)
    {
        double u1, u2;
        do
        {
            u1 = rng.NextDouble();
            u2 = rng.NextDouble();
        }
        while (u1 <= double.Epsilon);

        // Box–Muller transform
        double radius = Math.Sqrt(-2.0 * Math.Log(u1));
        double theta = 2.0 * Math.PI * u2;
        double z0 = radius * Math.Cos(theta);

        return mean + z0 * stdDev;
    }

    /// <summary>
    /// Returns a random integer in the inclusive range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>].
    /// </summary>
    /// <param name="rng">The <see cref="Random"/> instance.</param>
    /// <param name="minInclusive">Minimum value (inclusive).</param>
    /// <param name="maxInclusive">Maximum value (inclusive).</param>
    public static int NextInt(this Random rng, int minInclusive, int maxInclusive)
        => rng.Next(minInclusive, maxInclusive + 1);
}