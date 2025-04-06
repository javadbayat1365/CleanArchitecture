using System.Text.RegularExpressions;

namespace Clean.WebFramework.Extensions;

internal static class RegExHelpers
{
    public static bool MarchesApiVersion(string apiVersion, string text)
    {
        string pattern = $@"(?<=\/|^){Regex.Escape(apiVersion)}(?=\/|$)";
        Regex exactMatchRegex = new Regex(pattern,RegexOptions.Compiled);
        return exactMatchRegex.IsMatch(text);
    }
}
