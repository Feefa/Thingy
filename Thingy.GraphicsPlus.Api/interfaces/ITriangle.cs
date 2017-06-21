using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public interface ITriangle : IElement
    {
        void AddVertex(IJoint joint, PointF point);
    }
}
