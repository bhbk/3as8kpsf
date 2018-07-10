using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bhbk.WebApi.Sample.Controllers
{
    [AllowAnonymous]
    //[Authorize]
    //[ActionFilterDnsAddress(DnsAddressFilterAction.Deny)]
    //[AuthorizeDnsAddress(DnsAddressFilterAction.Deny)]
    //[ActionFilterHttpOption(HttpFilterAction.SslRequired)]
    //[AuthorizeHttpOption(HttpFilterAction.SslRequired)]
    //[ActionFilterIpAddress(IpAddressFilterAction.Deny)]
    //[AuthorizeIpAddress(IpAddressFilterAction.Deny)]
    public class BaseController : Controller
    {
        public BaseController() { }
    }
}