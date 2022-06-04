namespace ChummerDataViewer.Classes.DataStructures;

public static class FilterFunctions
{
    /// <summary>
    /// Filters an int against a given range
    /// </summary>
    /// <param name="numRange"></param>
    /// <param name="mainVariable"></param>
    /// <param name="fallbackToMinOnly">Default True only considers min on invalid ranges, if False it will return true if the range is invalid.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool NumberFilter(this NumericRange? numRange, int? mainVariable, bool fallbackToMinOnly = true)
    {
        if (mainVariable is null)
            throw new ArgumentNullException(nameof(mainVariable));
        if (numRange is null)
            throw new ArgumentNullException(nameof(numRange));

        var (min, max) = numRange;
        
        //Fallback to just min if range is invalid
        if (!numRange.IsValid())
        {
            if(fallbackToMinOnly)
                return (mainVariable >= min);
            return true;
        }
            

        return numRange.ContainsValue(mainVariable);
    }

    public static bool ListFilter<T>(this HashSet<T>? collection, T? propertyToCheck)
    {
        if (collection is null)
            return true;
        if (collection.Count == 0)
            return true;
        if (propertyToCheck is null)
            throw new ArgumentNullException(nameof(propertyToCheck));
        
        return collection.Contains(propertyToCheck);
    }
}