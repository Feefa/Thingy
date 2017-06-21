using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus
{
    public abstract class ElementStrategyBase<T> : IElementStrategy where T : IElement, new()
    {
        public int Priority
        {
            get
            {
                return typeof(T).GetHashCode();
            }
        }

        public bool CanCreateElementFor(string elementType)
        {
            return typeof(T).Name.StartsWith(elementType);
        }

        public IElement Create(string elementType)
        {
            return new T();
        }
    }
}
