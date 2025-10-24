namespace MyWebApi.DTOs
{
    public class UpdateUserDto
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
