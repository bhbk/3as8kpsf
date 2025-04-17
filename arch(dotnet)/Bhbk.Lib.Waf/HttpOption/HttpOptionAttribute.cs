using Bhbk.Lib.Waf.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Bhbk.Lib.Waf.HttpOption
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HttpOptionAttribute : ActionFilterAttribute
    {
        #region Fields

        private HttpFilterAction action;

        #endregion

        #region Constructors

        public HttpOptionAttribute(HttpFilterAction actionInput)
        {
            this.action = actionInput;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var localeUri = new Uri(context.HttpContext.Request.Scheme);

            if (!IsHttpOptionAllowed(localeUri))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ContentType = "application/json",
                    Content = String.Format("({0}) {1}", localeUri.ToString(), Constants.MsgApiHttpSessionNotAllowed),
                };
                return;
            }
        }

        private bool IsHttpOptionAllowed(Uri url)
        {
            return HttpOptionHelpers.IsHttpOptionAllowed(ref this.action, ref url);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeHttpOptionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        #region Fields

        private HttpFilterAction action;

        #endregion

        #region Constructors

        public AuthorizeHttpOptionAttribute(HttpFilterAction actionInput)
        {
            this.action = actionInput;
        }

        #endregion

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var localeUri = new Uri(context.HttpContext.Request.Scheme);

            if (!IsHttpOptionAllowed(localeUri))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        private bool IsHttpOptionAllowed(Uri url)
        {
            return HttpOptionHelpers.IsHttpOptionAllowed(ref this.action, ref url);
        }
    }
}
