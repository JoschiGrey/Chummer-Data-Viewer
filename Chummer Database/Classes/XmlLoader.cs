﻿using System.Xml.Serialization;
using Blazor.Extensions.Logging;
using Chummer_Database.Overrides;
using Microsoft.AspNetCore.Components;

namespace Chummer_Database.Classes;

public static class XmlLoader
{
    public static WeaponsXmlRoot? WeaponXmlData { get; private set; }
    public static RangesXmlRoot? RangesXmlData { get; private set; }
    public static SkillsXmlRoot? SkillsXmlData { get; private set; }
    
    public static BooksXmlRoot? BooksXmlData { get; private set; }

    public static async Task<bool> LoadAll(HttpClient client, ILogger logger)
    {
        //I Really need to write some kind of dependency tree to load stuff in the correct order
        //Info is never needed in deserialization. So we could paralelize  all ÍO calls and only need
        //to handle the Create() calls in a correct order
        
        //Basically everything needs this.
        await LoadBooksXml(client, logger);
        
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
        logger.LogInformation("Finished LoadingAll");
        return true;
    }


    private static async Task<bool> LoadWeaponsXml(HttpClient client, ILogger logger)
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

    private static async Task<bool> LoadRangesXml(HttpClient client, ILogger logger)
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
    
    private static async Task<bool> LoadSkillsXml(HttpClient client, ILogger logger)
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
    
    private static async Task<bool> LoadBooksXml(HttpClient client, ILogger logger)
    {
        logger.LogDebug("Loading Books.xml");
        var xmlString = await client.GetStringAsync("data/books.xml");
        var serializer = new XmlSerializer(typeof(BooksXmlRoot));
        
        using var stringReader = new StringReader(xmlString);
        using var reader = new CustomXmlReader(stringReader);

        BooksXmlData = serializer.Deserialize(reader) as BooksXmlRoot;

        BooksXmlData?.Create(logger);

        return true;
    }
}

