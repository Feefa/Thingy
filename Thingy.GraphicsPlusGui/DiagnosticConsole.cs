using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thingy.Diagnostics.Api;

namespace Thingy.GraphicsPlusGui
{
    public partial class DiagnosticConsole : Form, IDiagnosticConsole
    {
        public DiagnosticConsole()
        {
            InitializeComponent();
        }

        public void WriteMessage(object source, string method, DiagnosticLevels level, string message)
        {
            textBox1.Text += message;
        }
    }
}
