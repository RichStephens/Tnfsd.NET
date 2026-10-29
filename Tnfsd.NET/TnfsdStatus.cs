namespace Tnfsd.NET
{
    public record TnfsdStatus(
        bool TaskExists,
        bool IsRunning,
        bool FirewallRuleExists,
        string? ExecutableFolder,
        string? ShareFolder);
}
