
namespace asp.net_tuto_02.Middlewares
{
    public class CustomeExceptionHandeler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error => {ex.Message}");
                context.Response.Headers.ContentType = "text/html";
                await context.Response.WriteAsync($"<h3> {ex.Message} </h3>");
            }
        }
    }
}
