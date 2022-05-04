using System.Text.RegularExpressions;
using Blazor.Extensions.Logging;
using Blazorise.Extensions;
using Chummer_Database.Enums;
using Microsoft.AspNetCore.Components;

namespace Chummer_Database.Classes;

public class Damage
{
    public string FullDamageString { get; }
    public bool IsPhysical { get; }
    public Element Element { get; } = Element.None;
    public string DamageAmount { get; }
    public int DamageDropPerMeter { get; }
    public int DamageRadius { get; }
    

    public Damage(string fullDamageString, ILogger logger)
    {
        FullDamageString = fullDamageString;
        string match;
        
        //Is Stun?
        var pattern = "[0-9]S";
        if (Regex.IsMatch(fullDamageString, pattern))
        {
            IsPhysical = false;
        }
        
        //Damage Dropoff?

        try
        {
            pattern = @"(?<=\(-)[0-9]*(?<!\/m)";
            match = Regex.Match(fullDamageString, pattern).ToString();
            if (!match.IsNullOrEmpty())
                DamageDropPerMeter = int.Parse(match);
        }
        catch (FormatException e)
        {
            logger.LogDebug(e, "Parsing of Damage Dropoff for \"{FullDamageString}\" failed", FullDamageString);
        }
        
        //Damage Type
        try
        {
            pattern = @"(?<=\()([^0-9]*)(?=\))";
            match = Regex.Match(fullDamageString, pattern).ToString();
            if (!match.IsNullOrEmpty())
                Element = Enum.Parse<Element>(match, true);
        }
        catch (ArgumentException e)
        {
            logger.LogDebug(e,"Element Type Parsing of \"{FullDamageString}\" failed", fullDamageString);
        }
        
        //Damage Amount
        if (FullDamageString.Contains('{'))
        {
            pattern = @"{.*\+[0-9]*";
            DamageAmount = Regex.Match(FullDamageString, pattern).ToString();
        }
        else
        {
            pattern = @"[0-9]*";
            DamageAmount = Regex.Match(FullDamageString, pattern).ToString();
        }

        try
        {
            pattern = @"(?<=\()([0-9]*)(?=(m Radius))";
            match = Regex.Match(FullDamageString, pattern).ToString();
            if(!match.IsNullOrEmpty())
                DamageRadius = int.Parse(match);
        }
        catch(FormatException e)
        {
            logger.LogDebug(e, "Parsing of Damage Radius for \"{FullDamageString}\" failed", FullDamageString);
        }


    }
}