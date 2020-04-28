using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreFlow.Activities.Communication.QuestionAnswer
{
    class QuestionAnswerExtension : IWorkflowInstanceExtension
    {
        WorkflowInstanceProxy _instance;
        public Dictionary<String, String> Questions = new Dictionary<string, string>();

        public void Question(String ActivityId, String Question)
        {
            Task.Run(() =>
            {
                Questions.Add(ActivityId, Question);
                Console.WriteLine("Question added: " + Question);
                String answer = Console.ReadLine();
                _instance.BeginResumeBookmark(new Bookmark("QuestionAnswerBookmark" + ActivityId), answer, (o) => { Console.WriteLine("End of ResumeBookmark"); }, null);

            });


        }

        public IEnumerable<object> GetAdditionalExtensions()
        {
            return null;
        }

        public void SetInstance(WorkflowInstanceProxy instance)
        {
            _instance = instance;
        }
    }

}
