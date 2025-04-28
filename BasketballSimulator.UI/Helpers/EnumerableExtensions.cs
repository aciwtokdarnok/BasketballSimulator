namespace BasketballSimulator.UI.Helpers;

public static class EnumerableExtensions
{
    public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool ascending)
    {
        var prop = typeof(T).GetProperty(propertyName);
        if (ascending)
            return source.OrderBy(x => prop.GetValue(x));
        else
            return source.OrderByDescending(x => prop.GetValue(x));
    }
}
