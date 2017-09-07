using Bhbk.Lib.Env.Waf.IpAddress;
using System.Reflection;
using System.Web.Http;

namespace Bhbk.WebApi.Sample.WebApi.Controllers
{
    [RoutePrefix("ip-address")]
    public class IpAddressController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [ActionFilterIpAddress(IpAddressFilterAction.Allow)]
        //[AuthorizeIpAddress(IpAddressFilterAction.Allow)]
        public IHttpActionResult IpAddressDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [ActionFilterIpAddress(IpAddressFilterAction.Deny)]
        //[AuthorizeIpAddress(IpAddressFilterAction.Deny)]
        public IHttpActionResult IpAddressDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [ActionFilterIpAddress("::1", IpAddressFilterAction.Allow)]
        //[AuthorizeIpAddress("::1", IpAddressFilterAction.Allow)]
        public IHttpActionResult IpAddressStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [ActionFilterIpAddress("::1", IpAddressFilterAction.Deny)]
        //[AuthorizeIpAddress("::1", IpAddressFilterAction.Deny)]
        public IHttpActionResult IpAddressStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }
    }
}
