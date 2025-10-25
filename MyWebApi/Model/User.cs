namespace MyWebApi.Model
{
    public class User
    {
        public int Id { get; set; }          
        public string Login { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "User";

        public ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
