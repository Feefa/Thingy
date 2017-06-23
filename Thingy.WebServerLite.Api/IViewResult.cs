namespace Thingy.WebServerLite.Api
{
    public interface IViewResult
    {
        string Content { get; }

        string ContentType { get; }
    }
}