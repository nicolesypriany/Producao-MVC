public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string errorMessage;
        errorMessage = exception.Message;
        context.Response.Redirect($"/Error/GeneralError?errorMessage={Uri.EscapeDataString(errorMessage)}");
        return Task.CompletedTask;
    }
}
