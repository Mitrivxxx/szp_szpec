using MyWebApi.DTOs.Users;
using MyWebApi.Model;

namespace MyWebApi.DTOs.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<UserInProjectDto> Users { get; set; } = new();

        public class CreateProjectDto
        {
            public string Name { get; set; } = default!;
        }

        public class AddUserToProjectDto
        {
            public int UserId { get; set; }
        }

        public class UserInProjectDto
        {
            public int Id { get; set; }
            public string Login { get; set; } = default!;
        }
    }
}
