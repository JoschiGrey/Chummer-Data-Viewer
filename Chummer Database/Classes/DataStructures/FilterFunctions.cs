namespace Chummer_Database.Classes.DataStructures;

public static class FilterFunctions
{
    public static bool NumberFilter(this NumericRange? numRange, int? mainVariable)
    {
        if (mainVariable is null)
            throw new ArgumentNullException(nameof(mainVariable));
        if (numRange is null)
            throw new ArgumentNullException(nameof(numRange));

        var (min, max) = numRange;
        
        //Fallback to just min if range is invalid
        if (!numRange.IsValid())
            return (mainVariable >= min);

        return numRange.ContainsValue(mainVariable);
    }
    
    public static bool NumberFilter(int? mainVariable, int min, int max)
    {
        if (mainVariable is null)
            throw new ArgumentNullException(nameof(mainVariable));
        
        if (min == 0 && max == 0)
            return true;
        if (mainVariable > min && max < min)
            return true;
        return mainVariable > min && (mainVariable < max || mainVariable == 0 );
    }
    
}