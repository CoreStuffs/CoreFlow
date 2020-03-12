using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreFlow.Activities.Communication.REST
{
    public class CallRestApiActivity : NativeActivity<string>
    {
        public InArgument<String> Url { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddDefaultExtensionProvider<RestExtension>(() =>
                 new RestExtension());
            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
        {
            Console.WriteLine("Activity Id: " + Id);
            // Create a Bookmark and wait for it to be resumed.  
            Result.Set(context, context.GetExtension<RestExtension>().CallApi(Url.Get(context)));
        }

        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  
        protected override bool CanInduceIdle
        {
            get { return false; }
        }


        
    
    }
}
