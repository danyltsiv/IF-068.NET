using CommonLib;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ASPNETAPP.ActionFilters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        [Dependency]
        public ISLogger Logger { get; set; }

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                LogException(context, "NotImplementedException");
            }
            else if (context.Exception is NullReferenceException)
            {
                LogException(context, "NullReferenceException");
            }
            else if (context.Exception is HttpRequestException)
            {
                LogException(context, "HttpRequestException");
            }
            else if (context.Exception is HttpCompileException)
            {
                LogException(context, "HttpCompileException");
            }
            else if (context.Exception is ArgumentOutOfRangeException)
            {
                LogException(context, "ArgumentOutOfRangeException");
            }
            else
            {
                LogException(context, "Other Exception");
            }
        }

        private void LogException(HttpActionExecutedContext context, string exceptionName)
        {
            var controllerName = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = context.ActionContext.ActionDescriptor.ActionName;
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, exceptionName);
            string message = String.Format("{0} in {1}.{2}", exceptionName, controllerName, actionName);
            Logger.Error(message, context.Exception);
        }
    }
}