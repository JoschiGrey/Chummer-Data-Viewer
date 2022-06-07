using System.Text.RegularExpressions;
using Blazor.Extensions.Logging;
using Blazorise.Extensions;
using ChummerDataViewer.Classes.HelperMethods;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

public record Availability
{
    public string AvailabilityString { get; private set; } = string.Empty;
    
    public int AvailabilityInt { get; private set; }
    
    public Legality Legality { get; private set; } = Legality.Unrestricted;

    public Availability(string availabilityString, ILogger logger)
    {
        AvailabilityString = availabilityString;

        AvailabilityInt = RegexHelper.GetInt(AvailabilityString, logger);
        
        try
        {
            const string pattern = "[0-9]+";
            var match = Regex.Match(AvailabilityString, pattern).ToString();
            if (!match.IsNullOrEmpty())
                AvailabilityInt = int.Parse(match);
        }
        catch (FormatException e)
        {
            logger.LogWarning(e,"Failed Parsing of AvailabilityInt for {AvailabilityString}", AvailabilityString);
        }
        if (AvailabilityString.EndsWith('R'))
            Legality = Legality.Restricted;
        if (AvailabilityString.EndsWith('F'))
            Legality = Legality.Forbidden;

        if (AvailabilityInt >= 99)
            AvailabilityString = "NA";
    }

    private Availability() { }

    public static Availability operator +(Availability x, Availability y)
    {
        var newAvailInt =  x.AvailabilityInt + y.AvailabilityInt;

        var newLegality = x.Legality;
       
        if (x.Legality < y.Legality)
            newLegality = y.Legality;

        string suffix = x.Legality switch
        {
            Legality.Restricted => "R",
            Legality.Forbidden => "F",
            _ => string.Empty
        };

        var newAvailabilityString = $"{x.AvailabilityInt}{suffix}";
        
        return new Availability()
        {
            AvailabilityInt = newAvailInt,
            Legality = newLegality,
            AvailabilityString = newAvailabilityString
        };
    }


}