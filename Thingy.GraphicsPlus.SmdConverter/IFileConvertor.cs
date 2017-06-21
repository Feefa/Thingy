using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlus.SmdConverter
{
    public interface IFileConverter
    {
        string Convert(string inputFile);

        void Convert(string inputFile, string outputFile);
    }
}
