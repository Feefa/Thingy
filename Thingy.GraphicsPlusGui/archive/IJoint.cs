using System.Drawing;

namespace Thingy.GraphicsPlusGui
{
    public interface IJoint
    {
        PointF Location { get; set; }
        float Rotation { get; set; }
        float ZIndex { get; set; }
        void Draw(Graphics graphics);
        void AddJoint(IJoint joint);
        void AddElement(IElement element);
    }
}