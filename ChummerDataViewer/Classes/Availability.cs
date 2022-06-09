using System.Text.RegularExpressions;
using Blazor.Extensions.Logging;
using Blazorise.Extensions;
using ChummerDataViewer.Classes.HelperMethods;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

//TODO: Cavalier Falchion GH3 avail way too high!
//TODO: Mods in base weapon don't apply avail
public record Availability
{
    public string AvailabilityString { get; private set; } = string.Empty;
    
    public int AvailabilityInt { get; private set; }
    
    public Legality Legality { get; private set; } = Legality.Unrestricted;
    
    public bool IsModifyingAvailability { get; }

    public Availability(string availabilityString, ILogger logger)
    {
        AvailabilityString = availabilityString;

        AvailabilityInt = RegexHelper.GetInt(AvailabilityString, logger);

        if (AvailabilityString.Contains('+'))
            IsModifyingAvailability = true;
        
        
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
        //Keep the avail if it isn't a modifying one (like smartgun)
        int newAvailInt;
        if (y.IsModifyingAvailability)
            newAvailInt = x.AvailabilityInt + y.AvailabilityInt;
        else
            newAvailInt = x.AvailabilityInt;
        
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