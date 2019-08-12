using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFactTest.Dependency;
using AutoFactTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoFactTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;
        private ITest _testService;
        private IConfiguration _configService;
        private readonly ContentList _options;

        public ValuesController(ITest testService, ILogger<ValuesController> logger,
            IConfiguration configuration, IOptions<ContentList> options)
        {
            _testService = testService;
            _configService = configuration;
            _options = options.Value;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //IConfiguration
            var a1 = _configService.GetSection("ContentList").GetValue<int>("Id",0);//默认值
            var a2 = _configService.GetSection("ContentList").GetValue<string>("Title");
            var a3 = _configService.GetSection("ContentList").GetChildren().First();//获取全部项目
            var a4 = _configService["ContentList:Id"];//都是返回字符串

            //IOptions
            var b = _options;

            return new string[] { _testService.GetTestName() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //反模式，最好使用构建函数注入
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
