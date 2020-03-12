using Microsoft.Extensions.Configuration;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Text;

namespace CoreFlow.Activities.WorkflowModelCatalog
{
    public class FileWorkflowModelCatalog : CoreFlow.Shared.IWorkflowModelCatalog
    {


        public Activity GetActiveModel(string name)
        {
            Activity root = System.Activities.XamlIntegration.ActivityXamlServices.Load(System.IO.Path.Combine(@"..\CoreFlow.Activities\WorkflowModelCatalog\", String.Format("wf-{0}.xaml", name)));
            return root;
        }

        public Activity GetModel(WorkflowIdentity workflowIdentity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IConfiguration configuration)
        {

        }

        public WorkflowIdentity RegisterModel(string name, Activity workflowModel)
        {
            throw new NotImplementedException();
        }
    }
}
