namespace Thingy.GraphicsPlus.SmdConverter
{
    internal class SmdVertex
    {
        public float NormX { get; internal set; }
        public float NormY { get; internal set; }
        public float NormZ { get; internal set; }
        public int ParentBone { get; internal set; }
        public float PosX { get; internal set; }
        public float PosY { get; internal set; }
        public float PosZ { get; internal set; }
        public float TextureU { get; internal set; }
        public float TextureV { get; internal set; }
    }
}