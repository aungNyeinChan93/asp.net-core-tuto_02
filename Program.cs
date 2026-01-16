using asp.net_tuto_02.Classes.Users;
using asp.net_tuto_02.Middlewares;
using asp.net_tuto_02.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddTransient<MyMiddlewareOne>();
builder.Services.AddTransient<LoggerMiddleware>();
builder.Services.AddTransient<TestMiddleware>();
builder.Services.AddTransient<CustomeExceptionHandeler>();

// Build
var app = builder.Build();

// Middlewares
app.UseMiddleware<CustomeExceptionHandeler>();
app.UseMiddleware<MyMiddlewareOne>();
app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<TestMiddleware>();

app.UseRouting();

app.MapWhen((context) =>
{
    if (context.Request.Path == "/tests")
    {
        return true;
    }
    else
    {
        return false;
    };
},
    (appBuilder) =>
    {
        appBuilder.Use(async(context,next) =>
        {
            await context.Response.WriteAsync("Tests \n");
            await next(context);
        });
    }
);

app.Map("/test-two", (appBuilder) =>
{
    appBuilder.Use(async(context,next) =>
    {
        await context.Response.WriteAsync($"Test Two! \n");
        await next(context);
    });
});

// Middlwares
app.UseWhen((context) =>
{
    return context.Request.Path == "/test-three";
}, (appBuilder) =>
{
    appBuilder.Use(async (context,next) =>
    {
        Console.WriteLine("Hit test-three middleware! \n");
        await next(context);
        Console.WriteLine("After ->Hit test-three middleware! \n");
    });
});


app.Use(async (HttpContext context, RequestDelegate next) =>
{
    //await context.Response.WriteAsync($"Test middleware 4");
    Console.WriteLine("Hit test-four middleware! \n");
    await next(context);
    Console.WriteLine("After ->Hit test-four middleware! \n");

});

app.Map("/test-three", (appBuilder) =>
{
    appBuilder.Use(async (context, next) =>
    {
        await next(context);
        throw new Exception("test three error!");
    });
});


app.UseEndpoints(endpoint =>
{
    endpoint.MapGet("/customers", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Get All Customers");
    });

    endpoint.MapPost("/customers", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Create Customers");
    });
});

app.Run(async (HttpContext context) =>
{
    if(context.Request.Path == "/users")
    {
        // all users GET => /users
        if(context.Request.Method == "GET")
        {
            if (context.Request.Query.ContainsKey("id"))
            {
                UserService.GetUser(context);
            };
            UserService.GetUsers(context);
        }

        // create users POST => /user
        if (context.Request.Method == "POST")
        {
            UserService.CreateUser(context);
        }

        // create users POST => /user
        if (context.Request.Method == "PUT")
        {
           UserService.Update(context);
        }

       
    }

    if (context.Request.Path == "/err-test")
    {
        throw new Exception("Test error");
        
    }
});



app.Run();