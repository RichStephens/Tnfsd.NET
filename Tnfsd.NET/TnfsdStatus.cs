namespace Tnfsd.NET
{
    public record TnfsdStatus(
        bool TaskExists,
        bool IsRunning,
        bool FirewallRuleExists,
        bool ExecutableFound,
        string? ExecutableFolder,
        string? ShareFolder);
}
