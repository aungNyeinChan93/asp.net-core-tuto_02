
namespace asp.net_tuto_02.Middlewares
{
    public class TestMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine($"Before Hit the test middleware");
            await next(context);
            Console.WriteLine($"After Hit the test middleware");
        }
    }
}
