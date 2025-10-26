namespace MyWebApi.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; } = default!;
        public string Login { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
