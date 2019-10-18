using Bhbk.Lib.Waf.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Bhbk.WebApi.Sample.Controllers
{
    [Route("schedule")]
    public class ScheduleController : BaseController
    {
        [HttpGet]
        [Route("v1/dynamic-allow")]
        [Schedule(ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        public IActionResult ScheduleDynamicAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/dynamic-deny")]
        [Schedule(ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        public IActionResult ScheduleDynamicDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-allow")]
        [Schedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily)]
        public IActionResult ScheduleStaticAllow()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }

        [HttpGet]
        [Route("v1/static-deny")]
        [Schedule("2017:01:01T17:00:00-2018:01:01T08:00:00", ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily)]
        public IActionResult ScheduleStaticDeny()
        {
            return Ok(Assembly.GetAssembly(typeof(ScheduleController)).GetName().Version.ToString());
        }
    }
}