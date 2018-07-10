using LukeSkywalker.IPNetwork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.IpAddress
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IPAddress remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsIpAddressAllowed(remoteIpAddress.ToString()))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", remoteIpAddress.ToString(), Statics.MsgApiIpAddressNotAllowed),
                };
                return;
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
}
