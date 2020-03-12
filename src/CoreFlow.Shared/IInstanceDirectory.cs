using System;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Text;

namespace CoreFlow.Shared
{
    public interface IInstanceDirectory
    {
        IEnumerable<WorkflowInstance> GetInstances();
    }
}
