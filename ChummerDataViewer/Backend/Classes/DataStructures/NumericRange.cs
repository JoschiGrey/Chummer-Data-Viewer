namespace ChummerDataViewer.Backend.Classes.DataStructures;

/// <summary>The Range class.</summary>
public class NumericRange
{
    public void Deconstruct(out int minimum, out int maximum)
    {
        minimum = Minimum;
        maximum = Maximum;
    }

    /// <summary>Minimum value of the range.</summary>
    public int Minimum { get; set; }

    /// <summary>Maximum value of the range.</summary>
    public int Maximum { get; set; }

    /// <summary>Presents the Range in readable format.</summary>
    /// <returns>String representation of the Range</returns>
    public override string ToString()
    {
        return $"[{Minimum} - {Maximum}]";
    }

    /// <summary>Determines if the range is valid. A Range is considered valid if the min is smaller then the max.</summary>
    /// <returns>True if range is valid, else false</returns>
    public bool IsValid()
    {
        return Minimum.CompareTo(Maximum) < 0;
    }

    /// <summary>Determines if the provided value is inside the range.</summary>
    /// <param name="value">The value to test</param>
    /// <returns>True if the value is inside Range, else false</returns>
    public bool ContainsValue(int? value)
    {
        return (Minimum.CompareTo(value) <= 0) && (Maximum.CompareTo(value) >= 0);
    }

    /// <summary>Determines if this Range is inside the bounds of another range.</summary>
    /// <param name="numericRange">The parent range to test on</param>
    /// <returns>True if range is inclusive, else false</returns>
    public bool IsInsideRange(NumericRange numericRange)
    {
        return IsValid() && numericRange.IsValid() && numericRange.ContainsValue(this.Minimum) && numericRange.ContainsValue(this.Maximum);
    }

    /// <summary>Determines if another range is inside the bounds of this range.</summary>
    /// <param name="numericRange">The child range to test</param>
    /// <returns>True if range is inside, else false</returns>
    public bool ContainsRange(NumericRange numericRange)
    {
        return IsValid() && numericRange.IsValid() && ContainsValue(numericRange.Minimum) && ContainsValue(numericRange.Maximum);
    }
}