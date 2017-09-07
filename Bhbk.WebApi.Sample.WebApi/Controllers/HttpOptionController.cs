using Bhbk.Lib.Env.Waf.HttpOption;
using System.Reflection;
using System.Web.Http;

namespace Bhbk.WebApi.Sample.WebApi.Controllers
{
    [RoutePrefix("http-option")]
    public class HttpOptionController : BaseController
    {
        [HttpGet]
        [Route("v1/ssl-required")]
        [ActionFilterHttpOption(HttpFilterAction.SslRequired)]
        //[AuthorizeHttpOption(HttpFilterAction.SslRequired)]
        public IHttpActionResult SslRequired()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/ssl-not-allowed")]
        [ActionFilterHttpOption(HttpFilterAction.SslNotAllowed)]
        //[AuthorizeHttpOption(HttpFilterAction.SslNotAllowed)]
        public IHttpActionResult SslNotAllowed()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/ssl-optional")]
        [ActionFilterHttpOption(HttpFilterAction.SslOptional)]
        //[AuthorizeHttpOption(HttpFilterAction.SslOptional)]
        public IHttpActionResult SslOptional()
        {
            return Ok(Assembly.GetAssembly(typeof(HttpOptionController)).GetName().Version.ToString());
        }
    }
}
