using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiTestForConsul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly ILogger _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"WebApiTestForConsul001:{DateTime.Now.ToString()}  { Environment.MachineName + " OS:" + Environment.OSVersion.VersionString}";
        }

        /// <summary>
        /// http://localhost:1234/health
        /// </summary>
        /// <returns></returns>
        [HttpGet("/health")]
        public IActionResult Heathle()
        {
            return Ok();
        }

        /// <summary>
        /// http://localhost:1234/notice
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("/notice")]
        public IActionResult Notice()
        {
            var bytes = new byte[10240];
            var i = Request.Body.ReadAsync(bytes, 0, bytes.Length);
            var content = System.Text.Encoding.UTF8.GetString(bytes).Trim('\0');
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WebApiTestForConsul.Dto.NoticeError>>(content);
            SendEmail(result);
            return Ok();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="content"></param>
        private void SendEmail(List<WebApiTestForConsul.Dto.NoticeError> errors)
        {
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
            _logger.LogError("健康检查故障:" + content);
        }
    }
}
