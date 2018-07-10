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
        [ActionFilterDnsAddress(DnsAddressFilterAction.Allow)]
        public IActionResult DnsAddressDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-contains")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.AllowContains)]
        public IActionResult DnsAddressDynamicAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-regex")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.AllowRegEx)]
        public IActionResult DnsAddressDynamicAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.Deny)]
        public IActionResult DnsAddressDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-contains")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.DenyContains)]
        public IActionResult DnsAddressDynamicDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-regex")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DnsAddressDynamicDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [ActionFilterDnsAddress("ochap.local", DnsAddressFilterAction.Allow)]
        public IActionResult DnsAddressStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-contains")]
        [ActionFilterDnsAddress(".local", DnsAddressFilterAction.AllowContains)]
        public IActionResult DnsAddressStaticAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-regex")]
        [ActionFilterDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.AllowRegEx)]
        public IActionResult DnsAddressStaticAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [ActionFilterDnsAddress("ochap.local", DnsAddressFilterAction.Deny)]
        public IActionResult DnsAddressStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-contains")]
        [ActionFilterDnsAddress(".local", DnsAddressFilterAction.DenyContains)]
        public IActionResult DnsAddressStaticDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-regex")]
        [ActionFilterDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.DenyRegEx)]
        public IActionResult DnsAddressStaticDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }
    }
}