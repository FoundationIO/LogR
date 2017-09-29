using System;
using System.Collections.Generic;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.Web.Filters
{
    public class PerformanceFilter : ActionFilterAttribute, IExceptionFilter
    {
        private DateTime startTime;
        private ILog log;

        public PerformanceFilter(ILog log)
        {
            this.log = log;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            startTime = DateTime.Now;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            HandlePerfLog(context.ActionDescriptor, context.HttpContext, context.Exception);
            base.OnActionExecuted(context);
        }

        public void OnException(ExceptionContext context)
        {
            HandlePerfLog(context.ActionDescriptor, context.HttpContext, context.Exception);
        }

        private void HandlePerfLog(ActionDescriptor descriptor, HttpContext httpContext, Exception ex)
        {
            var endTime = DateTime.Now;
            if (!(descriptor is ControllerActionDescriptor controllerActionDescriptor))
                return;

            var parametersForLog = new List<KeyValuePair<string, object>>();
            foreach (var param in controllerActionDescriptor.Parameters)
            {
                parametersForLog.Add(new KeyValuePair<string, object>(param.Name, new object()));
            }

            log.Performance(controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName, startTime, endTime, parametersForLog, httpContext.Response.StatusCode, ex == null ? "Completed" : "Error", ex == null ? string.Empty : "Exception - " + ExceptionUtils.RecursivelyGetExceptionMessage(ex));
        }
    }
}
