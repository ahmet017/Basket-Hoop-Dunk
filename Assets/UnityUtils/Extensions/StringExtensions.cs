using System;
using System.Text.RegularExpressions;

public  static class StringExtensions
{
    public static bool IsConsistOfOnlyEnglishCharacters(this string input)
    {
        if (string.IsNullOrEmpty(input)) return true;

        // Regular expression pattern to match English characters
        string pattern = @"^[a-zA-Z]+$";

        // Create a regular expression object
        Regex regex = new Regex(pattern);

        // Match the input string against the pattern
        Match match = regex.Match(input);

        // Return true if the input string contains only English characters, false otherwise
        return match.Success;
    }
}
