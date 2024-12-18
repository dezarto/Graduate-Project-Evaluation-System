using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class TeamAppService : ITeamAppService
    {
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IProjectService _projectService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IMapper _mapper;

        public TeamAppService(IUserService userService, ITeamService teamService, IProjectService projectService, ITeamMemberService teamMemberService, IMapper mapper)
        {
            _userService = userService;
            _teamService = teamService;
            _projectService = projectService;
            _teamMemberService = teamMemberService;
            _mapper = mapper;
        }

        public async Task<object> CreateTeamAsync(string username, TeamCreator teamCreator)
        {
            var student = await _userService.GetByStudentNumberAsync(username);

            if (await _teamMemberService.GetByUserIdAsync(student.UserId) != null)
            {
                throw new Exception("You already have a team.");
            }

            var newProject = new Project
            {
                Description = teamCreator.Description,
                ProjectName = teamCreator.ProjectName,
            };
            await _projectService.AddProjectAsync(newProject);

            var project = await _projectService.GetProjectByIdAsync(newProject.ProjectId);

            if (project == null)
            {
                throw new Exception("Project creation failed.");
            }

            var newTeam = new Team
            {
                TeamName = teamCreator.TeamName,
                AdvisorId = student.ProfessorId,
                ProjectId = project.ProjectId,
                isActive = false,
            };
            await _teamService.AddTeamAsync(newTeam);

            foreach (var studentUsername in teamCreator.StudentList)
            {
                var studentL = await _userService.GetByStudentNumberAsync(studentUsername.StudentNumber);

                if (studentL == null)
                {
                    Console.WriteLine($"Student not found: {studentUsername.StudentNumber}");
                    continue;
                }

                var newTeamMember = new TeamMember
                {
                    TeamId = newTeam.TeamId,
                    UserId = studentL.UserId,
                };

                await _teamMemberService.AddTeamMemberAsync(newTeamMember);

                var newUser = new User
                {
                    UserId = studentL.UserId,
                    Email = studentL.Email,
                    FullName = studentL.FullName,
                    StudentNumber = studentL.StudentNumber,
                    Role = "Student",
                    ProfessorId = teamCreator.SelectedProfessorId,
                };

                await _userService.UpdateUserAsync(newUser);
            }

            return new { TeamId = newTeam.TeamId, ProjectId = project.ProjectId, Message = "Team created successfully" };
        }

        public async Task<IEnumerable<TeamDTO>> GetAllTeamAppAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return _mapper.Map<IEnumerable<TeamDTO>>(teams);
        }

        public async Task<TeamDTO> GetTeamAppByIdAsync(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            return _mapper.Map<TeamDTO>(team);
        }

        public async Task AddTeamAppAsync(TeamDTO teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            await _teamService.AddTeamAsync(team);

            teamDto.TeamId = team.TeamId;
        }

        public async Task UpdateTeamAppAsync(TeamDTO teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            await _teamService.UpdateTeamAsync(team);
        }

        public async Task DeleteTeamAppAsync(int id)
        {
            await _teamService.DeleteTeamAsync(id);
        }

        public async Task<IEnumerable<TeamDTO>> GetByAdvisorIdTeamAppAsync(int advisorId)
        {
            var teams = await _teamService.GetByAdvisorIdTeamAsync(advisorId);
            return _mapper.Map<IEnumerable<TeamDTO>>(teams);
        }

    }
}
