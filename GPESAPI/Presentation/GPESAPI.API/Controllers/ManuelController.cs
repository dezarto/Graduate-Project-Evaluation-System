using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduateProjectEvaluationSystemAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuelController : ControllerBase
    {
        private readonly IProfessorsUsersAppService _professorsUsersAppService;
        private readonly IUserAppService _userAppService;

        public ManuelController(IProfessorsUsersAppService professorsUsersAppService, IUserAppService userAppService)
        {
            _professorsUsersAppService = professorsUsersAppService;
            _userAppService = userAppService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUsersWithProfessors()
        {
            try
            {
                var usersWithProfessors = await _userAppService.GetUsersWithProfessorsAppAsync();

                foreach (var user in usersWithProfessors)
                {
                    bool exists = await _professorsUsersAppService.ProfessorsUsersExistsAppAsync(user.ProfessorId, user.UserId);
                    if (!exists)
                    {
                        await _professorsUsersAppService.AddProfessorsUsersAppAsync(user.ProfessorId, user.UserId);
                    }
                }

                return Ok(new { message = "Users synchronized successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while syncing users.", error = ex.Message });
            }
        }
    }
}
