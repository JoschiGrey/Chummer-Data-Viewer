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