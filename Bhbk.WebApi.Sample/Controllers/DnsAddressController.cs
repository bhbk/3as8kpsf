using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Bhbk.WebApi.Sample.Controllers
{
    [Route("dns-address")]
    public class DnsAddressController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [DnsAddress(DnsAddressFilterAction.Allow)]
        public IActionResult DnsAddressDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-contains")]
        [DnsAddress(DnsAddressFilterAction.AllowContains)]
        public IActionResult DnsAddressDynamicAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-regex")]
        [DnsAddress(DnsAddressFilterAction.AllowRegEx)]
        public IActionResult DnsAddressDynamicAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [DnsAddress(DnsAddressFilterAction.Deny)]
        public IActionResult DnsAddressDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-contains")]
        [DnsAddress(DnsAddressFilterAction.DenyContains)]
        public IActionResult DnsAddressDynamicDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-regex")]
        [DnsAddress(DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DnsAddressDynamicDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [DnsAddress("ochap.local", DnsAddressFilterAction.Allow)]
        public IActionResult DnsAddressStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-contains")]
        [DnsAddress(".local", DnsAddressFilterAction.AllowContains)]
        public IActionResult DnsAddressStaticAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-regex")]
        [DnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.AllowRegEx)]
        public IActionResult DnsAddressStaticAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [DnsAddress("ochap.local", DnsAddressFilterAction.Deny)]
        public IActionResult DnsAddressStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-contains")]
        [DnsAddress(".local", DnsAddressFilterAction.DenyContains)]
        public IActionResult DnsAddressStaticDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-regex")]
        [DnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DnsAddressStaticDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }
    }
}