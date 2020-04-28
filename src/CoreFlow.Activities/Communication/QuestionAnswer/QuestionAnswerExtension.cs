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
                Console.WriteLine("Question added: " + Question + " [Bookmark: QuestionAnswerBookmark" + ActivityId+"]");
                Console.WriteLine("Now, navigate to https://localhost:5001/api/instance/{0}/QuestionAnswerBookmark{1}/John%20Smith", _instance.Id, ActivityId);

                //String answer = Console.ReadLine();
                //_instance.BeginResumeBookmark(new Bookmark("QuestionAnswerBookmark" + ActivityId), answer, (o) =>
                //{
                //    Console.WriteLine("End of ResumeBookmark");
                //}, null);

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
