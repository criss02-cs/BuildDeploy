namespace BuildDeploy.Business.Extensions;
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

    public static int ToInt32(this string? s) => int.TryParse(s, out var n) ? n : int.MinValue;

    public static string Join<T>(this IEnumerable<T> list, string delimiter) => string.Join(delimiter, list);
}
