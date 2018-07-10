using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.Schedule
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
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicAllow].Split(',').Select(x => x.Trim()), actionOccur);

            else if (actionInput == ScheduleFilterAction.Deny)
                this.scheduleList = ScheduleHelpers.ParseScheduleConfig(ConfigurationManager.AppSettings[Statics.ApiScheduleDynamicDeny].Split(',').Select(x => x.Trim()), actionOccur);

            this.action = actionInput;
            this.occur = actionOccur;
        }

        public ActionFilterScheduleAttribute(string scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput.Split(',').Select(x => x.Trim()), actionOccur);
            this.action = actionInput;
            this.occur = actionOccur;
        }

        public ActionFilterScheduleAttribute(string[] scheduleListInput, ScheduleFilterAction actionInput, ScheduleFilterOccur actionOccur)
        {
            this.scheduleList = ScheduleHelpers.ParseScheduleConfig(scheduleListInput, actionOccur);
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IPAddress remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsScheduleAllowed(DateTime.Now))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", remoteIpAddress.ToString(), Statics.MsgApiScheduleNotAllowed),
                };
                return;
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
}
