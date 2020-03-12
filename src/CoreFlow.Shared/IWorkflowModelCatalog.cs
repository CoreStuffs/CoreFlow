using Microsoft.Extensions.Configuration;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Text;


namespace CoreFlow.Shared
{
    public interface IWorkflowModelCatalog
    {
        void Initialize(IConfiguration configuration);
        WorkflowIdentity RegisterModel(String name, Activity workflowModel);
        Activity GetModel(WorkflowIdentity workflowIdentity);
        Activity GetActiveModel(String name);
    }
}
