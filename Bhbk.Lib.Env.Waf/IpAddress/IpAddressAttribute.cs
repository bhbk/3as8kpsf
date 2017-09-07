using LukeSkywalker.IPNetwork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Bhbk.Lib.Env.Waf.IpAddress
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionFilterIpAddressAttribute : ActionFilterAttribute
    {
        #region Fields

        private IEnumerable<IPNetwork> cidrList;
        private IpAddressFilterAction action;

        #endregion

        #region Properties

        public IEnumerable<IPNetwork> CidrList
        {
            get
            {
                return this.cidrList;
            }
        }

        #endregion

        #region Constructors

        public ActionFilterIpAddressAttribute(IpAddressFilterAction actionInput)
        {
            if (actionInput == IpAddressFilterAction.Allow)
                this.cidrList = ConfigurationManager.AppSettings[Statics.ApiIpDynamicAllow].Split(',').Select(x => IPNetwork.Parse(x.Trim()));

            else if (actionInput == IpAddressFilterAction.Deny)
                this.cidrList = ConfigurationManager.AppSettings[Statics.ApiIpDynamicDeny].Split(',').Select(x => IPNetwork.Parse(x.Trim()));

            else
                throw new InvalidOperationException();

            this.action = actionInput;
        }

        public ActionFilterIpAddressAttribute(string cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput.Split(',').Select(x => IPNetwork.Parse(x.Trim()));
            this.action = actionInput;
        }

        public ActionFilterIpAddressAttribute(IPNetwork cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = new IPNetwork[] { cidrListInput };
            this.action = actionInput;
        }

        public ActionFilterIpAddressAttribute(IEnumerable<string> cidrListInput, IpAddressFilterAction actionInput)
            : this(cidrListInput.Select(x => IPNetwork.Parse(x)), actionInput)
        {

        }

        public ActionFilterIpAddressAttribute(IEnumerable<IPNetwork> cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput;
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
                    if (!IsIpAddressAllowed(client.Request.UserHostAddress))
                    {
                        string words = String.Format("({0}) {1}", client.Request.UserHostAddress, Statics.MsgApiIpAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    if (!IsIpAddressAllowed(client.Request.Address))
                    {
                        string words = String.Format("({0}) {1}", client.Request.Address, Statics.MsgApiIpAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    if (!IsIpAddressAllowed(client.Request.RemoteIpAddress))
                    {
                        string words = String.Format("({0}) {1}", client.Request.RemoteIpAddress, Statics.MsgApiIpAddressNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
        }

        private bool IsIpAddressAllowed(string request)
        {
            if (IpAddressHelpers.IsIpConfigValid(ref this.action, ref this.cidrList))
            {
                if (IpAddressHelpers.IsIpAddressAllowed(ref this.action, ref this.cidrList, ref request))
                    return true;
                else
                    return false;
            }
            else
                throw new InvalidOperationException();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeIpAddressAttribute : AuthorizeAttribute
    {
        #region Fields

        private IEnumerable<IPNetwork> cidrList;
        private IpAddressFilterAction action;

        #endregion

        #region Properties

        public IEnumerable<IPNetwork> CidrList
        {
            get
            {
                return this.cidrList;
            }
        }

        #endregion

        #region Constructors

        public AuthorizeIpAddressAttribute(IpAddressFilterAction actionInput)
        {
            if (actionInput == IpAddressFilterAction.Allow)
                this.cidrList = ConfigurationManager.AppSettings[Statics.ApiIpDynamicAllow].Split(',').Select(x => IPNetwork.Parse(x.Trim()));

            else if (actionInput == IpAddressFilterAction.Deny)
                this.cidrList = ConfigurationManager.AppSettings[Statics.ApiIpDynamicDeny].Split(',').Select(x => IPNetwork.Parse(x.Trim()));

            else
                throw new InvalidOperationException();

            this.action = actionInput;
        }

        public AuthorizeIpAddressAttribute(string cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput.Split(',').Select(x => IPNetwork.Parse(x.Trim()));
            this.action = actionInput;
        }

        public AuthorizeIpAddressAttribute(IPNetwork cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = new IPNetwork[] { cidrListInput };
            this.action = actionInput;
        }

        public AuthorizeIpAddressAttribute(IEnumerable<string> cidrListInput, IpAddressFilterAction actionInput)
            : this(cidrListInput.Select(x => IPNetwork.Parse(x)), actionInput)
        {

        }

        public AuthorizeIpAddressAttribute(IEnumerable<IPNetwork> cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput;
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
                    return IsIpAddressAllowed(client.Request.UserHostAddress);
                else
                    return false;
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    return IsIpAddressAllowed(client.Request.Address);
                else
                    return false;
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    return IsIpAddressAllowed(client.Request.RemoteIpAddress);
                else
                    return false;
            }
            else
                return false;
        }

        private bool IsIpAddressAllowed(string request)
        {
            if (IpAddressHelpers.IsIpConfigValid(ref this.action, ref this.cidrList))
            {
                if (IpAddressHelpers.IsIpAddressAllowed(ref this.action, ref this.cidrList, ref request))
                    return true;
                else
                    return false;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
