using System;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFlow.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowInstanceController : ControllerBase
    {
        RuntimeEngine runtime;
        public WorkflowInstanceController(RuntimeEngine runtime)
        {
            this.runtime = runtime;
        }

        // GET: api/WorkflowInstance
        [HttpGet]
        public IEnumerable<String> Get()
        {
            return runtime.GetInstances();
        }

        // GET: api/WorkflowInstance/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WorkflowInstance
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/WorkflowInstance/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
