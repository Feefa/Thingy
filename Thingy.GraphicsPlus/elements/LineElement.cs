using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public class LineElement : BaseElement
    {
        public override void Draw(Graphics graphics)
        {
            Pen pen = new Pen(Colors[0], Math.Max(Points[2].X, Points[2].Y));
            graphics.DrawLine(pen, Points[0], Points[1]);
        }
    }

    public class LineElementStrategy : ElementStrategyBase<LineElement> { }
}
