using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Contracts.Jobs
{
    public sealed class JobRequest
    {
        public string Name { get; set; }

        public string UrlCheck { get; set; }
    }
}
