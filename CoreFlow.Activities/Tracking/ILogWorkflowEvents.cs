using System;
using System.Collections.Generic;
using System.Text;

namespace CoreFlow.Activities.Tracking
{
    public interface ILogWorkflowEvents : IDisposable
    {
        void InsertEvent(WorkflowRecord EventRecord);
        void UpdateEvent(WorkflowRecord EventRecord);
    }

    [Serializable]
    public class WorkflowRecord
    {
        public long JobId { get; set; } //IDSCHEDULE
        public Guid WFId { get; set; } //IDWORKFLOW
        public Guid RunId { get; set; } //IDINSTANCE
        public long EventId { get; set; } //EVENTINDEX
        public DateTime EventTime { get; set; } //EVENTTIME
        public string Name { get; set; } //EVENTSOURCE
        public string State { get; set; } //EVENTSTATUS
        public string Message { get; set; } //EVENTMESSAGE

        public WorkflowRecord()
        {
            JobId = -1;
            RunId = Guid.Empty;
            WFId = Guid.Empty;
            EventId = -1;
            EventTime = DateTime.MinValue;
            Name = string.Empty;
            State = string.Empty;
        }

        public void Print()
        {
            Console.WriteLine("-------------");
            Console.WriteLine("{0}", EventTime);
            Console.WriteLine("{0}", Name);
            Console.WriteLine("{0}", State);
            if (Message.Length > 0) Console.WriteLine("{0}", Message);
            Console.WriteLine("-------------");
        }

        public override string ToString()
        {
            return string.Format("SchedId: {0}\nWF ID: {1}\nRun ID: {2}\nEventTime: {3}\nEvent Source: {4}\nEvent Status: {5}\nEvent Message: {6}", JobId, WFId, RunId, EventTime, Name, State, Message);
        }

        public string ToString(char p)
        {
            return String.Format("{0}{4}{1}{4}{2}{4}{3}", EventTime, Name, State, Message, p);
        }
    }
}
