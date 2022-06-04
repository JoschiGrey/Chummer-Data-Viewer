using System.Reflection;
using System.Runtime.CompilerServices;

namespace ChummerDataViewer.Extensions;

public static class DictionaryExtensions
{
    /// <summary>
    /// Gets an object from an dictionary based on a string key
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="searchKey">A string that is to be fetched from the dictionary</param>
    /// <param name="logger"></param>
    /// <param name="expression">This is the caller argument expression, don't provide anything</param>
    /// <param name="callerName"></param>
    /// <param name="callerFile"></param>
    /// <param name="callerLineNumber"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TValue GetValueByString<TValue>(this Dictionary<string, TValue>? dictionary, string searchKey,
        ILogger? logger,
        [CallerArgumentExpression("dictionary")]
        string expression = "",
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string callerFile = "",
        [CallerLineNumber] int callerLineNumber = 0)

    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));


        if (dictionary is null)
            throw new ArgumentNullException(expression);

        try
        {
            return dictionary[searchKey];
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error in fetching {SearchKey} from {Dictionary}, the caller was {CallerName} from {CallerFile} Line: {CallerLineNumber}",
                searchKey, expression, callerName, callerFile, callerLineNumber);
            throw;
        }

    }
}