using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace Tnfsd.NET
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Block startup unless running as Administrator
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    MessageBox.Show(
                        "This application must be run as Administrator.\n\n" +
                        "Please close and restart it using 'Run as administrator'.",
                        "Administrator Privileges Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return; // abort startup
                }
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
