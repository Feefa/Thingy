using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public class EllipseElement : BaseElement
    {
        public override void Draw(Graphics graphics)
        {
            graphics.FillEllipse(StandardSolidBrush, StandardRectangleF);
        }
    }

    public class CircleElementStrategy : ElementStrategyBase<EllipseElement> { }

    public class EllipseElementStrategy : ElementStrategyBase<EllipseElement> { }
}
