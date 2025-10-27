using System;
using System.Diagnostics;
using System.IO;

namespace Tnfsd.NET
{
    public static class FirewallManager
    {
        public static void AddFirewallException(string exePath, string displayName)
        {
            if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
                throw new FileNotFoundException("Executable not found for firewall exception.");

            // Don't do anything if the firewall rule already exists.
            if (VerifyFirewallRuleExists(exePath))
            {
                return;
            }

            RunNetshCommand($"advfirewall firewall add rule name=\"{displayName}\" dir=in action=allow program=\"{exePath}\" enable=yes profile=private,public");

            if (VerifyFirewallRuleExists(exePath))
            {
                System.Windows.Forms.MessageBox.Show($"Firewall rule successfully added for {Path.GetFileName(exePath)}.", "Firewall Rule Added", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show($"Warning: Unable to verify the firewall rule for {Path.GetFileName(exePath)}.", "Verification Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public static void RemoveFirewallException(string exePath)
        {
            if (string.IsNullOrWhiteSpace(exePath)) return;

            // Don't do anything if the firewall rule is doesn't exist
            if (!VerifyFirewallRuleExists(exePath))
            {
                return;
            }

            RunNetshCommand($"advfirewall firewall delete rule program=\"{exePath}\"");

            if (!VerifyFirewallRuleExists(exePath))
            {
                System.Windows.Forms.MessageBox.Show($"Firewall rule successfully removed for {Path.GetFileName(exePath)}.", "Firewall Rule Removed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show($"Warning: Firewall rule may still exist for {Path.GetFileName(exePath)}.", "Verification Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private static bool VerifyFirewallRuleExists(string exePath)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = $"advfirewall firewall show rule name=all | findstr /I \"{exePath}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output.Contains(exePath, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static void RunNetshCommand(string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = "netsh";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
