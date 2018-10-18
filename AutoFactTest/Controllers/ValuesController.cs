using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFactTest.Dependency;
using Microsoft.AspNetCore.Mvc;

namespace AutoFactTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ITest _testService;
        public ValuesController(ITest testService)
        {
            _testService = testService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { _testService.GetTestName() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return IoC.Resolve<ITest>("Test1").GetTestName() + " " + IoC.Resolve<ITest>("Test2").GetTestName();
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
