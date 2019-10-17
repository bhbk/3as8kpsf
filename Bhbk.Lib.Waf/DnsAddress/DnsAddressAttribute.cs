using Bhbk.Lib.Waf.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.DnsAddress
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DnsAddressAttribute : ActionFilterAttribute
    {
        #region Fields

        private IConfiguration conf;
        private IEnumerable<string> dnsList;
        private IEnumerable<IPHostEntry> ipList;
        private DnsAddressFilterAction action;

        #endregion

        #region Properties

        public IEnumerable<string> DnsList
        {
            get
            {
                return this.dnsList;
            }
        }

        public IEnumerable<IPHostEntry> IpList
        {
            get
            {
                return this.ipList;
            }
        }

        #endregion

        #region Constructors

        public DnsAddressAttribute(DnsAddressFilterAction actionInput)
        {
            switch (actionInput)
            {
                case DnsAddressFilterAction.Allow:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicAllow).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                case DnsAddressFilterAction.AllowContains:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicAllowContains).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                case DnsAddressFilterAction.AllowRegEx:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicAllowRegEx).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                case DnsAddressFilterAction.Deny:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicDeny).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                case DnsAddressFilterAction.DenyContains:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicDenyContains).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                case DnsAddressFilterAction.DenyRegEx:
                    {
                        this.dnsList = conf.GetSection("FirewallRules:" + Constants.ApiDnsDynamicDenyRegEx).GetChildren().Select(x => x.Value.Trim());
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }

            this.action = actionInput;
        }

        public DnsAddressAttribute(string dnsListInput, DnsAddressFilterAction actionInput)
        {
            if (actionInput == DnsAddressFilterAction.AllowRegEx || actionInput == DnsAddressFilterAction.DenyRegEx)
                this.dnsList =  new string[] { dnsListInput };
            else
                this.dnsList = dnsListInput.Split(',').Select(x => x.Trim());
    
            this.action = actionInput;
        }

        public DnsAddressAttribute(IEnumerable<string> dnsListInput, DnsAddressFilterAction actionInput)
        {
            this.dnsList = dnsListInput;
            this.action = actionInput;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            conf = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsDnsAddressAllowed(remoteIpAddress.ToString()))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", remoteIpAddress.ToString(), Constants.MsgApiDnsAddressNotAllowed),                    
                };
                return;
            }
        }

        private bool IsDnsAddressAllowed(string request)
        {
            if (DnsAddressHelpers.IsDnsConfigValid(ref this.action, ref this.dnsList))
            {
                if (DnsAddressHelpers.IsDnsAddressAllowed(ref this.action, ref this.dnsList, ref this.ipList, ref request))
                    return true;

                return false;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
