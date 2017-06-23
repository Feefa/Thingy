namespace Thingy.WebServerLite.Api
{
    public interface IView
    {
        IViewResult Render(object o);
    }
}