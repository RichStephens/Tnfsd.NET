using System;

namespace Tnfsd.NET
{
    public static class FirewallManager
    {
        public static void AddFirewallException(string exePath, string ruleName)
        {
            Type fwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            dynamic fwMgr = Activator.CreateInstance(fwMgrType);

            dynamic app = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));
            app.Name = ruleName;
            app.ProcessImageFileName = exePath;
            app.Enabled = true;
            app.Scope = 0; // NET_FW_SCOPE_ALL
            app.IpVersion = 2; // NET_FW_IP_VERSION_ANY

            // Add to authorized apps if not already present
            var apps = fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications;
            try
            {
                apps.Add(app);
            }
            catch
            {
                // If already exists, ignore
            }
        }

        public static void RemoveFirewallException(string exePath)
        {
            Type fwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            dynamic fwMgr = Activator.CreateInstance(fwMgrType);
            var apps = fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications;

            try
            {
                apps.Remove(exePath);
            }
            catch
            {
                // Ignore if not found
            }
        }

        public static bool FirewallExceptionExists(string ruleName)
        {
            Type fwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            dynamic fwMgr = Activator.CreateInstance(fwMgrType);
            var apps = fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications;

            foreach (dynamic app in apps)
            {
                if (app.Name != null &&
                    app.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

