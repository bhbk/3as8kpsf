using Bhbk.Lib.Waf.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.IpAddress
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IpAddressAttribute : ActionFilterAttribute
    {
        #region Fields

        private IConfiguration conf;
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

        public IpAddressAttribute(IpAddressFilterAction actionInput)
        {
            switch (actionInput)
            {
                case IpAddressFilterAction.Allow:
                    {
                        this.cidrList = conf.GetSection("FirewallRules:" + Constants.ApiIpDynamicAllow).GetChildren().Select(x => IPNetwork.Parse(x.Value.Trim()));
                    }
                    break;

                case IpAddressFilterAction.Deny:
                    {
                        this.cidrList = conf.GetSection("FirewallRules:" + Constants.ApiIpDynamicDeny).GetChildren().Select(x => IPNetwork.Parse(x.Value.Trim()));
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }

            this.action = actionInput;
        }

        public IpAddressAttribute(string cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput.Split(',').Select(x => IPNetwork.Parse(x.Trim()));
            this.action = actionInput;
        }

        public IpAddressAttribute(IPNetwork cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = new IPNetwork[] { cidrListInput };
            this.action = actionInput;
        }

        public IpAddressAttribute(IEnumerable<string> cidrListInput, IpAddressFilterAction actionInput)
            : this(cidrListInput.Select(x => IPNetwork.Parse(x)), actionInput) { }

        public IpAddressAttribute(IEnumerable<IPNetwork> cidrListInput, IpAddressFilterAction actionInput)
        {
            this.cidrList = cidrListInput;
            this.action = actionInput;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            conf = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsIpAddressAllowed(remoteIpAddress.ToString()))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", remoteIpAddress.ToString(), Constants.MsgApiIpAddressNotAllowed),
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
