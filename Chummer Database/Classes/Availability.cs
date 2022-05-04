using System.Text.RegularExpressions;
using Blazorise.Extensions;
using Chummer_Database.Enums;

namespace Chummer_Database.Classes;

public class Availability
{
    public string AvailabilityString { get;  }
    
    public int? AvailabilityInt { get; }
    
    public Legality Legality { get; } = Legality.Unrestricted;
    
    private ILogger Logger { get; }


    public Availability(string availabilityString, ILogger logger)
    {
        Logger = logger;
        AvailabilityString = availabilityString;

        string pattern;
        string match;
        
        try
        {
            pattern = @"(?<=[^0-9])[0-9]*(?=(F|R|[0-9]))";
            match = Regex.Match(AvailabilityString, pattern).ToString();
            if (!match.IsNullOrEmpty())
                AvailabilityInt = int.Parse(match);

        }
        catch (FormatException e)
        {
            logger.LogDebug(e,"Failed Parsing of AvailabilityInt for {AvailabilityString}", AvailabilityString);
        }

        if (AvailabilityString.EndsWith('R'))
            Legality = Legality.Restricted;
        if (AvailabilityString.EndsWith('F'))
            Legality = Legality.Forbidden;
    }
}