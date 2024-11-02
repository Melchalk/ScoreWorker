using Serilog;
using System.Net;
using WebLibrary.Backend.Models.Exceptions;

namespace ScoreWorker.Infrastructure.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message);

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            StatusCodeException statusException => (int)statusException.HttpStatus,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        await context.Response.WriteAsync(exception.Message);
    }
}
