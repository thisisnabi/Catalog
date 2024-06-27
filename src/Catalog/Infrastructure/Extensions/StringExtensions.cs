namespace Catalog.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string input)
    {
        string sanitizedInput = Regex.Replace(input, @"[^a-zA-Z0-9\s-]", "");

        string kebab = Regex.Replace(sanitizedInput.Trim(), @"\s+", "-");

        return kebab.ToLower();
    }
}
