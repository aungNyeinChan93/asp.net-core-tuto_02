
namespace asp.net_tuto_02.Middlewares
{
    public class LoggerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine($" {context.Request.Method} : {context.Request.Path} {DateTime.Now}");
            await next(context);
        }
    }
}
