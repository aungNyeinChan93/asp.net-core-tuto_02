namespace asp.net_tuto_02.Classes.Employees
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Position { get; set; } = null!;

        public int Salary { get; set; }

        public Employee() { }

        public Employee(int id,string name,string position,int salary)
        {
            this.Id = id;
            this.Name = name;
            this.Position = position;
            this.Salary = salary;
        }
    }

    public static class EmployeeRepository
    {
        private static List<Employee> _employees =
            [
            new Employee(1,"Chan","Developer",4000),
            new Employee(2,"SUSU","Sale",2900),
            ];

        public static List<Employee> GetEmployees() => _employees;

        public static Employee? GetEmployee(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public static void AddEmployee(Employee employee)=> _employees.Add(employee);

        public static Employee? UpdateEmployee(Employee employee, int id)
        {
            var oldEmployee = _employees.FirstOrDefault(e => e.Id == id);

            if(oldEmployee is null) return null;

            oldEmployee.Name = employee.Name;
            oldEmployee.Position = employee.Position;
            oldEmployee.Salary = employee.Salary;

            return oldEmployee;
        }

        public static void DeleteEmployee(int id) => _employees.Remove(_employees.FirstOrDefault(e => e.Id ==id)!);

    }

}
