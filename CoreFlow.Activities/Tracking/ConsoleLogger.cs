using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoreFlow.Activities.Tracking
{
    public class ConsoleLogger : ILogWorkflowEvents
    {
        public void InsertEvent(WorkflowRecord EventRecord)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Log : " + EventRecord.ToString('\t'));
            Console.ForegroundColor = currentColor;
        }

        public void UpdateEvent(WorkflowRecord EventRecord)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Log : " + EventRecord.ToString('\t').TrimEnd('\n').Split('\n').Last());
            Console.ForegroundColor = currentColor;
        }

        public void Dispose()
        {
        }
    }
}
