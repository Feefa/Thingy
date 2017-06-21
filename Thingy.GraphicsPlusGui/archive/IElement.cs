using System.Drawing;

namespace Thingy.GraphicsPlusGui
{
    public interface IElement
    {
        float ZIndex { get; set; }
        void AddPoint(PointF pointF);
        void AddColor(Color color);
        void Draw(Graphics graphics);
    }
}