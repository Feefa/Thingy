namespace Thingy.GraphicsPlus
{
    public interface IElementStrategy
    {
        int Priority { get; }

        bool CanCreateElementFor(string elementType);

        IElement Create(string elementType);
    }
}