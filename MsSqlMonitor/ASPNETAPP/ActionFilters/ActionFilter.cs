using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace ASPNETAPP.ActionFilters
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.Response != null)
            {
                var objectContent = context.Response.Content as ObjectContent;
                if (objectContent != null)
                {
                    var value = objectContent.Value;
                    if (value == null)
                    {
                        context.Response = context.Request
                            .CreateErrorResponse(HttpStatusCode.NotFound, "Return data is null");
                    }
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
