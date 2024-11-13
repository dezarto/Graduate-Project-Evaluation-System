using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectAppService(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectAppAsync()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> GetProjectAppByIdAsync(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task AddProjectAppAsync(ProjectDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _projectService.AddProjectAsync(project);

            projectDto.ProjectId = project.ProjectId;
        }

        public async Task UpdateProjectAppAsync(ProjectDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _projectService.UpdateProjectAsync(project);
        }

        public async Task DeleteProjectAppAsync(int id)
        {
            await _projectService.DeleteProjectAsync(id);
        }
    }
}
