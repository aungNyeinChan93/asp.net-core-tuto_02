using asp.net_tuto_02.Classes.Employees;
using System.Text.Json;

namespace asp.net_tuto_02.Services
{
    public static class EmployeeService
    {
        public static async void GetAllEmployees(HttpContext context)
        {
            var employees = EmployeeRepository.GetEmployees();
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(employees!);
        }

        public static async void CreateEmployee(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            Employee? employee = JsonSerializer.Deserialize<Employee>(body);
            EmployeeRepository.AddEmployee(employee!);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(employee);
        }

        public static async void GetEmployee(HttpContext context,int id)
        {
            var employee = EmployeeRepository.GetEmployee(id);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(employee);
        }

        public static async void UpdateEmployee(HttpContext context)
        {
            var isSuccess = int.TryParse(context.Request.Query["id"], out int employeeId);
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            Employee? employee = JsonSerializer.Deserialize<Employee>(body);
            Employee? updatedEmployee = EmployeeRepository.UpdateEmployee(employee!, employeeId);

            context.Response.StatusCode = 202;
            await context.Response.WriteAsJsonAsync(updatedEmployee);
        }

        public static async void Delete(HttpContext context)
        {
            //var id = context.Request.RouteValues.ContainsKey("id") ? (int)context.Request.RouteValues["id"] : 0;
            var isSuccess =int.TryParse(context.Request.RouteValues["id"].ToString(),out int employeeId);
            EmployeeRepository.DeleteEmployee(employeeId);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new {message = "Success" });
        }
    }
}
