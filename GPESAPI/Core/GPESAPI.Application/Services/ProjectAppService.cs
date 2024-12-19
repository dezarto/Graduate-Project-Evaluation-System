using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Services;

namespace GPESAPI.Application.Services
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IProfessorsUsersService _professorsUsersService;
        private readonly IProfessorService _professorService;
        private readonly ITeamService _teamService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IProfessorAvailabilityService _professorAvailabilityService;
        private readonly ITeamPresentationService _teamPresentationService;
        private readonly IEvaluationService _evaluationService;
        private readonly IEvaluationCriteriaDetailService _evaluationCriteriaDetailService;
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ProjectAppService(IProjectService projectService, IUserService userService, IProfessorsUsersService professorsUsersService, ITeamService teamService, IProfessorAvailabilityService professorAvailabilityService, ITeamPresentationService teamPresentationService, IProfessorService professorService, ITeamMemberService teamMemberService, IEvaluationService evaluationService, IEvaluationCriteriaDetailService evaluationCriteriaDetailService, IReportService reportService , IMapper mapper)
        {
            _projectService = projectService;
            _userService = userService;
            _professorsUsersService = professorsUsersService;
            _teamService = teamService;
            _professorAvailabilityService = professorAvailabilityService;
            _teamPresentationService = teamPresentationService;
            _professorService = professorService;
            _teamMemberService = teamMemberService;
            _evaluationService = evaluationService;
            _evaluationCriteriaDetailService = evaluationCriteriaDetailService;
            _reportService = reportService;
            _mapper = mapper;
        }

        public async Task<StudentProjectTeams> StudentProjectTeamView(string studentNumber)
        {
            var student = await _userService.GetByStudentNumberAsync(studentNumber);
            var teamId = await _teamMemberService.GetByUserIdAsync(student.UserId);

            if (teamId != null)
            {
                var team = await _teamService.GetTeamByIdAsync(teamId.TeamId);
                var project = await _projectService.GetProjectByIdAsync(team.ProjectId);
                var teamMembers = await _teamMemberService.GetByTeamIdAsync(team.TeamId);
                var report = await _reportService.GetReportByTeamId(teamId.TeamId);
                var presentations = await _teamPresentationService.GetTeamPresentationByTeamIdAsync(teamId.TeamId);

                if (report == null && presentations == null) { 
                    var newStudentProjectTeams = new StudentProjectTeams
                    {
                        TeamId = team.TeamId,
                        ProjectId = team.ProjectId,
                        AdvisorId = team.AdvisorId,
                        isActive = team.isActive,
                        TeamName = team.TeamName,
                        ProjectName = project.ProjectName,
                        Description = project.Description,
                        Members = new List<GPESAPI.Application.DTOs.MemberList>(),
                        ReportId = null,
                        PresentationDate = null,
                        EndTime = null,
                        StartTime = null,
                    };

                    if (teamMembers != null && teamMembers.Count > 0)
                    {
                        foreach (var teamMember in teamMembers)
                        {
                            var studentInfos = await _userService.GetByUserIdAsync(teamMember.UserId);

                            var studentDatas = new GPESAPI.Application.DTOs.MemberList
                            {
                                StudentId = teamMember.UserId,
                                StudentFullName = studentInfos.FullName,
                                StudentNumber = studentInfos.StudentNumber,
                            };

                            newStudentProjectTeams.Members.Add(studentDatas);
                        }
                    }

                    return newStudentProjectTeams;
                }
                else if(presentations == null && report != null)
                {
                    var newStudentProjectTeams = new StudentProjectTeams
                    {
                        TeamId = team.TeamId,
                        ProjectId = team.ProjectId,
                        AdvisorId = team.AdvisorId,
                        isActive = team.isActive,
                        TeamName = team.TeamName,
                        ProjectName = project.ProjectName,
                        Description = project.Description,
                        Members = new List<GPESAPI.Application.DTOs.MemberList>(),
                        ReportId = report.ReportId,
                        PresentationDate = null,
                        StartTime = null,
                        EndTime = null,
                    };

                    if (teamMembers != null && teamMembers.Count > 0)
                    {
                        foreach (var teamMember in teamMembers)
                        {
                            var studentInfos = await _userService.GetByUserIdAsync(teamMember.UserId);

                            var studentDatas = new GPESAPI.Application.DTOs.MemberList
                            {
                                StudentId = teamMember.UserId,
                                StudentFullName = studentInfos.FullName,
                                StudentNumber = studentInfos.StudentNumber,
                            };

                            newStudentProjectTeams.Members.Add(studentDatas);
                        }
                    }

                    return newStudentProjectTeams;
                }
                else if(report == null && presentations != null)
                {
                    var newStudentProjectTeams = new StudentProjectTeams
                    {
                        TeamId = team.TeamId,
                        ProjectId = team.ProjectId,
                        AdvisorId = team.AdvisorId,
                        isActive = team.isActive,
                        TeamName = team.TeamName,
                        ProjectName = project.ProjectName,
                        Description = project.Description,
                        Members = new List<GPESAPI.Application.DTOs.MemberList>(),
                        ReportId = null,
                        PresentationDate = presentations.PresentationDate,
                        StartTime = presentations.StartTime,
                        EndTime = presentations.EndTime,
                    };

                    if (teamMembers != null && teamMembers.Count > 0)
                    {
                        foreach (var teamMember in teamMembers)
                        {
                            var studentInfos = await _userService.GetByUserIdAsync(teamMember.UserId);

                            var studentDatas = new GPESAPI.Application.DTOs.MemberList
                            {
                                StudentId = teamMember.UserId,
                                StudentFullName = studentInfos.FullName,
                                StudentNumber = studentInfos.StudentNumber,
                            };

                            newStudentProjectTeams.Members.Add(studentDatas);
                        }
                    }

                    return newStudentProjectTeams;
                }
                else if(report != null && presentations != null)
                {
                    var newStudentProjectTeams = new StudentProjectTeams
                    {
                        TeamId = team.TeamId,
                        ProjectId = team.ProjectId,
                        AdvisorId = team.AdvisorId,
                        isActive = team.isActive,
                        TeamName = team.TeamName,
                        ProjectName = project.ProjectName,
                        Description = project.Description,
                        Members = new List<GPESAPI.Application.DTOs.MemberList>(),
                        ReportId = report.ReportId,
                        PresentationDate = presentations.PresentationDate,
                        StartTime = presentations.StartTime,
                        EndTime = presentations.EndTime,
                    };

                    if (teamMembers != null && teamMembers.Count > 0)
                    {
                        foreach (var teamMember in teamMembers)
                        {
                            var studentInfos = await _userService.GetByUserIdAsync(teamMember.UserId);

                            var studentDatas = new GPESAPI.Application.DTOs.MemberList
                            {
                                StudentId = teamMember.UserId,
                                StudentFullName = studentInfos.FullName,
                                StudentNumber = studentInfos.StudentNumber,
                            };

                            newStudentProjectTeams.Members.Add(studentDatas);
                        }
                    }

                    return newStudentProjectTeams;
                }
                else
                {
                    var newStudentProjectTeams = new StudentProjectTeams
                    {
                        TeamId = null,
                        ProjectId = null,
                        AdvisorId = null,
                        isActive = null,
                        TeamName = null,
                        ProjectName = null,
                        Description = null,
                        Members = null,
                        PresentationDate = null,
                        EndTime = null,
                        ReportId = null,
                        StartTime = null,
                    };

                    return newStudentProjectTeams;
                }
            }
            else
            {
                var newStudentProjectTeams = new StudentProjectTeams
                {
                    TeamId = null,
                    ProjectId = null,
                    AdvisorId = null,
                    isActive = null,
                    TeamName = null,
                    ProjectName = null,
                    Description = null,
                    Members = null,
                    PresentationDate= null,
                    EndTime= null,
                    ReportId = null,
                    StartTime = null,
                };

                return newStudentProjectTeams;
            }
        }

        public async Task<List<ProjectTeams>> ProfessorProjectTeamView(string professorMail)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(professorMail);

            var presentations = await _teamPresentationService.GetTeamPresentationByIdAsync(professor.ProfessorId);

            if (presentations == null || presentations.Count == 0)
            {
                throw new Exception("No presentations found for the given professor ID.");
            }

            var projectTeamsList = new List<ProjectTeams>();

            foreach (var presentation in presentations)
            {
                var project = await _projectService.GetProjectByIdAsync(presentation.ProjectId);
                var team = await _teamService.GetTeamByIdAsync(presentation.TeamId);
                var professorInfos = await _professorService.GetByProfessorIdAsync(professor.ProfessorId);
                var teamMembers = await _teamMemberService.GetByTeamIdAsync(presentation.TeamId);
                
                var newProjectTeams = new ProjectTeams
                {
                    TeamPresentationId = presentation.TeamPresentationId,
                    TeamId = presentation.TeamId,
                    ProjectName = project.ProjectName,
                    ProjectId = project.ProjectId,
                    Description = project.Description,
                    TeamName = team.TeamName,
                    isEvaluated = presentation.isEvaluated,
                    EvaluatingTeacherFullName = professorInfos.FullName,
                    EvaluatingTeacherMail = professorInfos.mailAddress,
                    isApproval = team.isActive,
                    PresentationDate = presentation.PresentationDate,
                    StartTime = presentation.StartTime,
                    EndTime = presentation.EndTime,
                    IsActive = team.isActive,
                    Members = new List<StudentList>()
                };

                if (teamMembers != null && teamMembers.Count > 0)
                {
                    foreach (var teamMember in teamMembers)
                    {
                        var studentInfos = await _userService.GetByUserIdAsync(teamMember.UserId);

                        var student = new StudentList
                        {
                            StudentId = teamMember.UserId,
                            StudentFullName = studentInfos.FullName,
                            StudentNumber = studentInfos.StudentNumber,
                        };

                        newProjectTeams.Members.Add(student);
                    }
                }
                projectTeamsList.Add(newProjectTeams);
            }

            return projectTeamsList;
        }

        public async Task<ProjectTeamResult> ProfessorProjectTeamResult(string professorMail, int teamId)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(professorMail);
            if (professor == null)
            {
                throw new Exception($"Professor with email {professorMail} not found.");
            }

            var teamPresentation = await _teamPresentationService.GetTeamPresentationByTeamIdAsync(teamId);
            if (teamPresentation == null)
            {
                throw new Exception($"Team presentation for Team ID {teamId} not found.");
            }

            var team = await _teamService.GetTeamByIdAsync(teamPresentation.TeamId);
            if (team == null)
            {
                throw new Exception($"Team with ID {teamPresentation.TeamId} not found.");
            }

            var teamMemberDomain = new List<StudentLists>();

            var teamMembers = await _teamMemberService.GetByTeamIdAsync(teamId);

            if (teamMembers == null)
            {
                throw new Exception($"Project with ID {team.ProjectId} Teammember error.");
            }

            foreach (var item in teamMembers)
            {
                var user = await _userService.GetByUserIdAsync(item.UserId);

                if (user == null)
                {
                    continue;
                }

                teamMemberDomain.Add(new StudentLists
                {
                    StudentId = user.UserId,
                    StudentFullName = user.FullName,
                    StudentNumber = user.StudentNumber,
                });
            }

            var project = await _projectService.GetProjectByIdAsync(team.ProjectId);
            if (project == null)
            {
                throw new Exception($"Project with ID {team.ProjectId} not found.");
            }

            var evaluations = await _evaluationService.GetEvaluationByTeamIdAsync(teamId);

            var professorsTeams = new List<ProfessorList>();

            foreach (var professorId in new[] { teamPresentation.AdvisorId, teamPresentation.Professor1Id, teamPresentation.Professor2Id })
            {
                var prof = await _professorService.GetByProfessorIdAsync(professorId);
                if (prof == null)
                {
                    continue;
                }

                var professorEvaluations = evaluations
                    .Where(e => e.ProfessorId == prof.ProfessorId)
                    .ToList();

                double averageScore = professorEvaluations.Any()
                    ? professorEvaluations.Average(e => e.EvaluationScore)
                    : 0.0;

                string generalComments = string.Join(" | ", professorEvaluations
                    .Where(e => !string.IsNullOrWhiteSpace(e.GeneralComments))
                    .Select(e => e.GeneralComments));

                var evaluationCriteriaResultList = new List<EvaluationCriteriaResult>();

                foreach (var evaluation in professorEvaluations)
                {
                    var evaluationCriterias = await _evaluationCriteriaDetailService.GetEvaluationCriteriaDetailByIdAsync(evaluation.EvaluationId);
                    if (evaluationCriterias != null)
                    {
                        evaluationCriteriaResultList.AddRange(evaluationCriterias.Select(ec => new EvaluationCriteriaResult
                        {
                            CriteriaId = ec.CriteriaId,
                            Feedback = ec.Feedback,
                            isChecked = ec.isChecked,
                            Score = ec.Score,
                        }));
                    }
                }

                professorsTeams.Add(new ProfessorList
                {
                    ProfessorId = prof.ProfessorId,
                    FullName = prof.FullName,
                    mailAddress = prof.mailAddress,
                    EvaluationScore = averageScore,
                    GeneralComments = generalComments,
                    EvaluationCriterias = evaluationCriteriaResultList,
                });
            }

            

            return new ProjectTeamResult
            {
                TeamId = teamId,
                TeamName = team.TeamName,
                ProjectName = project.ProjectName,
                ProjectDescription = project.Description,
                ProfessorsTeams = professorsTeams,
                Members = teamMemberDomain
            };
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
