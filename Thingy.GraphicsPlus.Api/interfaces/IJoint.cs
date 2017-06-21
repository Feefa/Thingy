using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Thingy.GraphicsPlus
{
    public interface IJoint
    {
        PointF Location { get; set; }
        float Rotation { get; set; }
        float ZIndex { get; set; }
        IEnumerable<IElement> Elements { get; }
        IEnumerable<IJoint> Joints { get; }
        Matrix TransformMatrix { get; }

        void Draw(Graphics graphics);
        void AddJoint(IJoint joint);
        void AddElement(IElement element);
        void AddTriangle(ITriangle triangle);
        bool RemoveJoint(IJoint joint);
    }
}