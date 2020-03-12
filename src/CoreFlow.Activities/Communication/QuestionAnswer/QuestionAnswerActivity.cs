using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreFlow.Activities.Communication.QuestionAnswer
{
    public class QuestionAnswerActivity : NativeActivity<string>
    {
        public InArgument<String> Question { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddDefaultExtensionProvider<QuestionAnswerExtension>(() =>
                 new QuestionAnswerExtension());
            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
        {
            Console.WriteLine("Activity Id: " + Id);
            // Create a Bookmark and wait for it to be resumed.  
            context.CreateBookmark("QuestionAnswerBookmark"+Id,
                new BookmarkCallback(OnResumeBookmark));
            context.GetExtension<QuestionAnswerExtension>().Question(Id, Question.Get(context));
        }

        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        private void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            // When the Bookmark is resumed, assign its value to  
            // the Result argument.  
            Result.Set(context, (string)obj);
        }
    
    }
}
