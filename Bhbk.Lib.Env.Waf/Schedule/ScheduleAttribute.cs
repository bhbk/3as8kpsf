using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Bhbk.Lib.Env.Waf.Schedule
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionFilterScheduleAttribute : ActionFilterAttribute
    {
        #region Fields

        private List<Tuple<DateTime, DateTime>> scheduleList;
        private ScheduleFilterAction action;
        private ScheduleFilterOccur occur;

        #endregion

        #region Properties

        public IEnumerable<Tuple<DateTime, DateTime>> ScheduleList
        {
            get
            {
                return this.scheduleList;
            }
        }

        #endregion

        #region Constructors

        public ActionFilterScheduleAttribute(ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            if (actionInput == ScheduleFilterAction.Allow)
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicAllow].Split(',').Select(x => x.Trim()));

            else if (actionInput == ScheduleFilterAction.Deny)
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicDeny].Split(',').Select(x => x.Trim()));

            this.action = actionInput;
            this.occur = actionOccur;
        }

        public ActionFilterScheduleAttribute(string scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput.Split(',').Select(x => x.Trim()));
            this.action = actionInput;
            this.occur = actionOccur;
        }

        public ActionFilterScheduleAttribute(string[] scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput);
            this.action = actionInput;
            this.occur = actionOccur;
        }

        public ActionFilterScheduleAttribute(List<Tuple<DateTime, DateTime>> scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = scheduleListInput;
            this.action = actionInput;
            this.occur = actionOccur;
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
                    if (!IsScheduleAllowed(DateTime.Now))
                    {
                        string words = String.Format("({0}) {1}", client.Request.UserHostAddress, Statics.MsgApiScheduleNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    if (!IsScheduleAllowed(DateTime.Now))
                    {
                        string words = String.Format("({0}) {1}", client.Request.Address, Statics.MsgApiScheduleNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    if (!IsScheduleAllowed(DateTime.Now))
                    {
                        string words = String.Format("({0}) {1}", client.Request.RemoteIpAddress, Statics.MsgApiScheduleNotAllowed);
                        context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                    }
            }
        }

        private bool IsScheduleAllowed(DateTime when)
        {
            if (ScheduleHelpers.IsScheduleConfigValid(ref this.action, ref this.occur, ref this.scheduleList))
            {
                if (ScheduleHelpers.IsScheduleAllowed(ref this.action, ref this.occur, ref this.scheduleList, ref when))
                    return true;
                else
                    return false;
            }
            else
                throw new InvalidOperationException();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeScheduleAttribute : AuthorizeAttribute
    {
        #region Fields

        private List<Tuple<DateTime, DateTime>> scheduleList;
        private ScheduleFilterAction action;
        private ScheduleFilterOccur occur;

        #endregion

        #region Properties

        public IEnumerable<Tuple<DateTime, DateTime>> ScheduleList
        {
            get
            {
                return this.scheduleList;
            }
        }

        #endregion

        #region Constructors

        public AuthorizeScheduleAttribute(ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            if (actionInput == ScheduleFilterAction.Allow)
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicAllow].Split(',').Select(x => x.Trim()));

            else if (actionInput == ScheduleFilterAction.Deny)
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicDeny].Split(',').Select(x => x.Trim()));

            else
                throw new InvalidOperationException();

            this.action = actionInput;
            this.occur = actionOccur;
        }

        public AuthorizeScheduleAttribute(string scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput.Split(',').Select(x => x.Trim()));
            this.action = actionInput;
            this.occur = actionOccur;
        }

        public AuthorizeScheduleAttribute(string[] scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput);
            this.action = actionInput;
            this.occur = actionOccur;
        }

        public AuthorizeScheduleAttribute(List<Tuple<DateTime, DateTime>> scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = scheduleListInput;
            this.action = actionInput;
            this.occur = actionOccur;
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
                    return IsScheduleAllowed(DateTime.Now);
                else
                    return false;
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    return IsScheduleAllowed(DateTime.Now);
                else
                    return false;
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    return IsScheduleAllowed(DateTime.Now);
                else
                    return false;
            }
            else
                return false;
        }

        private bool IsScheduleAllowed(DateTime when)
        {
            if (ScheduleHelpers.IsScheduleConfigValid(ref this.action, ref this.occur, ref this.scheduleList))
            {
                if (ScheduleHelpers.IsScheduleAllowed(ref this.action, ref this.occur, ref this.scheduleList, ref when))
                    return true;
                else
                    return false;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
