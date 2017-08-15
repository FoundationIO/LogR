using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Web.Filters
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var errorStatus = 500;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                if (typeof(ReturnModel<>) == controllerActionDescriptor.MethodInfo.ReturnType.GetGenericTypeDefinition()
                    || typeof(ReturnListModel<,>) == controllerActionDescriptor.MethodInfo.ReturnType.GetGenericTypeDefinition())
                {
                    context.HttpContext.Response.StatusCode = errorStatus;
                    context.Result = new JsonResult(new ReturnModel<Object>(exception));
                }
            }
        }

    }
}
