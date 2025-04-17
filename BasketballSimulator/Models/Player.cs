public class Player
{
    private const int MinHeight = 170;
    private const int MaxHeight = 220;

    public int Height, Strength, Speed, Jumping, Endurance;
    public int Inside, DunksLayups, FreeThrows, MidRange, ThreePointers;
    public int OffensiveIQ, DefensiveIQ, Dribbling, Passing, Rebounding;
    public double Overall, Potential;

    public Position Position;
    public HybridPosition HybridPosition;
    public PlayerRole PlayerRole;

    public void CalculateOverall()
    {
        // Normalize height to 1–99 based on expected range
        double heightRating = Math.Clamp((Height - MinHeight) * (99.0 / (MaxHeight - MinHeight)), 1, 99);

        // Sum of height plus the 15 skill attributes
        double sum = heightRating + Strength + Speed + Jumping + Endurance
                     + Inside + DunksLayups + FreeThrows + MidRange + ThreePointers
                     + OffensiveIQ + DefensiveIQ + Dribbling + Passing + Rebounding;

        // True average over 16 values (height + 15 skills)
        Overall = Math.Round(sum / 16.0);
    }

    public void CalculatePotential()
    {
        // Use same normalized height for physical potential
        double heightRating = Math.Clamp((Height - MinHeight) * (99.0 / (MaxHeight - MinHeight)), 1, 99);

        // Averages for each category
        double physAvg = (heightRating + Strength + Speed + Jumping + Endurance) / 5.0;
        double shootAvg = (Inside + DunksLayups + FreeThrows + MidRange + ThreePointers) / 5.0;
        double skillAvg = (OffensiveIQ + DefensiveIQ + Dribbling + Passing + Rebounding) / 5.0;

        // Weighted potential: 30% physical, 35% shooting, 35% skill
        double rawPotential = 0.30 * physAvg + 0.35 * shootAvg + 0.35 * skillAvg;

        // Ensure potential is at least current overall and capped at 99
        double pot = Math.Max(rawPotential, Overall);
        pot = Math.Min(pot, 99);

        Potential = Math.Round(pot);
    }
}