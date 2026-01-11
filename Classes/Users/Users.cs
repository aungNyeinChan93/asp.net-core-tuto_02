namespace asp.net_tuto_02.Classes.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public User() { }

        public User(int id,string name ,string email,string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
    }

    static class UserRepository
    {
        private static List<User> _users = new List<User>()
        {
            new User(1,"aung ","aung@123","123123123"),
            new User(2,"susu","susu@123","123123123"),
        };

        public static List<User>? GetAllUsers() => _users.Count >0 ? _users: null ;

        public static void AddUser(User? user) =>  _users.Add(user!);
    }
}
