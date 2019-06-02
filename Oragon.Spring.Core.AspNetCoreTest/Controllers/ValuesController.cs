using Microsoft.AspNetCore.Mvc;
using Oragon.Spring.Core.AspNetCoreTest.Service;
using System.Collections.Generic;

namespace Oragon.Spring.Core.AspNetCoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromServices] DummyService dummyService1, [FromServices] IDummyService dummyService2)
        {
            return new string[] {
                "value1",
                "value2",
                dummyService1.Ping(),
                dummyService2.Ping()
            };
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
