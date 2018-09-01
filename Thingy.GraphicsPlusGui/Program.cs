using System;
using System.Windows.Forms;
using Thingy.Infrastructure;

namespace Thingy.GraphicsPlusGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InfrastructureConfiguration.ConventionBasedInstallerNamespacePrefixes.Add("Thingy");
            Application.Run(Bootstrapper.Container.Resolve<IMainForm>().MainForm);
        }
    }
}
