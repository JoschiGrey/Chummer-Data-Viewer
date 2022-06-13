using System.Text.RegularExpressions;
using Blazorise.Extensions;

namespace ChummerDataViewer.Backend.Classes.HelperMethods;



public static class RegexHelper
{
    private const string NumberPattern = "[0-9]+";
    
    public static int GetInt(string input, ILogger logger)
    {
        var output = 0;
        try
        {
            var match = Regex.Match(input, NumberPattern).ToString();
            if (!match.IsNullOrEmpty())
                output = int.Parse(match);
        }
        catch (FormatException e)
        {
            logger.LogWarning(e, "Failed to parse {Input} to an cost", input);
        }

        return output;
    }
}