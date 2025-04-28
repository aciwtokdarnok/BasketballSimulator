namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Picks a random basketball country according to the historic NBA distribution.
/// </summary>
public static class CountrySelector
{
    // Raw weights from your list
    private static readonly Dictionary<string, int> _weights = new()
    {
        ["Albania"] = 2,
        ["Algeria"] = 1,
        ["Angola"] = 8,
        ["Argentina"] = 71,
        ["Australia"] = 174,
        ["Austria"] = 15,
        ["Azerbaijan"] = 1,
        ["Bahamas"] = 48,
        ["Belarus"] = 12,
        ["Belgium"] = 44,
        ["Benin"] = 5,
        ["Bosnia and Herzegovina"] = 140,
        ["Brazil"] = 82,
        ["Bulgaria"] = 29,
        ["Cameroon"] = 69,
        ["Canada"] = 392,
        ["Cape Verde"] = 5,
        ["Central African Republic"] = 9,
        ["Chad"] = 5,
        ["China"] = 34,
        ["Colombia"] = 15,
        ["Congo"] = 34,
        ["Croatia"] = 216,
        ["Czech Republic"] = 34,
        ["Denmark"] = 21,
        ["Dominican Republic"] = 33,
        ["Egypt"] = 15,
        ["England"] = 112,
        ["Estonia"] = 14,
        ["Finland"] = 34,
        ["France"] = 294,
        ["French Guiana"] = 7,
        ["Gabon"] = 5,
        ["Georgia"] = 20,
        ["Germany"] = 181,
        ["Ghana"] = 10,
        ["Greece"] = 214,
        ["Guadeloupe"] = 13,
        ["Guinea"] = 11,
        ["Haiti"] = 12,
        ["Hungary"] = 22,
        ["Iceland"] = 10,
        ["India"] = 1,
        ["Iran"] = 6,
        ["Ireland"] = 7,
        ["Israel"] = 50,
        ["Italy"] = 228,
        ["Ivory Coast"] = 26,
        ["Jamaica"] = 28,
        ["Japan"] = 7,
        ["Kazakhstan"] = 7,
        ["Kenya"] = 5,
        ["Kosovo"] = 13,
        ["Latvia"] = 69,
        ["Liberia"] = 4,
        ["Lithuania"] = 195,
        ["Luxembourg"] = 3,
        ["Mali"] = 20,
        ["Mexico"] = 17,
        ["Moldova"] = 4,
        ["Montenegro"] = 85,
        ["Morocco"] = 5,
        ["Netherlands"] = 49,
        ["New Zealand"] = 24,
        ["Nigeria"] = 136,
        ["North Macedonia"] = 28,
        ["Norway"] = 6,
        ["Panama"] = 17,
        ["Philippines"] = 3,
        ["Poland"] = 69,
        ["Portugal"] = 12,
        ["Puerto Rico"] = 58,
        ["Romania"] = 22,
        ["Russia"] = 116,
        ["Scotland"] = 6,
        ["Senegal"] = 113,
        ["Serbia"] = 341,
        ["Slovakia"] = 22,
        ["Slovenia"] = 105,
        ["South Africa"] = 9,
        ["South Korea"] = 5,
        ["South Sudan"] = 7,
        ["Spain"] = 251,
        ["Sudan"] = 24,
        ["Sweden"] = 47,
        ["Switzerland"] = 15,
        ["Taiwan"] = 3,
        ["Trinidad and Tobago"] = 11,
        ["Turkey"] = 110,
        ["USA"] = 23461,
        ["Ukraine"] = 48,
        ["Uruguay"] = 7,
        ["Uzbekistan"] = 3,
        ["Venezuela"] = 13,
        ["Virgin Islands"] = 10,
    };

    // Precompute cumulative distribution for fast lookup
    private static readonly List<(string Country, int Cumulative)> _cumulative;
    private static readonly int _totalWeight;

    static CountrySelector()
    {
        _cumulative = new List<(string, int)>(_weights.Count);
        int cum = 0;
        foreach (var kv in _weights)
        {
            cum += kv.Value;
            _cumulative.Add((kv.Key, cum));
        }
        _totalWeight = cum;
    }

    /// <summary>
    /// Returns a random country according to the basketball distribution.
    /// </summary>
    /// <param name="rng">An instance of <see cref="Random"/>.</param>
    /// <returns>The selected country name.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Select()
    {
        var rng = new Random();

        // Pick a value [1 .. totalWeight]
        int pick = rng.Next(1, _totalWeight + 1);

        // Find first cumulative >= pick
        foreach (var (country, cum) in _cumulative)
        {
            if (pick <= cum)
            {
                return country;
            }
        }

        // Fallback (should never happen)
        return "USA";
    }
}
