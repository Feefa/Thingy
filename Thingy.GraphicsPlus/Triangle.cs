using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public class Triangle : ITriangle
    {
        public float ZIndex { get; set; }

        public PointF[] Points
        {
            get
            {
                return points.ToArray();
            }
        }

        public Color[] Colors
        {
            get
            {
                return colors.ToArray();
            }
        }

        private Brush StandardSolidBrush
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

        private IList<PointF> points = new List<PointF>();
        private IList<Color> colors = new List<Color>();
        private IList<IJoint> joints = new List<IJoint>();
        private Brush standardSolidBrush;

        public void Draw(Graphics graphics)
        {
            Matrix storedMatrix = graphics.Transform;
            graphics.ResetTransform();
            PointF[] p1 = new PointF[] { points[0] };
            PointF[] p2 = new PointF[] { points[1] };
            PointF[] p3 = new PointF[] { points[2] };
            joints[0].TransformMatrix.TransformPoints(p1);
            joints[1].TransformMatrix.TransformPoints(p2);
            joints[2].TransformMatrix.TransformPoints(p3);
            PointF[] p = new PointF[] { p1[0], p2[0], p3[0] };
            graphics.FillPolygon(StandardSolidBrush, p);
            graphics.Transform = storedMatrix;
        }

        public void AddPoint(PointF pointF)
        {
            throw new NotImplementedException("Can not add a point to a triangle. Call AddVertex instead.");
        }

        public void AddColor(Color color)
        {
            colors.Add(color);
        }

        public void AddVertex(IJoint joint, PointF point)
        {
            joints.Add(joint);
            points.Add(point);
        }
    }
}
