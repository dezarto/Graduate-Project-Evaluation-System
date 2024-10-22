using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(LoginRequestDTO loginRequestDto);
        RefreshToken GenerateRefreshToken();
    }
}
