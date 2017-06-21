namespace Thingy.GraphicsPlus
{
    public interface IElementFactory
    {
        IElement Create(string elementType);
    }
}