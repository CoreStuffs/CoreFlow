using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreFlow.Activities.Tracking
{


    public class WFTracking : TrackingParticipant
    {
        public string WorkflowName { get; set; }
        public long JobId { get; set; }
        public Guid WFIdentifier { get; set; }

        ILogWorkflowEvents logWorkflowEvents;

        public WFTracking(ILogWorkflowEvents logWorkflowEvents)
        {
            this.logWorkflowEvents = logWorkflowEvents;
            JobId = -1;
        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            WorkflowRecord message = new WorkflowRecord();

            message.WFId = WFIdentifier;
            message.JobId = JobId;

            message.EventId = (int)record.RecordNumber;
            message.EventTime = record.EventTime.ToLocalTime();
            message.RunId = record.InstanceId;

            WorkflowInstanceRecord wfRecord = record as WorkflowInstanceRecord;
            if (wfRecord != null)
            {
                message.State = wfRecord.State;
                message.Name = WorkflowName;

                if (wfRecord.State == WorkflowInstanceStates.Idle) return; //do not track idle status
            }

            ActivityStateRecord actRecord = record as ActivityStateRecord;
            if (actRecord != null)
            {
                message.State = actRecord.State;
                message.Name = actRecord.Activity.Name;
                if (actRecord.Activity.Name.Equals("DynamicActivity"))
                {
                    return;
                }

                if (actRecord.State == ActivityStates.Executing && actRecord.Activity.TypeName == "System.Activities.Statements.WriteLine")
                {
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.Write(actRecord.Arguments["Text"]);
                        message.Message = writer.ToString();
                        writer.Close();
                    }
                }
            }

            WorkflowInstanceUnhandledExceptionRecord exRecord = record as WorkflowInstanceUnhandledExceptionRecord;
            if (exRecord != null)
            {
                message.State = exRecord.State;
                message.Name = WorkflowName;
                message.Message = exRecord.UnhandledException.Message;
            }

            CustomTrackingRecord cuRecord = record as CustomTrackingRecord;
            if (cuRecord != null)
            {
                message.Name = cuRecord.Activity.Name;
                message.State = cuRecord.Name;
                message.Message = cuRecord.Data.ContainsKey("Message") ? cuRecord.Data["Message"] as string : null;
            }

            logWorkflowEvents.InsertEvent(message);
        }
    }

}
