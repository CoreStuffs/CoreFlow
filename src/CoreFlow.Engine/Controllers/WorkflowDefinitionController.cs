using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFlow.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Engine.Controllers
{
    // Maintain the database of Workflow Definitions

    [Route("api/model")]
    [ApiController]
    public class WorkflowModelController : ControllerBase
    {
        RuntimeEngine runtime;
        public WorkflowModelController(RuntimeEngine runtime)
        {
            this.runtime = runtime;
        }

        // GET: api/WorkflowModel
        [HttpGet]
        public IEnumerable<String> Get()
        {
            return new String[0];
        }

        // GET: api/WorkflowModel/5
        [HttpGet("{name}")]
        public string Get(String name)
        {
            return "value";
        }

        // POST: api/WorkflowInstance
        [HttpPost("{name}")]
        public void Post([FromBody] string value)
        {
        }

        // DELETE: api/WorkflowModel/5
        [HttpDelete("{name}")]
        public void Delete(String name)
        {
        }
    }
}
