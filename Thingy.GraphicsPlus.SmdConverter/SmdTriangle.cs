using System.Collections.Generic;

namespace Thingy.GraphicsPlus.SmdConverter
{
    internal class SmdTriangle
    {
        internal SmdTriangle()
        {
            Vertices = new List<SmdVertex>();
        }

        public string Material { get; internal set; }
        public IList<SmdVertex> Vertices { get; private set; }
    }
}