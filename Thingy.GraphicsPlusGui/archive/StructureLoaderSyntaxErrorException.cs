using System;
using System.Runtime.Serialization;

namespace Thingy.GraphicsPlusGui
{
    [Serializable]
    internal class StructureLoaderSyntaxErrorException : Exception
    {
        public StructureLoaderSyntaxErrorException()
        {
        }

        public StructureLoaderSyntaxErrorException(string message) : base(message)
        {
        }

        public StructureLoaderSyntaxErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StructureLoaderSyntaxErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}