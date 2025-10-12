namespace MyWebApi.Models
{
    public class UserCreateDto
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
