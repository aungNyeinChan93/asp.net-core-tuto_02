
namespace asp.net_tuto_02.Middlewares
{
    public class MyMiddlewareOne : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Hit The MyMiddlewareOne");
            await next(context);
        }
    }
}

