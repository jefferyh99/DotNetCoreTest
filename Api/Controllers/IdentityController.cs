using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //当有权限时，返回客户的信息，这些信息主要是翻译JWT里面的Payload获得的
            var list = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return new JsonResult(list);
        }
    }
}
