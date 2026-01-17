using asp.net_tuto_02.Classes.Employees;
using asp.net_tuto_02.Classes.Users;
using asp.net_tuto_02.Middlewares;
using asp.net_tuto_02.Services;
using asp.net_tuto_02.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddTransient<MyMiddlewareOne>();
builder.Services.AddTransient<LoggerMiddleware>();
builder.Services.AddTransient<TestMiddleware>();
builder.Services.AddTransient<CustomeExceptionHandeler>();

builder.Services.AddRouting(option =>
{
    option.ConstraintMap.Add("type", typeof(CustomerConstaint));
});

// Build
var app = builder.Build();

// Static Page
app.UseStaticFiles();


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


//app.UseEndpoints(endpoint =>
//{
//    endpoint.MapGet("/customers", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync("Get All Customers");
//    });


//    endpoint.MapDelete("/customers/{id}/{category=medium}/{size?}", async (HttpContext context) =>
//    {
//        Console.WriteLine($"{context.Request.RouteValues["id"]}");
//        await context.Response.WriteAsync($"Delete Customers Id- {context.Request.RouteValues["id"]} \n");
//        await context.Response.WriteAsync($"Delete Customers - {context.Request.RouteValues["category"]} \n");
//        await context.Response.WriteAsync($"Size - {context.Request.RouteValues["size"]}");
//    });

//    endpoint.MapGet("/customers/type/{customerType:type}", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync($"Customer Type : {context.Request.RouteValues["customerType"]}");
//    });
//});

app.UseEndpoints((endPoint) =>
{

    endPoint.MapGet("/", async (HttpContext context) =>
    {
        //EmployeeService.GetAllEmployees(context);
        foreach (var key in context.Request.Headers.Keys)   
        {
            await context.Response.WriteAsync($"{key} => {context.Request.Headers[key]} \n");
        }
    });

    endPoint.MapGet("/employees", (HttpContext context) =>
    {
        EmployeeService.GetAllEmployees(context);
    });

    endPoint.MapGet("/employees/byId", ([FromQuery (Name ="id")] int[] ids) =>
    {
        var employees = EmployeeRepository.GetEmployees();
        var emps = employees.Where(x => ids.Contains(x.Id)).ToList();

        return emps;
    });


    endPoint.MapGet("/employees/{id:int}", (HttpContext context) =>
    {
        var isSuccess = int.TryParse(context.Request.RouteValues["id"].ToString(), out int employeeId);
        if (!isSuccess) throw new Exception("employee id is not found");
        EmployeeService.GetEmployee(context, employeeId);
    });

    endPoint.MapPost("/employees", ([AsParameters] CreateEmployeeParams param) =>
    {
        var employee = new Employee();
        employee.Id = param.Id; 
        employee.Name = param.Name;
        employee.Position = param.Position;
        employee.Salary = param.Salary;

        EmployeeRepository.AddEmployee(employee);

        return new { employee };

        //EmployeeService.CreateEmployee(context);
    });

    endPoint.MapPut("/employees", (HttpContext context) =>
    {
         EmployeeService.UpdateEmployee(context);
    });

    endPoint.MapDelete("/employees/{id}", (HttpContext context) =>
    {
        EmployeeService.Delete(context);
    });

    endPoint.MapGet("/test/{id:int}/{name=koko}/{size:alpha?}", async ([FromRoute]int id, [FromRoute] string? size,HttpContext context, [FromQuery]string? location) =>
    {
        await context.Response.WriteAsync($"ID => {id} | Name => {context.Request.RouteValues["name"]} | Size => {size} | Location => {location} ");
    });

    endPoint.MapGet("/admin", async (HttpContext context, [FromHeader] string? Authorization ) =>
    {
        var token = Authorization?.Split(" ").Last<string>();
        await context.Response.WriteAsJsonAsync(new 
        {
            path = "/admin",
            token,
        }
        );
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