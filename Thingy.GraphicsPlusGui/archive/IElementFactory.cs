namespace Thingy.GraphicsPlusGui
{
    public interface IElementFactory
    {
        IElement Create(string elementType);
    }
}