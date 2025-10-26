using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.DTOs.Project;
using MyWebApi.DTOs.Users;
using MyWebApi.Model;
using MyWebApi.Services.Interface;


namespace MyWebApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            return await _context.Project
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _context.Project
                .Include(p => p.ProjectUsers)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return null;

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(ProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateAsync(int id, ProjectDto dto)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null) return null;

            _mapper.Map(dto, project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null) return false;

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUserToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Project.FindAsync(projectId);
            var user = await _context.User.FindAsync(userId);

            if (project == null || user == null)
                return false;

            if (await _context.ProjectUser.AnyAsync(pu => pu.ProjectId == projectId && pu.UserId == userId))
                return true; // już przypisany

            _context.ProjectUser.Add(new ProjectUser { ProjectId = projectId, UserId = userId });
            await _context.SaveChangesAsync();
            return true;
        }
    }
}