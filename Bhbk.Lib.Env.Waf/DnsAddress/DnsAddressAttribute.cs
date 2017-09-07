using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Bhbk.Lib.Env.Waf.DnsAddress
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

        public override void OnActionExecuting(HttpActionContext context)
        {
            //https://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
            //for web-hosted... needs reference to System.Web.dll
            if (context.Request.Properties.ContainsKey(Statics.ApiContextIsHttp))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsHttp];

                if (client != null)
                    if (!IsDnsAddressAllowed(client.Request.UserHostAddress))
                    {
                        string words = String.Format("({0}) {1}", client.Request.UserHostAddress, Statics.MsgApiDnsAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    if (!IsDnsAddressAllowed(client.Request.Address))
                    {
                        string words = String.Format("({0}) {1}", client.Request.Address, Statics.MsgApiDnsAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    if (!IsDnsAddressAllowed(client.Request.RemoteIpAddress))
                    {
                        string words = String.Format("({0}) {1}", client.Request.RemoteIpAddress, Statics.MsgApiDnsAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
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

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeDnsAddressAttribute : AuthorizeAttribute
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

        public AuthorizeDnsAddressAttribute(DnsAddressFilterAction actionInput)
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

        public AuthorizeDnsAddressAttribute(string dnsListInput, DnsAddressFilterAction actionInput)
        {
            if (actionInput == DnsAddressFilterAction.AllowRegEx || actionInput == DnsAddressFilterAction.DenyRegEx)
                this.dnsList =  new string[] { dnsListInput };
            else
                this.dnsList = dnsListInput.Split(',').Select(x => x.Trim());
    
            this.action = actionInput;
        }

        public AuthorizeDnsAddressAttribute(IEnumerable<string> dnsListInput, DnsAddressFilterAction actionInput)
        {
            this.dnsList = dnsListInput;
            this.action = actionInput;
        }

        #endregion

        protected override bool IsAuthorized(HttpActionContext context)
        {
            //https://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
            //for web-hosted... needs reference to System.Web.dll 
            if (context.Request.Properties.ContainsKey(Statics.ApiContextIsHttp))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsHttp];

                if (client != null)
                    return IsDnsAddressAllowed(client.Request.UserHostName);
                else
                    return false;
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    return IsDnsAddressAllowed(client.Request.HostName);
                else
                    return false;
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    return IsDnsAddressAllowed(client.Request.RemoteHostName);
                else
                    return false;
            }
            else
                return false;
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
