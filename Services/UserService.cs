using asp.net_tuto_02.Classes.Users;
using System;
using System.Text.Json;

namespace asp.net_tuto_02.Services
{
    public static class UserService
    {
        public async static void GetUsers(HttpContext context)
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

        public async static void CreateUser(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            User? user = JsonSerializer.Deserialize<User>(body);

            if (user is null) return;

            UserRepository.AddUser(user);

            context.Response.StatusCode = 201;
            await context.Response.WriteAsJsonAsync(user);
        }

        public async static void Update(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            User? user = JsonSerializer.Deserialize<User>(body) ?? throw new Exception("User Update Fail!");
            User? updatedUser = UserRepository.UpdateUser(user);

            context.Response.StatusCode =202;
            await context.Response.WriteAsJsonAsync(updatedUser);
        }

        public async static void GetUser(HttpContext context)
        {
            var isSuccess = int.TryParse(context.Request.Query["id"][0], out var id);
            if(!isSuccess) return;
            User? user = UserRepository.GetUser(id);
            if(user is null)
            {
                //context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"User not Found");
            };

            //context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync<User>(user!);
        }
    }
}
