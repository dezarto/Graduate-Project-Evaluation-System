using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduateProjectEvaluationSystemAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : Controller
    {
        private readonly IProfessorAppService _professorAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;

        public ProfessorController(IProfessorAppService professorAppService, IProfessorAvailabilityAppService professorAvailabilityAppService)
        {
            _professorAppService = professorAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
        }

        [HttpPost]
        public async Task<ActionResult<ProfessorDTO>> CreateProfessor([FromBody] ProfessorDTO professorDto)
        {
            await _professorAppService.AddProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfessorDTO>>> GetAllProfessors()
        {
            return await _professorAppService.GetAllProfessorAppAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfessorDTO>> GetProfessorById(int id)
        {
            var professor = await _professorAppService.GetByProfessorAppIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return professor;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorDTO professorDto)
        {
            if (id != professorDto.ProfessorId)
            {
                return BadRequest();
            }

            await _professorAppService.UpdateProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            await _professorAppService.DeleteProfessorAppAsync(id);
            return Ok("Successful");
        }

        [HttpPost("{professorId}/availability")]
        public async Task<ActionResult> AddProfessorAvailability(int professorId, [FromBody] List<ProfessorAvailabilityDTO> availabilities)
        {
            try
            {
                foreach (var availabilityDto in availabilities)
                {
                    // ProfessorId zaten mevcut, her availability kaydında set ediyoruz
                    availabilityDto.ProfessorId = professorId;

                    // 1. Adım: Veritabanında belirtilen tarih ve saat aralığında bir kaydın olup olmadığını kontrol ediyoruz
                    var existingAvailability = await _professorAvailabilityAppService.CheckExistingAvailabilityAppAsync(
                        professorId, availabilityDto.AvailableDate, availabilityDto.StartTime, availabilityDto.EndTime);

                    if (existingAvailability)
                    {
                        // Eğer aynı zaman diliminde zaten bir kayıt varsa, hata döndürüyoruz
                        return Conflict(new { message = "An availability record already exists for the specified date and time range." });
                    }

                    // 2. Adım: Eğer kayıt yoksa, Availability kaydediliyor
                    await _professorAvailabilityAppService.AddProfessorAvailabilityAppAsync(availabilityDto);
                }

                return Ok(new { message = "Availability data added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding availability data.", error = ex.Message });
            }
        }
    }
}
