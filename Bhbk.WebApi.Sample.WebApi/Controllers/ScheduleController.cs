using Bhbk.Lib.Env.Waf.Schedule;
using System.Reflection;
using System.Web.Http;

namespace Bhbk.WebApi.Sample.WebApi.Controllers
{
    [RoutePrefix("schedule")]
    public class ScheduleController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [ActionFilterSchedule(ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        [AuthorizeSchedule(ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        public IHttpActionResult ScheduleDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [ActionFilterSchedule(ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        [AuthorizeSchedule(ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        public IHttpActionResult ScheduleDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [ActionFilterSchedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        //[AuthorizeSchedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        public IHttpActionResult ScheduleStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [ActionFilterSchedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        //[AuthorizeSchedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        public IHttpActionResult ScheduleStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }
    }
}
