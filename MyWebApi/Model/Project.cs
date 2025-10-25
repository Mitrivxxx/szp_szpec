namespace MyWebApi.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
