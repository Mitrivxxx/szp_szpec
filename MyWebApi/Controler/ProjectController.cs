using Microsoft.AspNetCore.Mvc;
using MyWebApi.DTOs.Project;
using MyWebApi.Services.Interface;
using static MyWebApi.DTOs.Project.ProjectDto;

namespace MyWebApi.Controler
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Zwraca listę wszystkich projektów w systemie.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }
        /// <summary>
        /// Zwraca projekt o wybranym id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();

            return Ok(project);
        }
        /// <summary>
        /// tworzy nowy projekt
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto dto)
        {
            var project = await _projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }
        /// <summary>
        /// edycja projektu
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectDto dto)
        {
            var updated = await _projectService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }


        /// <summary>
        /// dodaj user do projektu
        /// </summary>
        [HttpPost("{projectId}/add-user")]
        public async Task<IActionResult> AddUser(int projectId, [FromBody] AddUserToProjectDto dto)
        {
            var success = await _projectService.AddUserToProjectAsync(projectId, dto.UserId);
            if (!success) return BadRequest("Nie można dodać użytkownika do projektu.");
            return Ok("Użytkownik dodany do projektu.");
        }

    }
}