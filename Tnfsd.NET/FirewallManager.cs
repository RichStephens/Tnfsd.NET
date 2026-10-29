using System;
using System.Runtime.InteropServices;

namespace Tnfsd.NET
{
    public static class FirewallManager
    {
        private const int NET_FW_RULE_DIR_IN = 1;
        private const int NET_FW_ACTION_ALLOW = 1;
        private const int NET_FW_PROFILE2_ALL = 0x7FFFFFFF;

        public static void AddFirewallException(string exePath, string ruleName)
        {
            RemoveFirewallException(ruleName);

            dynamic rule = CreateComObject("HNetCfg.FWRule");
            rule.Name = ruleName;
            rule.ApplicationName = exePath;
            rule.Direction = NET_FW_RULE_DIR_IN;
            rule.Action = NET_FW_ACTION_ALLOW;
            rule.Profiles = NET_FW_PROFILE2_ALL;
            rule.Enabled = true;

            CreateComObject("HNetCfg.FwPolicy2").Rules.Add(rule);
        }

        // Older versions created one rule per protocol and profile, all with the same name.
        public static bool RemoveFirewallException(string ruleName)
        {
            dynamic rules = CreateComObject("HNetCfg.FwPolicy2").Rules;
            bool removed = false;

            while (RuleExists(rules, ruleName))
            {
                int before = rules.Count;
                rules.Remove(ruleName);
                if (rules.Count == before)
                    break;
                removed = true;
            }

            return removed;
        }

        public static bool FirewallExceptionExists(string ruleName)
        {
            return RuleExists(CreateComObject("HNetCfg.FwPolicy2").Rules, ruleName);
        }

        private static bool RuleExists(dynamic rules, string ruleName)
        {
            try
            {
                rules.Item(ruleName);
                return true;
            }
            // A missing rule raises 0x80070002, which .NET maps to FileNotFoundException.
            catch (Exception ex) when (ex is FileNotFoundException or COMException)
            {
                return false;
            }
        }

        private static dynamic CreateComObject(string progId)
        {
            return Activator.CreateInstance(Type.GetTypeFromProgID(progId, throwOnError: true)!)!;
        }
    }
}
