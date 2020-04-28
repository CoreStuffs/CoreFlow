using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreFlow.Activities.Communication.REST
{
    class RestExtension : IWorkflowInstanceExtension
    {
        private WorkflowInstanceProxy _instance;
        public RestExtension()
        {

        }
        public String CallApi(String Url)
        {
            WebProxy proxy = new WebProxy(WebRequest.GetSystemWebProxy().GetProxy(new Uri(Url))); //new WebProxy();
            //proxy.Address = new Uri("http://webproxy-dr.erbhlux.lan:8080");
            proxy.UseDefaultCredentials = true;
            proxy.BypassProxyOnLocal = true;


            WebClient wc = new WebClient();
            wc.Proxy = proxy;
            // Let's simulate a slow call
            Console.WriteLine("Calling {0} ...", Url);


            return wc.DownloadString(new Uri(Url));



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
