using Bhbk.Lib.Waf.IpAddress;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Bhbk.WebApi.Sample.Controllers
{
    [Route("ip-address")]
    public class IpAddressController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [IpAddress(IpAddressFilterAction.Allow)]
        public IActionResult IpAddressDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [IpAddress(IpAddressFilterAction.Deny)]
        public IActionResult IpAddressDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [IpAddress("::1", IpAddressFilterAction.Allow)]
        public IActionResult IpAddressStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [IpAddress("::1", IpAddressFilterAction.Deny)]
        public IActionResult IpAddressStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(IpAddressController)).GetName().Version.ToString());
        }
    }
}