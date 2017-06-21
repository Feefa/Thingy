using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public class StructureProvider : IStructureProvider
    {
        private readonly IJointFactory jointFactory;
        private readonly IElementFactory elementFactory;

        public StructureProvider(IJointFactory jointFactory, IElementFactory elementFactory)
        {
            this.jointFactory = jointFactory;
            this.elementFactory = elementFactory;
        }

        public IJoint Create(string structure)
        {
            return CreateFromStructure(structure, null);
        }


        public IJoint Create(string structure, IDictionary<string, string> settings)
        {
            return CreateFromStructure(ApplySettings(structure, settings), null);
        }

        public IJoint Create(string structure, IDictionary<string, string> settings, IDictionary<string, string> structures)
        {
            return CreateFromStructure(ApplySettings(structure, settings), structures);
        }

        public IJoint CreateFromStructure(string structure, IDictionary<string, string> structures)
        {
            IList<string> lines = new List<string>(structure.Split(';'));
            IDictionary<string, IJoint> joints = new Dictionary<string, IJoint>();
            string line = PopLine(lines);

            while (!string.IsNullOrEmpty(line) || lines.Count > 0)
            {
                switch (line[0])
                {
                    case 'J': CreateJoint(lines, line, joints); break;
                    case 'D': CreateJoint(lines, line, joints, structures); break;
                    case 'E': CreateElement(lines, line, joints); break;
                }

                line = PopLine(lines);
            }

            return joints.Values.First();
        }

        private string PopLine(IList<string> lines)
        {
            string line = lines.FirstOrDefault();

            if (lines.Count > 0)
            {
                lines.Remove(line);
            }

            return line;
        }

        private void CreateJoint(IList<string> lines, string line, IDictionary<string, IJoint> joints)
        {
            string[] parts = line.Split(' ');
            IJoint joint = jointFactory.Create();
            CommonCreateJoint(lines, joints, parts, joint);
        }

        private void CreateJoint(IList<string> lines, string line, IDictionary<string, IJoint> joints, IDictionary<string, string> structures)
        {
            string[] parts = line.Split(' ');

            if (structures.ContainsKey(parts[2]))
            {
                IJoint joint = CreateFromStructure(structures[parts[2]], null);
                CommonCreateJoint(lines, joints, parts, joint);
            }
            else
            {
                throw new ArgumentException(string.Format("Dynamic joint \"{0}\" references unknown sub-structure \"{1}\".", parts[1], parts[2]));
            }
        }

        private void CommonCreateJoint(IList<string> lines, IDictionary<string, IJoint> joints, string[] parts, IJoint joint)
        {
            string locationLine = PopLine(lines);
            string[] locationValues = locationLine.Split(',');
            joint.Location = new PointF(Convert.ToSingle(locationValues[0]), Convert.ToSingle(locationValues[0]));
            joint.Rotation = Convert.ToSingle(PopLine(lines));
            joint.ZIndex = Convert.ToSingle(PopLine(lines));

            if (parts.Length > 1)
            {
                string parentName = GetParentName(parts[1]);
                IJoint parentJoint = joints[parentName];
                parentJoint.AddJoint(joint);
                joints.Add(parts[1], joint);
            }
            else
            {
                joints.Add(".", joint);
            }
        }

        private void CreateElement(IList<string> lines, string line, IDictionary<string, IJoint> joints)
        {
            string[] parts = line.Split(' ');
            IElement element = elementFactory.Create(PopLine(lines));
            AddElementPoints(element, PopLine(lines));
            AddElementColors(element, PopLine(lines));
            element.ZIndex = Convert.ToSingle(PopLine(lines));
            string parentName = GetParentName(parts[1]);
            IJoint parentJoint = joints[parentName];
            parentJoint.AddElement(element);
        }

        private string GetParentName(string name)
        {
            if (!name.Contains('.'))
            {
                return ".";
            }
            else
            {
                return name.Substring(0, name.LastIndexOf("."));
            }
        }

        private void AddElementPoints(IElement element, string line)
        {
            string[] points = line.Split(' ');

            foreach(string point in points)
            {
                string[] values = point.Split(',');
                element.AddPoint(new PointF(Convert.ToSingle(values[0]), Convert.ToSingle(values[0])));
            }
        }

        private void AddElementColors(IElement element, string line)
        {
            string[] colors = line.Split(' ');

            foreach (string colorRef in colors)
            {
                element.AddColor(!colorRef.StartsWith("#") ? Color.FromName(colorRef) : ColorTranslator.FromHtml(colorRef));
            }
        }
        public string ApplySettings(string structure, IDictionary<string, string> settings)
        {
            if (settings == null)
            {
                return structure;
            }

            string[] parts = structure.Split('@');

            for (int i = 1; i < parts.Length; i += 2)
            {
                if (string.IsNullOrEmpty(parts[i]))
                {
                    parts[i] = "@"; // @@ produces an @-sign
                }
                else
                {
                    if (settings.ContainsKey(parts[i]))
                    {
                        parts[i] = settings[parts[i]];
                    }
                    // No else clause - the name doubles as the default value
                }
            }

            return string.Join(string.Empty, parts);
        }
    }
}
