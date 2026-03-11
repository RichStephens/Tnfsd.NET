using System;

namespace Tnfsd.NET
{
    public class TaskProperties
    {
        // These may legitimately be null when a task/action doesn't provide values.
        public string? ExecutableFolder { get; set; }
        public string? ShareFolder { get; set; }
    }
}
