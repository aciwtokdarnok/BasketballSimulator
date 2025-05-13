namespace BasketballSimulator.Logic.Extensions.Players;

public static class HeightExtensions
{
    // Static array of (probability cutoff, height in inches) pairs.
    // Must be sorted in ascending order by cutoff.
    // Used to sample a realistic basketball player height based on probability distribution.
    private static readonly (double Cutoff, int Height)[] _distribution = new[]
    {
        // (Probability cutoff, Height in inches)
        (0.000000000051653, 54),
        (0.000000000258264, 55),
        (0.000000001084711, 56),
        (0.000000004390496, 57),
        (0.000000017561983, 58),
        (0.000000069214876, 59),
        (0.000000275826446, 60),
        (0.000001308884298, 61),
        (0.00001163946281,  62),
        (0.000063292355372, 63),
        (0.000218251033058, 64),
        (0.000476515495868, 65),
        (0.000838085743802, 66),
        (0.00130296177686,  67),
        (0.002066115702479, 68),
        (0.004132231404959, 69),
        (0.008780991735537, 70),
        (0.012913223140496, 71),
        (0.041838842975207, 72),
        (0.083161157024793, 73),
        (0.12654958677686,  74),
        (0.196797520661157, 75),
        (0.267045454545455, 76),
        (0.337809917355372, 77),
        (0.419421487603306, 78),
        (0.521694214876033, 79),
        (0.62396694214876,  80),
        (0.739669421487603, 81),
        (0.832128099173554, 82),
        (0.915805785123967, 83),
        (0.967458677685951, 84),
        (0.984504132231405, 85),
        (0.991735537190083, 86),
        (0.995351239669422, 87),
        (0.997417355371901, 88),
        (0.998243801652893, 89),
        (0.999018595041323, 90),
        (0.99974173553719,  91),
        (0.999870867097108, 92),
        (0.999917354700413, 93),
        (0.999950929080579, 94),
        (0.99997675552686,  95),
        (0.999988377427686, 96),
        (0.99999612536157,  97),
        (0.999999456973141, 98),
        (0.999999818543389, 99),
        (0.999999934762397,100),
        (0.999999979958678,101),
        (0.999999991580579,102),
        (0.999999997572314,103),
        (0.999999999225207,104),
        (0.99999999963843, 105),
        (0.999999999845042,106),
        (0.999999999948347,107),
        // If no cutoff is exceeded, return 108
    };

    /// <summary>
    /// Generates a random basketball player height in inches based on the defined probability distribution.
    /// </summary>
    /// <returns>Height in inches as a double.</returns>
    public static double NextHeightInches()
    {
        Random random = new Random();
        double r = random.NextDouble(); // Random value between 0.0 and 1.0
        foreach (var (cutoff, height) in _distribution)
        {
            if (r < cutoff)
            {
                return height;
            }
        }
        // If r >= last cutoff, return the tallest possible height (108 inches)
        return 108;
    }

    /// <summary>
    /// Converts a height in inches to a 0–100 rating, scaled for basketball-specific min/max heights.
    /// </summary>
    /// <param name="heightInches">Height in inches.</param>
    /// <returns>Rating as a byte (0–100).</returns>
    public static byte HeightToRating(double heightInches)
    {
        // Minimum/maximum basketball-specific height in inches
        const int minHgt = 66; // 5'6"
        const int maxHgt = 93; // 7'9"

        // Scale to 0–100
        double raw = 100.0 * (heightInches - minHgt) / (maxHgt - minHgt);

        // Clamp to range and round
        byte rating = (byte)Math.Round(raw);
        return Math.Clamp(rating, (byte)0, (byte)100);
    }
}
