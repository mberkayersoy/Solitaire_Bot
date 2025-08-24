public class GameDataPathProvider : IPathProvider
{
    public string GetPath(string filePath)
    {
        return GameConstantData.GAME_DATA_PATH + "/" + filePath;
    }
}
