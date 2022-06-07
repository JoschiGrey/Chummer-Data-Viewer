using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

public record ArmorPen : IDisplayable
{
    public string DisplayName => ToString();
    
    private string BaseString { get; set; }
    
    private int BaseApValue { get; set; }
    
    public int ApModifier { get; set; }

    public int ArmorPenValue => BaseApValue + ApModifier;
    
    private bool IsSpecial { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (IsSpecial)
            sb.Append(BaseString);

        if (BaseApValue != 0)
            sb.Append(BaseApValue);
        
        if (ApModifier != 0)
            sb.Append(' ').Append(BaseApValue);

        return sb.ToString();
    }

    public ArmorPen(string baseString)
    {
        BaseString = baseString;
        
        const string valuesWithOperatorPattern = @"(\+|\-)\d+";
        var matches = Regex.Matches(baseString, valuesWithOperatorPattern);

        if (matches.Count != 1)
        {
            IsSpecial = true;
            return;
        }

        if (int.TryParse(matches.First().ToString(), out var newInt))
        {
            BaseApValue = newInt;
            return;
        }

        if (baseString.Equals("-"))
            BaseApValue = 0;
    }
    
}