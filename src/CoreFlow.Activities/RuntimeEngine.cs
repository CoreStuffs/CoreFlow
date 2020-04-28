using CoreFlow.Activities.Tracking;
using System;
using System.Xaml;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Hosting;
using System.Activities.Runtime.DurableInstancing;
using System.Activities.Statements;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreFlow.Shared;

namespace CoreFlow.Activities
{
    public class RuntimeEngine
    {
        InstanceStore instanceStore;
        List<Object> extensions = new List<object>();
        Guid lastid;
        public RuntimeEngine()
        {
        }

        public RuntimeEngine WithInstanceStore(InstanceStore instanceStore)
        {
            this.instanceStore = instanceStore;
            return this;
        }

        public RuntimeEngine AddExtension(Object singletonExtension)
        {
            this.extensions.Add(singletonExtension);
            return this;
        }

        public T GetExtension<T>()
        {
            return extensions.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<WorkflowInstance> GetInstances()
        {
            return GetExtension<IInstanceDirectory>()?.GetInstances();
        }


        public void ResumeBookmark(Guid instanceId, String bookmark, Object input)
        {
            var ii = WorkflowApplication.GetInstance(instanceId, instanceStore);
            var WorkflowDefinition = this.GetExtension<IWorkflowModelCatalog>()?.GetActiveModel("Process1");
            var app = new WorkflowApplication(WorkflowDefinition, ii.DefinitionIdentity);
            EnrichWorkflowApplication(app);
            app.Load(instanceId);
            app.ResumeBookmark(bookmark, input);
        }

        public Guid StartNewInstance(String modelName)
        {
            WorkflowIdentity workflowIdentity = new WorkflowIdentity("Process1", new Version(0, 1), "wf");
            var wf = this.GetExtension<IWorkflowModelCatalog>()?.GetActiveModel("Process1");
            var app = new WorkflowApplication(wf);
            EnrichWorkflowApplication(app);
            app.Run();
            lastid = app.Id;
            Console.WriteLine(lastid);
            return app.Id;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        private WorkflowApplication EnrichWorkflowApplication(WorkflowApplication workflowApplication)
        {

            var consoleLogger = new ConsoleLogger();
            WFTracking track = new WFTracking(consoleLogger);
            //Tracking Profile
            TrackingProfile prof = new TrackingProfile();
            prof.Name = "CustomTrackingProfile";
            prof.Queries.Add(new WorkflowInstanceQuery { States = { "*" } });
            prof.Queries.Add(new ActivityStateQuery { States = { "*" }, Arguments = { "*" } });
            prof.Queries.Add(new CustomTrackingQuery() { Name = "*", ActivityName = "*" });
            prof.ImplementationVisibility = ImplementationVisibility.RootScope;
            track.TrackingProfile = prof;
            track.WorkflowName = "wf";

            foreach (var extension in extensions)
            {
                workflowApplication.Extensions.Add(extension);
            }
            workflowApplication.Extensions.Add(track);
            workflowApplication.Extensions.Add(consoleLogger);

            workflowApplication.InstanceStore = instanceStore;

            workflowApplication.Completed = delegate (WorkflowApplicationCompletedEventArgs e)
            {
                if (e.CompletionState == ActivityInstanceState.Faulted)
                {
                    Console.WriteLine(string.Format("Workflow Terminated. Exception: {0}\r\n{1}",
                        e.TerminationException.GetType().FullName,
                        e.TerminationException.Message));
                }
                else if (e.CompletionState == ActivityInstanceState.Canceled)
                {
                    Console.WriteLine("Workflow Canceled : " + workflowApplication.Id);
                }
                else
                {
                    if (e.Outputs != null && e.Outputs.Count > 0)
                    {
                        foreach (var kvp in e.Outputs)
                            Console.WriteLine("Output --> {0} : {1}", kvp.Key, kvp.Value.ToString());
                    }
                    Console.WriteLine("Success : " + workflowApplication.Id);
                }
            };

            workflowApplication.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
            {
                Console.WriteLine(string.Format("Workflow Aborted. Exception: {0}\r\n{1}",
                        e.Reason.GetType().FullName,
                        e.Reason.Message));
            };

            workflowApplication.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
            {
                Console.WriteLine(string.Format("Unhandled Exception in {2}: {0}\r\n{1}",
                        e.UnhandledException.GetType().FullName,
                        e.UnhandledException.Message, e.ExceptionSource));
                return UnhandledExceptionAction.Terminate;
            };

            workflowApplication.PersistableIdle = delegate (WorkflowApplicationIdleEventArgs e)
            {
                Console.WriteLine("PersistableIdle : " + workflowApplication.Id);
                return PersistableIdleAction.Unload;
            };

            workflowApplication.Idle = delegate (WorkflowApplicationIdleEventArgs e)
            {

                Console.WriteLine("Idle : " + workflowApplication.Id);

            };

            return workflowApplication;
        }


    }
}
