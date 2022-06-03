
using System.Reflection;

using System.Xml.Serialization;

using Chummer_Database.Interfaces;
using Chummer_Database.Overrides;


namespace Chummer_Database.Classes;

public static class XmlLoader
{
    private static Dictionary<Type, ICreatable> DataDic { get; set; } = new Dictionary<Type, ICreatable>();

    public static WeaponsXmlRoot? WeaponXmlData => DataDic[typeof(WeaponsXmlRoot)] as WeaponsXmlRoot;
    public static RangesXmlRoot? RangesXmlData => DataDic[typeof(RangesXmlRoot)] as RangesXmlRoot;
    public static SkillsXmlRoot? SkillsXmlData => DataDic[typeof(SkillsXmlRoot)] as SkillsXmlRoot;
    public static BooksXmlRoot? BooksXmlData => DataDic[typeof(BooksXmlRoot)] as BooksXmlRoot;

    private static readonly Dictionary<Type, string> PathDictionary = new()
    {
        {typeof(WeaponsXmlRoot), "data/weapons.xml"},
        {typeof(RangesXmlRoot), "data/ranges.xml"},
        {typeof(SkillsXmlRoot), "data/skills.xml"},
        {typeof(BooksXmlRoot), "data/books.xml"}
    };

    public static HashSet<Type> CreatedXml { get; set; } = new HashSet<Type>();

    public static async Task LoadAllAsync(HttpClient client, ILogger logger)
    {
        //This should be replaced by individual checks on each loading item, but only as soon as it actually uses a list of stuff as input instead of "everything".
        if(CreatedXml.Count > 0)
            return;
        
        var deserializeTasks = new List<Task<ICreatable>>();

        var method = typeof(XmlLoader).GetMethod(nameof(LoadXmlAsync), BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (method is null)
            throw new NullReferenceException(nameof(method));
        
        foreach (var (type, path) in PathDictionary)
        {
            var generic = method.MakeGenericMethod(type);
            var invoke = generic.Invoke(null, new object?[] {path, client, logger});
            if (invoke is null)
                throw new NullReferenceException(nameof(invoke));
            var task = (Task<ICreatable>) invoke;
            deserializeTasks.Add(task);
        }
        


        var waitingCreations = new List<IHasDependency>();
        var creationTasks = new List<Task<ICreatable>>();
        while (deserializeTasks.Any())
        {
            Task<ICreatable> finishedTask = await Task.WhenAny(deserializeTasks);
            deserializeTasks.Remove(finishedTask);

            var result = await finishedTask;

            DataDic.Add(key: result.GetType(), value: result);

            if (result is IHasDependency hasDependency && !hasDependency.CheckDependencies())
            {
                waitingCreations.Add(hasDependency);
                logger.LogInformation("Put {Type} in the creation waiting que", hasDependency.GetType());
                continue;
            }

            creationTasks.Add(result.CreateAsync(logger));

            //Check all awaiting creations and put their CreateAsync() in the task list if the dependencies are fulfilled
            for (var i = waitingCreations.Count; i > 0 ; i--)
            {
                var waitingCreation = waitingCreations[i - 1];
                if (!waitingCreation.CheckDependencies()) continue;
                
                waitingCreations.Remove(waitingCreation);
                var creatable = (ICreatable) waitingCreation;
                creationTasks.Add(creatable.CreateAsync(logger));
            }
        }

        //Make sure, that all creation tasks are completed and also that all IHasDependency that are waiting are thrown into the que
        while (creationTasks.Any())
        {
            var finishedTask = await Task.WhenAny(creationTasks);
            creationTasks.Remove(finishedTask);

            for (var i = waitingCreations.Count; i > 0 ; i--)
            {
                var waitingCreation = waitingCreations[i - 1];
                if (!waitingCreation.CheckDependencies()) continue;
                
                waitingCreations.Remove(waitingCreation);
                var creatable = (ICreatable) waitingCreation;
                creationTasks.Add(creatable.CreateAsync(logger));
            }
        }
    }
    
    private static async Task<ICreatable> LoadXmlAsync<T>(string path, HttpClient client, ILogger logger) where T : ICreatable
    {
        logger.LogInformation("Loading {Type}", typeof(T).FullName);

        var xmlString = client.GetStringAsync(path);
        var serializer = new XmlSerializer(typeof(T));
        
        using var stringReader = new StringReader(await xmlString);
        using var reader = new CustomXmlReader(stringReader);
        
        var xmlData = serializer.Deserialize(reader);
        if (xmlData is null)
            throw new NullReferenceException(nameof(xmlData) + typeof(T).FullName);

        var value = (ICreatable)xmlData;
        
        logger.LogInformation("Finished Loading {Type}", typeof(T).FullName);
        
        return value;
    }
}



