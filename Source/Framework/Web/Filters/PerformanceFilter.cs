using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Web.Filters
{
    public class PerformanceFilter : ActionFilterAttribute
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
            var endTime = DateTime.Now;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var parametersForLog = new List<KeyValuePair<string, object>>();
                foreach(var param in controllerActionDescriptor.Parameters)
                {
                    parametersForLog.Add(new KeyValuePair<string, object>(param.Name, new object()));
                }
                log.Performance(controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName, startTime, endTime, parametersForLog, context.HttpContext.Response.StatusCode, "", "");
            }
            base.OnActionExecuted(context);
        }

    }
}
