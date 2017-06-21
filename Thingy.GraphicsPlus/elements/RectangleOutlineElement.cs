using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public class RectangleOutlineElement : BaseElement
    {
        public override void Draw(Graphics graphics)
        {
            Pen pen = new Pen(Colors[0], Math.Max(Points[2].X, Points[2].Y));
            graphics.DrawRectangles(pen, new RectangleF[] { StandardRectangleF });
        }
    }

    public class RectangleOutlineElementStrategy : ElementStrategyBase<RectangleOutlineElement> { }
}
