using System.Xml.Serialization;
using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace Chummer_Database.Classes;

public static class XmlLoader
{
    public static WeaponsXmlRoot? WeaponXmlData { get; private set; }
    public static RangesXmlRoot? RangesXmlData { get; private set; }
    public static SkillsXmlRoot? SkillsXmlData { get; private set; }

    public static async Task<bool> LoadAll(HttpClient client, ILogger logger)
    {
        //Later data may depend on this either while creating or deserializing
        logger.LogInformation("Started priority tasks");
        Task<bool>[] priorityTasks =
        {
            LoadSkillsXml(client, logger)
        };

        await Task.WhenAll(priorityTasks);
        logger.LogInformation("Finished Priority Tasks");
        
        Task<bool>[] tasks = 
        {
            LoadWeaponsXml(client, logger),
            LoadRangesXml(client, logger)
        };

        await Task.WhenAll(tasks);
        logger.LogTrace("Finished LoadingAll");
        return true;
    }


    public static async Task<bool> LoadWeaponsXml(HttpClient client, ILogger logger)
    { 
        logger.LogDebug("Loading WeaponsXml");
        var xmlString = await client.GetStringAsync("data/weapons.xml");
        var serializer = new XmlSerializer(typeof(WeaponsXmlRoot));
        
        using var stringReader = new StringReader(xmlString);
        using var reader = new CustomXmlReader(stringReader);
        
        WeaponXmlData = serializer.Deserialize(reader) as WeaponsXmlRoot;
        
        WeaponXmlData?.Create(logger);

        return true;
    }

    public static async Task<bool> LoadRangesXml(HttpClient client, ILogger logger)
    {
        logger.LogDebug("Loading Ranges.xml");
        var xmlString = await client.GetStringAsync("data/ranges.xml");
        var serializer = new XmlSerializer(typeof(RangesXmlRoot));
        
        using var stringReader = new StringReader(xmlString);
        using var reader = new CustomXmlReader(stringReader);
        
        RangesXmlData = serializer.Deserialize(reader) as RangesXmlRoot;
        
        RangesXmlData?.Create(logger);

        return true;
    }
    
    public static async Task<bool> LoadSkillsXml(HttpClient client, ILogger logger)
    {
        logger.LogDebug("Loading Skills.xml");
        var xmlString = await client.GetStringAsync("data/skills.xml");
        var serializer = new XmlSerializer(typeof(SkillsXmlRoot));
        
        using var stringReader = new StringReader(xmlString);
        using var reader = new CustomXmlReader(stringReader);

        SkillsXmlData = serializer.Deserialize(reader) as SkillsXmlRoot;

        SkillsXmlData?.Create(logger);

        return true;
    }
}

