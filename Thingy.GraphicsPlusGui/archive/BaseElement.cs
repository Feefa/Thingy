using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public abstract class BaseElement : IElement
    {
        private SizeF? standardSize = null;
        private Brush standardSolidBrush = null;

        protected IList<PointF> Points = new List<PointF>();
        protected IList<Color> Colors = new List<Color>();

        protected PointF StandardLocation
        {
            get
            {
                return Points[0];
            }
        }

        protected SizeF StandardSizeF
        {
            get
            {
                if (standardSize == null)
                {
                    standardSize = new SizeF(Math.Abs(Points[1].X - Points[0].X), Math.Abs(Points[1].Y - Points[0].Y));
                }

                return (SizeF)standardSize;
            }
        }

        protected RectangleF StandardRectangleF
        {
            get
            {
                return new RectangleF(Math.Min(Points[0].X, Points[1].X), Math.Min(Points[0].Y, Points[1].Y), Math.Abs(Points[1].X - Points[0].X), Math.Abs(Points[1].Y - Points[0].Y));
            }
        }

        protected Brush StandardSolidBrush
        {
            get
            {
                if (standardSolidBrush == null)
                {
                    standardSolidBrush = new SolidBrush(Colors[0]);
                }

                return standardSolidBrush;
            }
        }

        protected PointF OffsetPointF(PointF point, float deltaX, float deltaY)
        {
            return new PointF(point.X + deltaX, point.Y + deltaY);
        }

        public float ZIndex { get; set; }

        public void AddColor(Color color)
        {
            Colors.Add(color);
        }

        public void AddPoint(PointF point)
        {
            Points.Add(point);
        }

        public abstract void Draw(Graphics graphics);
    }
}
