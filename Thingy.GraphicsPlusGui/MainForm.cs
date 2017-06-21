using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Thingy.GraphicsPlus;
using Thingy.GraphicsPlus.SmdConverter;

namespace Thingy.GraphicsPlusGui
{
    public partial class MainForm : Form, IMainForm
    {
        private readonly string partDirectory;
        private readonly IGraphicsPlus graphicsPlus;
        private readonly IStructureLoader loader;
        private readonly IStructureProvider structureProvider;
        private readonly IFileConverter smdConverter;

        private IJoint worldRoot = null;
        private IDictionary<string, string> structures = new Dictionary<string, string>();
        private IDictionary<string, string> settings = new Dictionary<string, string>();

        private readonly char[] kvpSeparators = new char[] { '=' };
        private readonly char[] settingsDelimiters = new char[] { '@' };
        private readonly Regex numbersRegex = new Regex("(?:\\+|-\\d|-\\.|\\d|\\d\\.|\\.\\d)+");

        Form IMainForm.MainForm
        {
            get
            {
                return this;
            }
        }

        public MainForm(string partDirectory, IGraphicsPlus graphicsPlus, IStructureLoader loader, IStructureProvider structureProvider, IFileConverter smdConverter)
        {
            this.partDirectory = partDirectory;
            Directory.SetCurrentDirectory(partDirectory);
            this.graphicsPlus = graphicsPlus;
            this.loader = loader;
            this.structureProvider = structureProvider;
            this.smdConverter = smdConverter;
            ////string structure = loader.Load("c:\\scratch\\temp.str");
            ////IDictionary<string, string> settings = new Dictionary<string, string>();
            ////settings["configured_color"] = "Yellow";
            ////IDictionary<string, string> structures = new Dictionary<string, string>();
            ////structures["head_joint"] = loader.Load("c:\\scratch\\temphead.str");
            ////worldRoot = structureProvider.Create(structure, settings, structures);
            InitializeComponent();
            Application.Idle += Application_Idle;
            RefreshFileDirectory();
            RefreshDictionaryView(structures, StructuresListBox);
            RefreshDictionaryView(settings, SettingsListBox);
            RefreshObjectTree();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            ////EnabledDisableControl(CreateButton, !string.IsNullOrEmpty(StructureTextBox.Text) && (worldRoot == null || (ObjectTreeView.SelectedNode != null && (ObjectTreeView.SelectedNode.Tag as IJoint) != null)));
            EnabledDisableControl(AddSettingsButton, !string.IsNullOrEmpty(StructureTextBox.Text));
            EnabledDisableControl(RenameButton, StructuresListBox.SelectedItem != null && !StructuresListBox.SelectedItem.ToString().StartsWith(string.Format("{0}=", StructureNameTextBox.Text)));
            EnabledDisableControl(CopyButton, StructuresListBox.SelectedItem != null && !StructuresListBox.SelectedItem.ToString().StartsWith(string.Format("{0}=", StructureNameTextBox.Text)));
            EnabledDisableControl(SetValueButton, !string.IsNullOrEmpty(KeyTextBox.Text));
            EnabledDisableControl(RemoveButton, ObjectTreeView.SelectedNode != null && (ObjectTreeView.SelectedNode.Tag as IJoint) != null);
        }

        private void EnabledDisableControl(Control control, bool enabled)
        {
            if (enabled != control.Enabled)
            {
                control.Enabled = enabled;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ////using (Image image = graphicsPlus.CreateImage(e.Graphics, e.ClipRectangle.Size))
            ////using (Graphics graphics = graphicsPlus.CreateGraphics(image))
            ////{
            ////    //DoDrawingOperations(graphics, e.ClipRectangle);
            ////    worldRoot.Draw(graphics);
            ////    graphicsPlus.Render(image, e.Graphics, e.ClipRectangle);
            ////}
        }

        ////private void DoDrawingOperations(Graphics graphics, RectangleF clipRect)
        ////{
        ////    RectangleF[] clipRects = { clipRect };
        ////    graphics.FillRectangles(Brushes.White, clipRects);
        ////    Pen pen = new Pen(Color.SkyBlue, 1);
            
        ////    Random rng = new Random();

        ////    for (int i = 0; i < 100; i++)
        ////    {
        ////        float left = Convert.ToSingle(clipRect.Width * rng.NextDouble());
        ////        float top = Convert.ToSingle(clipRect.Height * rng.NextDouble());
        ////        float width = Convert.ToSingle((clipRect.Width - left) * rng.NextDouble());
        ////        float height = Convert.ToSingle((clipRect.Height - top) * rng.NextDouble());
        ////        graphics.DrawEllipse(pen, left, top, width, height);
        ////        graphics.DrawLine(pen, RandomPoint(clipRect, rng), RandomPoint(clipRect, rng));
        ////    }

        ////    graphics.DrawRectangles(new Pen(Color.DarkRed, 11), clipRects);
        ////    graphics.DrawLine(new Pen(Color.DarkBlue, 13), 0.0f, 0.0f, clipRect.Width, clipRect.Height);
        ////    graphics.DrawLine(new Pen(Color.DarkGreen, 7), clipRect.Width, 0.0f, 0.0f, clipRect.Height);
        ////}

        ////private static PointF RandomPoint(RectangleF clipRect, Random rng)
        ////{
        ////    return new PointF(
        ////        Convert.ToSingle(clipRect.Width * rng.NextDouble()), 
        ////        Convert.ToSingle(clipRect.Height * rng.NextDouble())
        ////        );
        ////}

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ResizePreviewOption_Click(object sender, EventArgs e)
        {
            string[] dimensions = ((RadioButton)sender).Text.Split('x');
            this.Size = new Size(this.Size.Width - PreviewTabPage.Size.Width + Convert.ToInt32(dimensions[0]), 
                                 this.Size.Height - PreviewTabPage.Size.Height + Convert.ToInt32(dimensions[1]));
            PreviewTabPage.Size = new Size(Convert.ToInt32(dimensions[0]), Convert.ToInt32(dimensions[1]));
        }

        private void tabPage2_Paint(object sender, PaintEventArgs e)
        {
            if (worldRoot != null)
            {
                using (Image image = graphicsPlus.CreateImage(e.Graphics, e.ClipRectangle.Size))
                using (Graphics graphics = graphicsPlus.CreateGraphics(image))
                {
                    //DoDrawingOperations(graphics, e.ClipRectangle);
                    worldRoot.Draw(graphics);
                    graphicsPlus.Render(image, e.Graphics, e.ClipRectangle);
                }
            }
        }

        private void FileDirectoryRefreshButton_Click(object sender, EventArgs e)
        {
            RefreshFileDirectory();
        }

        private void RefreshFileDirectory()
        {
            FilesListBox.Items.Clear();

            foreach (string fileName in Directory.GetFiles(partDirectory, "*.str", SearchOption.TopDirectoryOnly))
            {
                FilesListBox.Items.Add(fileName);
            }
        }

        private void FilesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedItem != null)
            {
                string fileName = FilesListBox.SelectedItem.ToString();
                string name = Path.GetFileNameWithoutExtension(fileName);
                try
                {
                    structures[name] = loader.Load(fileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);

                    return;
                }

                RefreshDictionaryView(structures, StructuresListBox);
            }
        }

        private void RefreshDictionaryView(IDictionary<string, string> dictionary, ListBox view)
        {
            view.Items.Clear();

            foreach (string key in dictionary.Keys)
            {
                view.Items.Add(string.Format("{0}={1}", key, dictionary[key].ToString()));
            }
        }

        private void RefreshObjectTree()
        {
            ObjectTreeView.Nodes.Clear();

            if (worldRoot != null)
            {
                RefreshObjectTreeNode(worldRoot, ObjectTreeView.Nodes);
            }
        }

        private void RefreshObjectTreeNode(IJoint joint, TreeNodeCollection nodes)
        {
            TreeNode node = new TreeNode(joint.ToString());
            node.Tag = joint;
            AddLocationTreeNode(node, joint.Location);
            AddRotationNode(node, joint.Rotation);
            AddZIndexNode(node, joint.ZIndex);
            TreeNode elementsNode = new TreeNode("elements");

            foreach (IElement childElement in joint.Elements)
            {
                TreeNode elementNode = new TreeNode(childElement.ToString());
                AddPointsNode(elementNode, childElement.Points);
                AddColorsNode(elementNode, childElement.Colors);
                AddZIndexNode(elementNode, childElement.ZIndex);
                elementNode.Tag = childElement;
                elementsNode.Nodes.Add(elementNode);
            }

            node.Nodes.Add(elementsNode);
            TreeNode jointsNode = new TreeNode("joints");

            foreach (IJoint childJoint in joint.Joints)
            {
                RefreshObjectTreeNode(childJoint, jointsNode.Nodes);
            }

            node.Nodes.Add(jointsNode);
            nodes.Add(node);
        }

        private void AddPointsNode(TreeNode node, PointF[] points)
        {
            StringBuilder pointsBuilder = new StringBuilder("Points : {");
            
            foreach(PointF point in points)
            {
                pointsBuilder.Append(' ');
                pointsBuilder.Append(point.ToString());
            }

            pointsBuilder.Append(" }");

            node.Nodes.Add(pointsBuilder.ToString());
        }

        private void AddColorsNode(TreeNode node, Color[] colors)
        {
            StringBuilder colorsBuilder = new StringBuilder("Colors : {");

            foreach (Color color in colors)
            {
                colorsBuilder.Append(' ');
                colorsBuilder.Append(color.ToString());
            }

            colorsBuilder.Append(" }");

            node.Nodes.Add(colorsBuilder.ToString());
        }

        private void AddLocationTreeNode(TreeNode node, PointF location)
        {
            node.Nodes.Add(string.Format("Location : {0}", location.ToString()));
        }

        private void AddRotationNode(TreeNode node, float rotation)
        {
            node.Nodes.Add(string.Format("Rotation : {0}", rotation));
        }

        private void AddZIndexNode(TreeNode node, float zIndex)
        {
            node.Nodes.Add(string.Format("ZIndex : {0}", zIndex));
        }

        private void StructuresListBox_Click(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItem != null)
            {
                string[] kvp = listBox.SelectedItem.ToString().Split(kvpSeparators);
                StructureNameTextBox.Text = kvp[0];
                StructureTextBox.Text = kvp[1];
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            string[] kvp = StructuresListBox.SelectedItem.ToString().Split(kvpSeparators);
            structures.Remove(kvp[0]);
            structures[StructureNameTextBox.Text] = StructureTextBox.Text;
            RefreshDictionaryView(structures, StructuresListBox);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            structures[StructureNameTextBox.Text] = StructureTextBox.Text;
            RefreshDictionaryView(structures, StructuresListBox);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            IJoint joint;

            try
            {
                joint = structureProvider.Create(StructureTextBox.Text, settings, structures);                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

                return;
            }

            if(worldRoot == null)
            {
                worldRoot = joint;
            }
            else
            {
                ((IJoint)ObjectTreeView.SelectedNode.Tag).AddJoint(joint);
            }

            RefreshObjectTree();
        }

        private void SetValueButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(KeyTextBox.Text))
            {
                settings[KeyTextBox.Text] = ValueTextBox.Text;
                KeyTextBox.Clear();
                ValueTextBox.Clear();
                RefreshDictionaryView(settings, SettingsListBox);
            }
        }

        private void AddSettingsButton_Click(object sender, EventArgs e)
        {
            if (StructuresListBox != null && StructuresListBox.SelectedItem != null)
            {
                string[] settingNames = StructuresListBox.SelectedItem.ToString().Split(settingsDelimiters);

                for (int i = 1; i < settingNames.Length; i += 2)
                {
                    if (!settings.ContainsKey(settingNames[i]))
                    {
                        settings[settingNames[i]] = "";
                    }
                }

                RefreshDictionaryView(settings, SettingsListBox);
            }
        }

        private void SettingsListBox_Click(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItem != null)
            {
                string[] kvp = listBox.SelectedItem.ToString().Split(kvpSeparators);
                KeyTextBox.Text = kvp[0];
                ValueTextBox.Text = kvp.Length > 1 ? kvp[1] : "";
            }
        }

        private void ObjectTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Text.StartsWith("Location :") || e.Node.Text.StartsWith("Rotation :"))
            {
                e.Node.Tag = e.Node.Text;
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void ObjectTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                try
                {
                    IList<float> values = new List<float>();

                    foreach (Match match in numbersRegex.Matches(e.Label))
                    {
                        values.Add(Convert.ToSingle(match.Value));
                    }

                    if (e.Node.Tag.ToString().StartsWith("Location"))
                    {
                        PointF location = new PointF(values[0], values[1]);
                        ((IJoint)e.Node.Parent.Tag).Location = location;
                        e.Node.Text = string.Format("Location : {0}", location.ToString());
                    }

                    if (e.Node.Tag.ToString().StartsWith("Rotation"))
                    {
                        ((IJoint)e.Node.Parent.Tag).Rotation = values[0];
                        e.Node.Text = string.Format("Rotation : {0}", values[0].ToString());
                    }
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

            e.CancelEdit = true;
            e.Node.Tag = null;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = ObjectTreeView.SelectedNode.Parent;

            while (node != null && node.Tag == null)
            {
                node = node.Parent;
            }

            if (node == null)
            {
                worldRoot = null;
            }
            else
            {
                ((IJoint)node.Tag).RemoveJoint((IJoint)ObjectTreeView.SelectedNode.Tag);
            }

            RefreshObjectTree();
        }

        private void OenFolderButton_Click(object sender, EventArgs e)
        {

        }

        private void SmdButton_Click(object sender, EventArgs e)
        {
            if (OpenSmdFileDialog.ShowDialog() == DialogResult.OK)
            {
                smdConverter.Convert(OpenSmdFileDialog.FileName);
            }
        }
    }
}
