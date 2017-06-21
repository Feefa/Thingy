using System.Drawing;

namespace Thingy.GraphicsPlus
{
    public interface IElement
    {
        float ZIndex { get; set; }
        PointF[] Points { get; }
        Color[] Colors { get; }

        void AddPoint(PointF pointF);
        void AddColor(Color color);
        void Draw(Graphics graphics);
    }
}