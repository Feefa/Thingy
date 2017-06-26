using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public interface IViewFactory
    {
        IView Create(string masterPage, string sectionMasterPage, string viewTemplate);
    }
}