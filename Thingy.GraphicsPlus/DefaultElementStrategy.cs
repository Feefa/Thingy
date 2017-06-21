using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public class DefaultElementStrategy : IElementStrategy
    {
        public int Priority
        {
            get
            {
                return Int32.MaxValue;
            }
        }

        public bool CanCreateElementFor(string elementType)
        {
            return true;
        }

        public IElement Create(string elementType)
        {
            return new Element();
        }
    }
}
