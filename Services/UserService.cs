using asp.net_tuto_02.Classes.Users;
using System;
using System.Text.Json;

namespace asp.net_tuto_02.Services
{
    static public class UserService
    {
        async public static void GetUsers(HttpContext context)
        {
            List<User>? users = UserRepository.GetAllUsers();

            if (users is null || users.Count() == 0)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = "user not found!" });
                return;
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(users);
            return;
        }

        async public static void CreateUser(HttpContext context)
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
}
