using Bhbk.Lib.Waf.HttpOption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Bhbk.WebApi.Sample.Controllers
{
    [Route("http-option")]
    public class HttpOptionController : BaseController
    {
        [HttpGet]
        [Route("v1/ssl-required")]
        [HttpOption(HttpFilterAction.SslRequired)]
        public IActionResult SslRequired()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/ssl-not-allowed")]
        [HttpOption(HttpFilterAction.SslNotAllowed)]
        public IActionResult SslNotAllowed()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/ssl-optional")]
        [HttpOption(HttpFilterAction.SslOptional)]
        public IActionResult SslOptional()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }
    }
}