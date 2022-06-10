
using System.IO.Compression;
using System.Reflection;

using System.Xml.Serialization;
using ChummerDataViewer.Classes.XmlRoots;
using ChummerDataViewer.Interfaces;
using ChummerDataViewer.Overrides;


namespace ChummerDataViewer.Classes;

public static class XmlLoader
{
    private static readonly Dictionary<Type, string> PathDictionary = new()
    {
        {typeof(WeaponsXmlRoot), "data/weapons.zz"},
        {typeof(RangesXmlRoot), "data/ranges.zz"},
        {typeof(SkillsXmlRoot), "data/skills.zz"},
        {typeof(BooksXmlRoot), "data/books.zz"}
    };

    public static HashSet<Type> CreatedXml { get; set; } = new HashSet<Type>();

    private static Dictionary<Type, ICreatable> LoadedXml { get; set; } = new();

    public static async Task LoadAllAsync<T>(HttpClient client, ILogger logger) where T : ICreatable
    {
        var path = PathDictionary[typeof(T)];
        var iCreatableObjects = new List<ICreatable>();
        var xmlObject = await LoadXmlAsync<T>(path, client, logger);
        iCreatableObjects.Add(xmlObject);
        if (xmlObject is IHasDependency hasDependencies)
            iCreatableObjects.AddRange(await LoadXmlRangeAsync(hasDependencies.Dependencies, client, logger));


        var waitingCreations = new List<IHasDependency>();
        
        
        
        
        var creationTasks = new List<Task>();
        foreach (var creatable in iCreatableObjects)
        {
            if (creatable is IHasDependency hasDependency && !hasDependency.CheckDependencies())
            {
                waitingCreations.Add(hasDependency);
                logger.LogInformation("Put {Type} in the creation waiting que", hasDependency.GetType());
                continue;
            }

            creationTasks.Add(creatable.CreateAsync(logger));

            //Check all awaiting creations and put their CreateAsync() in the task list if the dependencies are fulfilled
            for (var i = waitingCreations.Count; i > 0 ; i--)
            {
                var waitingCreation = waitingCreations[i - 1];
                if (!waitingCreation.CheckDependencies()) continue;
                
                waitingCreations.Remove(waitingCreation);
                var creatableTask = (ICreatable) waitingCreation;
                creationTasks.Add(creatableTask.CreateAsync(logger));
            }

        }

        //Make sure, that all creation tasks are completed and also that all IHasDependency that are waiting are thrown into the que
        while (creationTasks.Any())
        {
            var finishedTask = await Task.WhenAny(creationTasks).ConfigureAwait(false);
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

    private static async Task<List<ICreatable>> LoadXmlRangeAsync(IEnumerable<Type> listOfTypesToDeserialize, HttpClient client, ILogger logger)
    {
        var arrayTypesToDeserialize = listOfTypesToDeserialize as Type[] ?? listOfTypesToDeserialize.ToArray();
        //If all types in the list are already loaded, we can save us the reflection and method generic method invocation / creation.
        if (arrayTypesToDeserialize.ToHashSet().IsSubsetOf(LoadedXml.Keys))
            return new List<ICreatable>();
        
        
        var outputList = new List<ICreatable>();
        var deserializeTasks = new List<Task<ICreatable>>();
        var method = typeof(XmlLoader).GetMethod(nameof(LoadXmlAsync), BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method is null)
            throw new NullReferenceException(nameof(method));
        
        foreach (var type in arrayTypesToDeserialize)
        {
            var path = PathDictionary[type];
            
            var generic = method.MakeGenericMethod(type);
            var invoke = generic.Invoke(null, new object?[] {path, client, logger});
            if (invoke is null)
                throw new NullReferenceException(nameof(invoke));
            var task = (Task<ICreatable>) invoke;
            deserializeTasks.Add(task);
        }

        while (deserializeTasks.Any())
        {
            var finishedTask = await Task.WhenAny(deserializeTasks);
            deserializeTasks.Remove(finishedTask);
            
            var result = await finishedTask;
            outputList.Add(result);
            if (result is IHasDependency hasDependency)
                outputList.AddRange(await LoadXmlRangeAsync(hasDependency.Dependencies, client, logger));
        }

        return outputList;
    }

    private static async Task<ICreatable> LoadXmlAsync<T>(string path, HttpClient client, ILogger logger) where T : ICreatable
    {
        if (LoadedXml.ContainsKey(typeof(T)))
        {
            logger.LogInformation("Skipped Loading of {Type}, because it is already loaded", typeof(T).FullName);
            return LoadedXml[typeof(T)];
        }
        
        
        logger.LogInformation("Loading {Type}", typeof(T).FullName);
        
        var input = client.GetStreamAsync(path);

        await using var ms = new MemoryStream();
        //Sadly Brotli is not supported on browsers, could have saved some of those sweet sweet kB download size :/
        await using var decompressor = new DeflateStream(await input, CompressionMode.Decompress);
        await decompressor.CopyToAsync(ms);
        ms.Position = 0;
        using var streamReader = new StreamReader(ms);
        using var reader = new CustomXmlReader(streamReader);
        var serializer = new XmlSerializer(typeof(T));
        var xmlData = serializer.Deserialize(reader);
            
        if (xmlData is null)
            throw new NullReferenceException(nameof(xmlData) + typeof(T).FullName);

        var value = (ICreatable)xmlData;

        LoadedXml.Add(typeof(T), value);
        
        logger.LogInformation("Finished Loading {Type}", typeof(T).FullName);
        
        return value;
    }
}



