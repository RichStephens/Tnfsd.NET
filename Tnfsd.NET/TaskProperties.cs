using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tnfsd.NET
{
    public class TaskProperties
    {
        public string ExecutableFolder {  get; set; }
        public string ShareFolder { get; set; }
        public bool   RunWithHighestPrivilege { get; set; }
        public string UserId { get; set; }

    }
}
