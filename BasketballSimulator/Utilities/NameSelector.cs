using System.Reflection;
using System.Text.Json;

namespace BasketballSimulator.Core.Utilities;

/// <summary>
/// Generates random first and last names based on country-specific distributions.
/// Requires a JSON file at Data/names.json structured as:
/// { "countries": { "CountryName": { "first": { ... }, "last": { ... } }, ... } }
/// </summary>
public static class NameSelector
{
    private class NameLists
    {
        public Dictionary<string, int> first { get; set; }
        public Dictionary<string, int> last { get; set; }
    }

    private class Root
    {
        public Dictionary<string, NameLists> countries { get; set; }
    }

    private static readonly Dictionary<string, List<(string Name, int Cumulative)>> _firstCumulative;
    private static readonly Dictionary<string, List<(string Name, int Cumulative)>> _lastCumulative;
    private static readonly Random _fallbackRng = new Random();

    static NameSelector()
    {
        var asm = Assembly.GetExecutingAssembly();
        var resourceName = asm.GetManifestResourceNames()
                              .FirstOrDefault(n => n.EndsWith("names.json", StringComparison.OrdinalIgnoreCase));

        if (resourceName == null)
        {
            throw new FileNotFoundException(
                $"Embedded names.json not found as resource. Available: {string.Join(", ", asm.GetManifestResourceNames())}"
            );
        }

        using Stream? stream = asm.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Failed to load embedded resource stream: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        string json = reader.ReadToEnd();

        var root = JsonSerializer.Deserialize<Root>(json)
                   ?? throw new InvalidDataException("Could not deserialize names.json");

        _firstCumulative = new Dictionary<string, List<(string, int)>>(root.countries.Count);
        _lastCumulative = new Dictionary<string, List<(string, int)>>(root.countries.Count);

        // Build cumulative distributions
        foreach (var kv in root.countries)
        {
            var country = kv.Key;
            var lists = kv.Value;

            _firstCumulative[country] = BuildCumulative(lists.first);
            _lastCumulative[country] = BuildCumulative(lists.last);
        }
    }

    private static List<(string Name, int Cumulative)> BuildCumulative(Dictionary<string, int> weights)
    {
        var list = new List<(string, int)>(weights.Count);
        int cum = 0;
        foreach (var kv in weights)
        {
            cum += kv.Value;
            list.Add((kv.Key, cum));
        }
        return list;
    }

    /// <summary>
    /// Generates a random first name for the specified country.
    /// If country is not found, a random available country is selected.
    /// </summary>
    public static string GenerateFirst(string country, Random rng)
    {
        if (rng == null) throw new ArgumentNullException(nameof(rng));
        if (!_firstCumulative.TryGetValue(country, out var cumList))
        {
            country = _firstCumulative.Keys.OrderBy(_ => _fallbackRng.Next()).First();
            cumList = _firstCumulative[country];
        }
        int total = cumList.Last().Cumulative;
        int pick = rng.Next(1, total + 1);
        foreach (var (name, cum) in cumList)
        {
            if (pick <= cum) return name;
        }
        return cumList.Last().Name;
    }

    /// <summary>
    /// Generates a random last name for the specified country.
    /// If country is not found, a random available country is selected.
    /// </summary>
    public static string GenerateLast(string country, Random rng)
    {
        if (rng == null) throw new ArgumentNullException(nameof(rng));
        if (!_lastCumulative.TryGetValue(country, out var cumList))
        {
            country = _lastCumulative.Keys.OrderBy(_ => _fallbackRng.Next()).First();
            cumList = _lastCumulative[country];
        }
        int total = cumList.Last().Cumulative;
        int pick = rng.Next(1, total + 1);
        foreach (var (name, cum) in cumList)
        {
            if (pick <= cum) return name;
        }
        return cumList.Last().Name;
    }

    /// <summary>
    /// Generates a full name (first + last) for the specified country.
    /// </summary>
    public static string GenerateFullName(string country)
    {
        var rng = new Random();
        var first = GenerateFirst(country, rng);
        var last = GenerateLast(country, rng);
        return $"{first} {last}";
    }
}
