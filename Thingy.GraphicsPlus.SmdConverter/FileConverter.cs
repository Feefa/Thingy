using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.Diagnostics.Api;

namespace Thingy.GraphicsPlus.SmdConverter
{
    public class FileConverter : IFileConverter
    {
        private readonly IDiagnosticConsole diagnostics;
        private readonly char[] itemSeparators = new char[] { ' ' };

        public FileConverter(IDiagnosticConsole diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public string Convert(string inputFile)
        {
            string outputFile = Path.ChangeExtension(inputFile, ".STR");
            Convert(inputFile, outputFile);

            return outputFile;
            
        }

        public void Convert(string inputFile, string outputFile)
        {
            IDictionary<int, SkeletonNode> nodeTree = new Dictionary<int, SkeletonNode>();
            IList<SmdTriangle> triangles = new List<SmdTriangle>();

            using (StreamReader reader = new StreamReader(inputFile))
            {
                diagnostics.WriteMessage(this, "Convert(string,string)", DiagnosticLevels.Information, "Reading SMD File");
                ParseSmdFile(reader, nodeTree, triangles);
            }

            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                diagnostics.WriteMessage(this, "Convert(string,string)", DiagnosticLevels.Information, "Writing STR File");

                foreach (int boneId in nodeTree.Keys)
                {                    
                    string boneName = GetBoneName(nodeTree, boneId);
                    writer.WriteLine(string.IsNullOrEmpty(boneName) ? "Joint" : string.Format("Joint {0}", boneName));
                    writer.WriteLine(string.Format("{0},{1}", 256 * nodeTree[boneId].PosX, 256 * nodeTree[boneId].PosY));
                    writer.WriteLine(string.Format("{0}", 180 + nodeTree[boneId].RotZ * 57.295791433133264917914229473464));
                    writer.WriteLine(string.Format("{0}", nodeTree[boneId].PosZ));
                    writer.WriteLine("");
                    writer.WriteLine(string.IsNullOrEmpty(boneName) ? "Element dot" : string.Format("Element {0}.dot", boneName));
                    writer.WriteLine("Circle");
                    writer.WriteLine("0,0 1,0");
                    writer.WriteLine("Blue");
                    writer.WriteLine("10");
                    writer.WriteLine("");
                }

                int triangleNo = 1;

                foreach(int boneId in nodeTree.Keys)
                {
                    int parentBoneId = nodeTree[boneId].ParentNode;

                    string boneName = GetBoneName(nodeTree, boneId);

                    if (parentBoneId != -1)
                    {
                        string parentBoneName = GetBoneName(nodeTree, parentBoneId);

                        if (!string.IsNullOrEmpty(parentBoneName))
                        {
                            writer.WriteLine(string.Format("Triangle t{0}", triangleNo++));
                            writer.WriteLine(string.Format("{0} 0,0", boneName));
                            writer.WriteLine(string.Format("{0} 1,-1", parentBoneName));
                            writer.WriteLine(string.Format("{0} -1,1", parentBoneName));
                            writer.WriteLine("Black");
                            writer.WriteLine("0");
                            writer.WriteLine("");
                        }
                    }
                }

                ////foreach (SmdTriangle triangle in triangles)
                ////{
                ////    triangleNo = CreateRegularTriangle(nodeTree, writer, triangleNo, triangle);
                ////}
            }
        }

        private int CreateRegularTriangle(IDictionary<int, SkeletonNode> nodeTree, StreamWriter writer, int triangleNo, SmdTriangle triangle)
        {
            writer.WriteLine(string.Format("Triangle t{0}", triangleNo++));
            int parentBoneId = triangle.Vertices[0].ParentBone;
            string parentBoneName = GetBoneName(nodeTree, parentBoneId);
            writer.WriteLine(string.Format("{0} {1},{2}", parentBoneName, 128 * triangle.Vertices[0].PosX, 128 * triangle.Vertices[0].PosY));
            parentBoneId = triangle.Vertices[1].ParentBone;
            parentBoneName = GetBoneName(nodeTree, parentBoneId);
            writer.WriteLine(string.Format("{0} {1},{2}", parentBoneName, 128 * triangle.Vertices[1].PosX, 128 * triangle.Vertices[1].PosY));
            parentBoneId = triangle.Vertices[2].ParentBone;
            parentBoneName = GetBoneName(nodeTree, parentBoneId);
            writer.WriteLine(string.Format("{0} {1},{2}", parentBoneName, 128 * triangle.Vertices[2].PosX, 128 * triangle.Vertices[2].PosY));
            writer.WriteLine("Black");
            writer.WriteLine(string.Format("{0}", triangle.Vertices[0].PosZ));
            writer.WriteLine("");
            return triangleNo;
        }

        private string GetBoneName(IDictionary<int, SkeletonNode> nodeTree, int boneId)
        {
            int parentBoneId = nodeTree[boneId].ParentNode;

            if (parentBoneId == -1)
            {
                return string.Empty;
            }

            string parentBoneName = GetBoneName(nodeTree, parentBoneId);

            return string.Format(string.IsNullOrEmpty(parentBoneName) ? "b{1}" : "{0}.b{1}", parentBoneName, boneId);
        }

        private void ParseSmdFile(StreamReader reader, IDictionary<int, SkeletonNode> nodeTree, IList<SmdTriangle> triangles)
        {
            ReadVersion(reader);
            BuildNodeTree(reader, nodeTree);
            BuildSkeleton(reader, nodeTree);
            ReadTriangles(reader, triangles);
        }

        private void ReadTriangles(StreamReader reader, IList<SmdTriangle> triangles)
        {
            ReadTrianglesHeader(reader);
            string line = reader.ReadLine();

            while (line != "end")
            {
                SmdTriangle triangle = new SmdTriangle();
                triangle.Material = line;

                for (int i = 0; i < 3; i++)
                {
                    triangle.Vertices.Add(CreateVertexFromLine(reader.ReadLine()));
                }

                triangles.Add(triangle);
                line = reader.ReadLine();
            }
        }

        private SmdVertex CreateVertexFromLine(string line)
        {
            SmdVertex vertex = new SmdVertex();
            string[] items = line.Split(itemSeparators);
            vertex.ParentBone = System.Convert.ToInt32(items[0]);
            vertex.PosX = System.Convert.ToSingle(items[1]);
            vertex.PosY = System.Convert.ToSingle(items[2]);
            vertex.PosZ = System.Convert.ToSingle(items[3]);
            vertex.NormX = System.Convert.ToSingle(items[4]);
            vertex.NormY = System.Convert.ToSingle(items[5]);
            vertex.NormZ = System.Convert.ToSingle(items[6]);
            vertex.TextureU = System.Convert.ToSingle(items[7]);
            vertex.TextureV = System.Convert.ToSingle(items[8]);
            return vertex;
        }

        private void ReadTrianglesHeader(StreamReader reader)
        {
            string line = reader.ReadLine();
            diagnostics.WriteMessage(GetType().Name, "ReadTrainglesHeader", DiagnosticLevels.Information, line);

            if (line != "triangles")
            {
                throw new InvalidOperationException("triangles header expected");
            }
        }

        private void BuildSkeleton(StreamReader reader, IDictionary<int, SkeletonNode> nodeTree)
        {
            ReadSkeletonHeader(reader);
            ReadTimeIndex(reader);
            string line = reader.ReadLine();

            while (line != "end")
            {
                string[] items = line.Split(itemSeparators);
                int boneId = System.Convert.ToInt32(items[0]);
                nodeTree[boneId].PosX = System.Convert.ToSingle(items[1]);
                nodeTree[boneId].PosY = System.Convert.ToSingle(items[2]);
                nodeTree[boneId].PosZ = System.Convert.ToSingle(items[3]);
                nodeTree[boneId].RotX = System.Convert.ToSingle(items[4]);
                nodeTree[boneId].RotY = System.Convert.ToSingle(items[5]);
                nodeTree[boneId].RotZ = System.Convert.ToSingle(items[6]);
                line = reader.ReadLine();
            }
        }

        private void ReadTimeIndex(StreamReader reader)
        {
            string line = reader.ReadLine();
            diagnostics.WriteMessage(GetType().Name, "ReadTimeIndex", DiagnosticLevels.Information, line);
        }

        private void ReadSkeletonHeader(StreamReader reader)
        {
            string line = reader.ReadLine();
            diagnostics.WriteMessage(GetType().Name, "ReadSkeletonHeader", DiagnosticLevels.Information, line);

            if (line != "skeleton")
            {
                throw new InvalidOperationException("skeleton header expected");
            }
        }

        private void BuildNodeTree(StreamReader reader, IDictionary<int, SkeletonNode> nodeTree)
        {
            ReadNodesHeader(reader);
            string line = reader.ReadLine();

            while (line != "end")
            {
                string[] items = line.Split(itemSeparators);
                nodeTree[System.Convert.ToInt32(items[0])] = new SkeletonNode(System.Convert.ToInt32(items[2]));
                line = reader.ReadLine();
            }
        }

        private void ReadNodesHeader(StreamReader reader)
        {
            string line = reader.ReadLine();
            diagnostics.WriteMessage(GetType().Name, "ReadNodesHeader", DiagnosticLevels.Information, line);

            if (line != "nodes")
            {
                throw new InvalidOperationException("nodes header expected");
            }
        }

        private void ReadVersion(StreamReader reader)
        {
            string line = reader.ReadLine();
            diagnostics.WriteMessage(GetType().Name, "ReadVersion", DiagnosticLevels.Information, line);
        }
    }
}
