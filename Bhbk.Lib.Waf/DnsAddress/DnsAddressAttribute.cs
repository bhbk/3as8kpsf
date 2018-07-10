using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.DnsAddress
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionFilterDnsAddressAttribute : ActionFilterAttribute
    {
        #region Fields

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

        public ActionFilterDnsAddressAttribute(DnsAddressFilterAction actionInput)
        {
            if (actionInput == DnsAddressFilterAction.Allow)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicAllow].Split(',').Select(x => x.Trim());

            else if (actionInput == DnsAddressFilterAction.AllowRegEx)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicAllowRegEx].Select(x => x.ToString());

            else if (actionInput == DnsAddressFilterAction.AllowContains)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicAllowContains].Split(',').Select(x => x.Trim());

            else if (actionInput == DnsAddressFilterAction.Deny)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicDeny].Split(',').Select(x => x.Trim());

            else if (actionInput == DnsAddressFilterAction.DenyRegEx)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicDenyRegEx].Select(x => x.ToString());

            else if (actionInput == DnsAddressFilterAction.DenyContains)
                this.dnsList = ConfigurationManager.AppSettings[Statics.ApiDnsDynamicDenyContains].Split(',').Select(x => x.Trim());

            else
                throw new InvalidOperationException();

            this.action = actionInput;
        }

        public ActionFilterDnsAddressAttribute(string dnsListInput, DnsAddressFilterAction actionInput)
        {
            if (actionInput == DnsAddressFilterAction.AllowRegEx || actionInput == DnsAddressFilterAction.DenyRegEx)
                this.dnsList =  new string[] { dnsListInput };
            else
                this.dnsList = dnsListInput.Split(',').Select(x => x.Trim());
    
            this.action = actionInput;
        }

        public ActionFilterDnsAddressAttribute(IEnumerable<string> dnsListInput, DnsAddressFilterAction actionInput)
        {
            this.dnsList = dnsListInput;
            this.action = actionInput;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IPAddress remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsDnsAddressAllowed(remoteIpAddress.ToString()))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", remoteIpAddress.ToString(), Statics.MsgApiDnsAddressNotAllowed),                    
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
                else
                    return false;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
