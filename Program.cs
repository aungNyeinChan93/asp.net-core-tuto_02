using asp.net_tuto_02.Classes.Users;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!"); 

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
            return;
        });
    }
);

app.Map("/test-two", (appBuilder) =>
{
    appBuilder.Use(async(context,next) =>
    {
        await context.Response.WriteAsync($"Test Two! \n");
        await next(context);
        return;
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
        await context.Response.WriteAsync("Test Three! \n");
        await next(context);
    });
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync($"Test middleware 4");
    await next(context);
});

app.Run(async (HttpContext context) =>
{
    if(context.Request.Path == "/users")
    {
        // all users GET => /users
        if(context.Request.Method == "GET")
        {
            List<User>? users = UserRepository.GetAllUsers();

            if(users is null || users.Count() == 0)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = "user not found!" });
                return;
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(users);
            return;
        }

        // create users POST => /user
        if (context.Request.Method == "POST")
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            User? user = JsonSerializer.Deserialize<User>(body);

            if (user is null) return;

            UserRepository.AddUser(user);

            context.Response.StatusCode = 201;
            await context.Response.WriteAsJsonAsync(user);
            return;
        }
    }
});

app.Run();
