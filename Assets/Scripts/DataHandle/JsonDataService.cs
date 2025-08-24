using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class JsonDataService : IDataService
{
    private const string EXTENSION = ".json";
    private readonly IPathProvider _pathProvider;
    public JsonDataService(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    public T LoadData<T>(string filePath)
    {
        string path = _pathProvider.GetPath(filePath + EXTENSION);
        if (File.Exists(path))
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        else
        {
            return default;
        }
    }
    public void SaveData<T>(string filePath, T data)
    {
        string path = _pathProvider.GetPath(filePath + EXTENSION);
        try
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public bool CheckFileExistince(string filePath)
    {
        string path = _pathProvider.GetPath(filePath + EXTENSION);

        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
