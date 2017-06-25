namespace Thingy.WebServerLite
{
    public interface IMimeTypeProvider
    {
        string GetMimeType(string filePath);
    }
}