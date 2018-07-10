using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Bhbk.Lib.Waf.HttpOption
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionFilterHttpOptionAttribute : ActionFilterAttribute
    {
        #region Fields

        private HttpFilterAction action;

        #endregion

        #region Constructors

        public ActionFilterHttpOptionAttribute(HttpFilterAction actionInput)
        {
            this.action = actionInput;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Uri localeUri = new Uri(context.HttpContext.Request.Scheme);

            if (!IsHttpOptionAllowed(localeUri))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", localeUri.ToString(), Statics.MsgApiHttpSessionNotAllowed),
                };
                return;
            }
        }

        private bool IsHttpOptionAllowed(Uri url)
        {
            return HttpOptionHelpers.IsHttpOptionAllowed(ref this.action, ref url);
        }
    }
}
