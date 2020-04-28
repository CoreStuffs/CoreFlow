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
    [Route("api/instance")]
    [ApiController]
    public class WorkflowInstanceController : ControllerBase
    {
        RuntimeEngine runtime;
        public WorkflowInstanceController(RuntimeEngine runtime)
        {
            this.runtime = runtime;
        }

        // GET: api/instance
        [HttpGet]
        public IActionResult Get()
        {
            runtime.StartNewInstance("Process1");

            return Ok(runtime.GetInstances());
        }

        // GET: api/instance/5
        [HttpGet("{id}/{bookmark}/{data}")]
        public IActionResult Get(Guid id, String bookmark, String data)
        {
            runtime.ResumeBookmark(id, bookmark, data);
            return Ok();
        }

        // POST: api/instance
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/instance/5
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
