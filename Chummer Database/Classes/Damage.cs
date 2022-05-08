using System.Text;
using System.Text.RegularExpressions;
using Blazor.Extensions.Logging;
using Blazorise.Extensions;
using Chummer_Database.Enums;
using Microsoft.AspNetCore.Components;
// ReSharper disable MemberCanBePrivate.Global


namespace Chummer_Database.Classes;

public class Damage
{
    private string FullDamageString { get; }
    public string DisplayString { get; }
    public DamageType DamageType { get; } = DamageType.Special;
    public Element Element { get; } = Element.None;
    public int DamageAmount { get; }
    public int DamageDropPerMeter { get; }
    public string? DamageDropPerMeterString { get; }
    public int DamageRadius { get; }
    public string? DamageRadiusString { get; }
    

    public Damage(string fullDamageString, ILogger logger)
    {
        FullDamageString = fullDamageString;
        string match;
        
        
        //Is Physical?
        var pattern = "[0-9]S";
        if (Regex.IsMatch(fullDamageString, pattern))
        {
            DamageType = DamageType.Stun;
        }
        pattern = "[0-9]P";
        if (Regex.IsMatch(fullDamageString, pattern))
        {
            DamageType = DamageType.Physical;
        }
        
        //Damage Dropoff
        try
        {
            pattern = @"(?<=\(-)[0-9]*(?<!\/m)";
            match = Regex.Match(fullDamageString, pattern).ToString();
            if (!match.IsNullOrEmpty())
            {
                DamageDropPerMeter = int.Parse(match);
                DamageDropPerMeterString = $"-{DamageDropPerMeter} / m";
            }

        }
        catch (FormatException e)
        {
            logger.LogDebug(e, "Parsing of Damage Dropoff for \"{FullDamageString}\" failed", FullDamageString);
        }
        
        //Damage Type
        try
        {
            // TODO: Needs to be reworked for spells probably
            // pattern = @"(?<=\()([^0-9]*)(?=\))";
            // match = Regex.Match(fullDamageString, pattern).ToString();
            if (FullDamageString.Contains("(e)"))
                Element = Element.Electro;
        }
        catch (ArgumentException e)
        {
            logger.LogDebug(e,"Element Type Parsing of \"{FullDamageString}\" failed", fullDamageString);
        }

        //Mainly takes out the Osmium Mace... still not gonna deal with that shit!
        if (!FullDamageString.Contains(">="))
        {
            pattern = @"\-?[0-9]+(?!.{0,2}m)";
            match = Regex.Match(FullDamageString, pattern).ToString();

            try
            {
                if (!match.IsNullOrEmpty())
                    DamageAmount = int.Parse(match);
            }
            catch (FormatException e)
            {
                logger.LogWarning(e, "Could not parse {Match} to int", match);
            }
        }
        
        //Damage Radius
        try
        {
            pattern = @"(?<=\()([0-9]*)(?=(m Radius))";
            match = Regex.Match(FullDamageString, pattern).ToString();
            if (!match.IsNullOrEmpty())
            {
                DamageRadius = int.Parse(match);
                DamageRadiusString = $"{DamageRadius}m Radius";
            }
                
        }
        catch(FormatException e)
        {
            logger.LogDebug(e, "Parsing of Damage Radius for \"{FullDamageString}\" failed", FullDamageString);
        }


        DisplayString = FormDamageDisplay();

    }
    
    private string FormDamageDisplay()
    {
        if (FullDamageString.IsNullOrEmpty())
            return string.Empty;
        
        //Osmium Mace, yeah not gonna deal with that shit rn!
        if (FullDamageString.Contains(")))"))
            return "special ";

        var sb = new StringBuilder();

        var outputString = FullDamageString;

        //Remove all { }
        const string pattern = "({|})";
        outputString = Regex.Replace(outputString, pattern, "");
        
        //Removes most (), leaves stuff like (e) or (Radius 10) alone
        //pattern = @"(^\((?!\d))|(\)(?=.))";
        //outputString = Regex.Replace(outputString, pattern, "");

        //Insert a space in front of P or S
        //pattern = "(P|S)";
        //outputString = Regex.Replace(outputString, pattern, @" $&");
        
        
        
        switch (FullDamageString)
        {
            case { } s when s.Contains("MAG*"):
                sb.Append("MAG *");
                break;
            case { } s when s.Contains("Rating+"):
                sb.Append("Rating +");
                break;
            case { } s when s.Contains("{STR}+"):
                sb.Append("STR +");
                break;
            case { } s when s.Contains("{STR}"):
                sb.Append("STR");
                break;
            case { } s when s.Contains("Special"):
                sb.Append("Special");
                break;
            case { } s when s.Contains("Grenade"):
                sb.Append("Grenade");
                break;
            case { } s when s.Contains("Missile"):
                sb.Append("missile ");
                break;
            case { } s when s.Contains("Chemical"):
                sb.Append("chemical ");
                break;
            case { } s when s.Contains("Toxin"):
                sb.Append("chemical ");
                break;
            case { } s when s.Contains("Pepper Punch"):
                sb.Append("chemical ");
                break;
        }

        switch (DamageType)
        {
            case DamageType.Physical:
                sb.Append("dmg-physical ");
                break;
            case DamageType.Stun:
                sb.Append("dmg-stun ");
                break;
        }

        switch (Element)
        {
            case Element.Electro:
                sb.Append("dmg-electro ");
                break;
        }

        return outputString;
    }


}