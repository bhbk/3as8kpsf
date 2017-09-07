using Bhbk.Lib.Env.Waf.DnsAddress;
using Bhbk.Lib.Env.Waf.IpAddress;
using Bhbk.Lib.Env.Waf.HttpOption;
using System.Web.Http;

namespace Bhbk.WebApi.Sample.WebApi.Controllers
{
    [AllowAnonymous]
    //[Authorize]
    //[ActionFilterDnsAddress(DnsAddressFilterAction.Deny)]
    //[AuthorizeDnsAddress(DnsAddressFilterAction.Deny)]
    //[ActionFilterHttpOption(HttpFilterAction.SslRequired)]
    //[AuthorizeHttpOption(HttpFilterAction.SslRequired)]
    //[ActionFilterIpAddress(IpAddressFilterAction.Deny)]
    //[AuthorizeIpAddress(IpAddressFilterAction.Deny)]
    public class BaseController : ApiController
    {
        public BaseController() { }
    }
}
