using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Thingy.GraphicsPlusGui
{
    public class Joint : IJoint
    {
        private readonly IList<IElement> elements = new List<IElement>();
        private readonly IList<IJoint> joints = new List<IJoint>();

        public PointF Location { get; set; }

        public float Rotation { get; set; }

        public float ZIndex { get; set; }

        public void AddElement(IElement element)
        {
            elements.Add(element);
        }

        public void AddJoint(IJoint joint)
        {
            joints.Add(joint);
        }

        public void Draw(Graphics graphics)
        {
            graphics.TranslateTransform(Location.X, Location.Y);
            ////DrawElementsByZIndex(graphics, -100.0f, -1000.0f);
            graphics.RotateTransform(Rotation);
            DrawElementsByZIndex(graphics, Single.MinValue, 0.0f);
            DrawChildJoints(graphics);
            DrawElementsByZIndex(graphics, 0.0f, Single.MaxValue);
            graphics.RotateTransform(-Rotation);
            ////DrawElementsByZIndex(graphics, 100.0f, 1000.0f);
            graphics.TranslateTransform(-Location.X, -Location.Y);
        }

        private void DrawChildJoints(Graphics graphics)
        {
            joints.OrderBy(j => j.ZIndex).ToList().ForEach(j => j.Draw(graphics));
        }

        private void DrawElementsByZIndex(Graphics graphics, float inclusiveBound, float exclusiveBound)
        {
            elements.Where(e => inclusiveBound < exclusiveBound && e.ZIndex >= inclusiveBound && e.ZIndex < exclusiveBound ||
                               inclusiveBound > exclusiveBound && e.ZIndex > exclusiveBound && e.ZIndex < inclusiveBound)
                .OrderBy(e => e.ZIndex)
                .ToList()
                .ForEach(e => e.Draw(graphics));
        }
    }
}