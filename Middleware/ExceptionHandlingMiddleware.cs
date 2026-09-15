using System.Net;
using System.Text.Json;

namespace LiquidlabsPostcacheApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
			await HandleExceptionAsync(context);
		}
	}

	private static Task HandleExceptionAsync(HttpContext ctx)
	{
		ctx.Response.ContentType = "application/json";
		ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var err = new { message = "Something went wrong. Please try again later." };
		return ctx.Response.WriteAsync(JsonSerializer.Serialize(err));
	}
}