namespace Thingy.GraphicsPlusGui
{
    public interface IElementStrategy
    {
        int Priority { get; }

        bool CanCreateElementFor(string elementType);

        IElement Create(string elementType);
    }

    public interface IElementStrategy<T> : IElementStrategy
    {
    };
}