using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public class JointFactory : IJointFactory
    {
        public IJoint Create()
        {
            return new Joint();
        }
    }
}
