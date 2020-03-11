﻿using CoreFlow.Activities.Tracking;
using System;
using System.Activities;
using System.Activities.Hosting;
using System.Activities.Runtime.DurableInstancing;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoreFlow.Activities
{
    public class RuntimeEngine
    {
        InstanceStore instanceStore;
        List<Object> extensions = new List<object>();

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
            return new WorkflowInstance[0];
        }


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
                    Console.WriteLine("Workflow Canceled.");
                }
                else
                {
                    if (e.Outputs != null && e.Outputs.Count > 0)
                    {
                        foreach (var kvp in e.Outputs)
                            Console.WriteLine("Output --> {0} : {1}", kvp.Key, kvp.Value.ToString());
                    }
                    Console.WriteLine("Success");
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
                Console.WriteLine("PersistableIdle");
                return PersistableIdleAction.Unload;
            };

            workflowApplication.Idle = delegate (WorkflowApplicationIdleEventArgs e)
            {

                Console.WriteLine("Idle");

            };

            return workflowApplication;
        }


    }
}
