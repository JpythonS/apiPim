using System.Net;
using System.Text.Json;
using api_pim.Exceptions;

namespace api_pim.Middleware;

public class ApiExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ApiException ex)
    {
        var result = JsonSerializer.Serialize(new { error = ex.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex.StatusCode;
        await context.Response.WriteAsync(result);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var result = JsonSerializer.Serialize(new { error = "Ocorreu um erro interno no servidor." });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(result);
    }
}
