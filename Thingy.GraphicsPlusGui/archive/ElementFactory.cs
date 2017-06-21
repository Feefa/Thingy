using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public class ElementFactory : IElementFactory
    {
        private readonly IElementStrategy[] strategies;

        public ElementFactory(IElementStrategy[] strategies)
        {
            this.strategies = strategies;
        }

        public IElement Create(string elementType)
        {
            return strategies.OrderBy(s => s.Priority).First(s => s.CanCreateElementFor(elementType)).Create(elementType);
        }
    }
}
