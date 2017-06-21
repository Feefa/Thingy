using System;
using System.Drawing;

namespace Thingy.GraphicsPlus
{
    public class Element : BaseElement
    {
        public override void Draw(Graphics graphics)
        {
            foreach (PointF point in Points)
            {
                Pen pen = new Pen(Colors[0], 2.0f);
                graphics.DrawLine(pen, OffsetPointF(point, 3.0f, 3.0f), OffsetPointF(point, -3.0f, -3.0f));
                graphics.DrawLine(pen, OffsetPointF(point, 3.0f, -3.0f), OffsetPointF(point, -3.0f, 3.0f));
            }
        }
    }
}