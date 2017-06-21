namespace Thingy.GraphicsPlus.SmdConverter
{
    internal class SkeletonNode
    {
        public SkeletonNode(int ParentNode)
        {
            this.ParentNode = ParentNode;
        }

        public int ParentNode { get; private set; }
        public float RotX { get; internal set; }
        public float RotY { get; internal set; }
        public float RotZ { get; internal set; }
        public float PosX { get; internal set; }
        public float PosY { get; internal set; }
        public float PosZ { get; internal set; }
    }
}