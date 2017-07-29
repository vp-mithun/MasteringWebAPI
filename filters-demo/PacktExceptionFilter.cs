using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace filters_demo
{
    // Exception Filter example
    public class PacktExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            String message = String.Empty;

            var exceptionType = context.Exception.GetType();
            
            if (exceptionType == typeof(ZeroValueException))
            {
                message = context.Exception.Message;
                status = HttpStatusCode.InternalServerError;
            }
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;            
            var err = message;
            response.WriteAsync(err);
        }
    }
}