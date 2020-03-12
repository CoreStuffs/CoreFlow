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

    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowDefinitionController : ControllerBase
    {
        RuntimeEngine runtime;
        public WorkflowDefinitionController(RuntimeEngine runtime)
        {
            this.runtime = runtime;
        }

        // GET: api/WorkflowDefinition
        [HttpGet]
        public IEnumerable<String> Get()
        {
            return new String[];
        }

        // GET: api/WorkflowDefinition/5
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

        // PUT: api/WorkflowDefinition/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/WorkflowDefinition/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
