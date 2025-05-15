using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(LoginRequestDTO loginRequestDto, string role);
        RefreshToken GenerateRefreshToken();
    }
}
