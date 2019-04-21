using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;

namespace Bhbk.Lib.Hosting.Filters
{
    //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcore
    public class SwaggerFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (attributes.Any())
                operation.Responses.Add(((int)HttpStatusCode.Unauthorized).ToString(),
                    new Response { Description = HttpStatusCode.Unauthorized.ToString() });
        }
    }
}
