using System.Runtime.CompilerServices;

namespace Chummer_Database.Extensions;

public static class DictionaryExtensions
{
    /// <summary>
    /// Gets an object from an dictionary based on a string key
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="searchKey">A string that is to be fetched from the dictionary</param>
    /// <param name="expression">This is the caller argument expression, don't provide anything</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TValue GetValueByString<TValue>(this Dictionary<string, TValue>? dictionary , string searchKey, [CallerArgumentExpression("dictionary")] string expression = "")
    {
        if (dictionary is null)
            throw new ArgumentNullException(expression);

        return dictionary[searchKey];
    }
}