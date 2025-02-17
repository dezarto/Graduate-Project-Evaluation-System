﻿using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITeamMemberAppService
    {
        Task<IEnumerable<TeamMemberDTO>> GetAllTeamMemberAppAsync();
        Task<TeamMemberDTO> GetTeamMemberByUserIdAsync(int userId);
        Task<List<TeamMemberDTO>> GetTeamMemberByTeamIdAsync(int teamId);
        Task AddTeamMemberAppAsync(TeamMemberDTO teamMemberDto);
        Task UpdateTeamMemberAppAsync(TeamMemberDTO teamMemberDto);
        Task DeleteTeamMemberAppAsync(int teamId);
    }
}
