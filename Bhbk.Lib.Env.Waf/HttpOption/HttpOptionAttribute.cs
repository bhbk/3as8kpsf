using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Bhbk.Lib.Env.Waf.HttpOption
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

        public override void OnActionExecuting(HttpActionContext context)
        {
            //https://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
            //for web-hosted... needs reference to System.Web.dll
            if (context.Request.Properties.ContainsKey(Statics.ApiContextIsHttp))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsHttp];

                if (!IsHttpOptionAllowed(context.Request.RequestUri))
                {
                    string words = String.Format("({0}) {1}", client.Request.UserHostAddress, Statics.MsgApiHttpSessionNotAllowed);
                    context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                }
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (!IsHttpOptionAllowed(context.Request.RequestUri))
                {
                    string words = String.Format("({0}) {1}", client.Request.Address, Statics.MsgApiHttpSessionNotAllowed);
                    context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                }
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (!IsHttpOptionAllowed(context.Request.RequestUri))
                {
                    string words = String.Format("({0}) {1}", client.Request.RemoteIpAddress, Statics.MsgApiHttpSessionNotAllowed);
                    context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, words);
                }
            }
        }

        private bool IsHttpOptionAllowed(Uri url)
        {
            return HttpOptionHelpers.IsHttpOptionAllowed(ref this.action, ref url);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeHttpOptionAttribute : AuthorizeAttribute
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

        protected override bool IsAuthorized(HttpActionContext context)
        {
            //https://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
            //for web-hosted... needs reference to System.Web.dll 
            if (context.Request.Properties.ContainsKey(Statics.ApiContextIsHttp))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsHttp];

                if (client != null)
                    return IsHttpOptionAllowed(context.Request.RequestUri);
                else
                    return false;
            }
            //for self-hosted... needs reference to System.ServiceModel.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsRemoteEndPoint))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsRemoteEndPoint];

                if (client != null)
                    return IsHttpOptionAllowed(context.Request.RequestUri);
                else
                    return false;
            }
            //for self-hosted using owin... needs reference to Microsoft.Owin.dll
            else if (context.Request.Properties.ContainsKey(Statics.ApiContextIsOwin))
            {
                dynamic client = context.Request.Properties[Statics.ApiContextIsOwin];

                if (client != null)
                    return IsHttpOptionAllowed(context.Request.RequestUri);
                else
                    return false;
            }
            else
                return false;
        }

        private bool IsHttpOptionAllowed(Uri url)
        {
            return HttpOptionHelpers.IsHttpOptionAllowed(ref this.action, ref url);
        }
    }
}
