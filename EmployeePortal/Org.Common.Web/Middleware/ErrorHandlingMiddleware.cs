using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.Common.Exception;


namespace Org.Common.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DuplicateEtityException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync($"\"{e.Message}\"");
            }
            catch (OperationException e)
            {
                context.Response.StatusCode = e.StatusCode ?? (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = e.Message }));
            }
            catch (ArgumentException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = e.Message }));
            }
        }
    }
}
