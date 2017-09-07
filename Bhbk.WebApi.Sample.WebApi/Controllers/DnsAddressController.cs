using Bhbk.Lib.Env.Waf.DnsAddress;
using System.Reflection;
using System.Web.Http;

namespace Bhbk.WebApi.Sample.WebApi.Controllers
{
    [RoutePrefix("dns-address")]
    public class DnsAddressController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.Allow)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.Allow)]
        public IHttpActionResult DnsAddressDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-contains")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.AllowContains)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.AllowContains)]
        public IHttpActionResult DnsAddressDynamicAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-allow-regex")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.AllowRegEx)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.AllowRegEx)]
        public IHttpActionResult DnsAddressDynamicAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.Deny)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.Deny)]
        public IHttpActionResult DnsAddressDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-contains")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.DenyContains)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.DenyContains)]
        public IHttpActionResult DnsAddressDynamicDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny-regex")]
        [ActionFilterDnsAddress(DnsAddressFilterAction.DenyRegEx)]
        //[AuthorizeDnsAddress(DnsAddressFilterAction.DenyRegEx)]
        public IHttpActionResult DnsAddressDynamicDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [ActionFilterDnsAddress("ochap.local", DnsAddressFilterAction.Allow)]
        //[AuthorizeDnsAddress("ochap.local", DnsAddressFilterAction.Allow)]
        public IHttpActionResult DnsAddressStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-contains")]
        [ActionFilterDnsAddress(".local", DnsAddressFilterAction.AllowContains)]
        //[AuthorizeDnsAddress(".local", DnsAddressFilterAction.AllowContains)]
        public IHttpActionResult DnsAddressStaticAllowContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow-regex")]
        [ActionFilterDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.AllowRegEx)]
        //[AuthorizeDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.AllowRegEx)]
        public IHttpActionResult DnsAddressStaticAllowRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [ActionFilterDnsAddress("ochap.local", DnsAddressFilterAction.Deny)]
        //[AuthorizeDnsAddress("ochap.local", DnsAddressFilterAction.Deny)]
        public IHttpActionResult DnsAddressStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-contains")]
        [ActionFilterDnsAddress(".local", DnsAddressFilterAction.DenyContains)]
        //[AuthorizeDnsAddress(".local", DnsAddressFilterAction.DenyContains)]
        public IHttpActionResult DnsAddressStaticDenyContains()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny-regex")]
        [ActionFilterDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.DenyRegEx)]
        //[AuthorizeDnsAddress(@"[a-zA-Z0-9]*\.local$", DnsAddressFilterAction.DenyRegEx)]
        public IHttpActionResult DnsAddressStaticDenyRegEx()
        {
            return Ok(Assembly.GetAssembly(typeof(DnsAddressController)).GetName().Version.ToString());
        }
    }
}
