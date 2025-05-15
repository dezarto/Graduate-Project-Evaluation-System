using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPESAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Professor")]
    public class AdminController : Controller
    {
        private readonly IProfessorAppService _professorAppService;
        private readonly IUserAppService _userAppService;
        private readonly IEvaluationCriteriaAppService _evaluationCriteriaAppService;
        private readonly IChecklistItemsAppService _checklistItemsAppService;
        private readonly ITeamAppService _teamAppService;
        public AdminController(IProfessorAppService professorAppService, IUserAppService userAppService, IEvaluationCriteriaAppService evaluationCriteriaService, IEvaluationCriteriaAppService evaluationCriteriaAppService, IChecklistItemsAppService checklistItemsAppService, ITeamAppService teamAppService)
        {
            _professorAppService = professorAppService;
            _userAppService = userAppService;
            _evaluationCriteriaAppService = evaluationCriteriaAppService;
            _checklistItemsAppService = checklistItemsAppService;
            _teamAppService = teamAppService;
        }
        //Professor endpoints
        [HttpPost("create-professor")]
        public async Task<ActionResult<ProfessorDTO>> CreateProfessor([FromBody] ProfessorDTO professorDto)
        {
            await _professorAppService.AddProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [HttpGet("get-all-professor")]
        public async Task<ActionResult<List<ProfessorDTO>>> GetAllProfessors()
        {
            return await _professorAppService.GetAllProfessorAppAsync();
        }

        [HttpGet("get-professor-by-id/{id}")]
        public async Task<ActionResult<ProfessorDTO>> GetProfessorById(int id)
        {
            var professor = await _professorAppService.GetByProfessorAppIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return professor;
        }

        [HttpPut("update-professor-by-id/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorDTO professorDto)
        {
            if (id != professorDto.ProfessorId)
            {
                return BadRequest();
            }

            await _professorAppService.UpdateProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [HttpDelete("delete-professor-by-id/{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            await _professorAppService.DeleteProfessorAppAsync(id);
            return Ok("Successful");
        }
        //Student endpoints
        [HttpPost("create-student")]
        public async Task<ActionResult<UserDTO>> CreateStudent([FromBody] UserDTO userDto)
        {
            await _userAppService.AddUserAppAsync(userDto);
            return Ok(userDto);
        }

        [HttpGet("get-all-student")]
        public async Task<ActionResult<List<UserDTO>>> GetAllStudent()
        {
            return await _userAppService.GetAllUserAppAsync();
        }

        [HttpGet("get-student-by-id/{id}")]
        public async Task<ActionResult<UserDTO>> GetStudentById(int id)
        {
            var customer = await _userAppService.GetByUserAppIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UserDTO userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }

            await _userAppService.UpdateUserAppAsync(userDto);
            return Ok(userDto);
        }

        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _userAppService.DeleteUserAppAsync(id);
            return Ok("Successful");
        }

        //Evaluation Criteria endpoints
        [HttpGet("get-all-evaluation-criteria")]
        public async Task<IActionResult> GetAllEvaluationCriteria()
        {
            var criteria = await _evaluationCriteriaAppService.GetAllEvaluationCriteriaAsync();
            return Ok(criteria);
        }

        [HttpGet("get-evaluation-criteria-by-id/{id}")]
        public async Task<IActionResult> GetEvaluationCriteriaById(int id)
        {
            var criteria = await _evaluationCriteriaAppService.GetByEvaluationCriteriaIdAsync(id);

            return Ok(criteria);
        }

        [HttpPost("post-add-evaluation-criteria")]
        public async Task<IActionResult> AddEvaluationCriteria([FromBody] EvaluationCriteriaDTO evaluationCriteria)
        {
            if (evaluationCriteria == null)
                return BadRequest("Evaluation criteria is null.");

            await _evaluationCriteriaAppService.AddEvaluationCriteriaAsync(evaluationCriteria);

            return CreatedAtAction(nameof(GetEvaluationCriteriaById), new { id = evaluationCriteria.CriteriaId }, evaluationCriteria);
        }

        [HttpPut("put-update-evaluation-criteria/{id}")]
        public async Task<IActionResult> UpdateEvaluationCriteria(int id, [FromBody] EvaluationCriteriaDTO evaluationCriteria)
        {
            if (evaluationCriteria == null || id != evaluationCriteria.CriteriaId)
                return BadRequest("Invalid data.");

            await _evaluationCriteriaAppService.UpdateEvaluationCriteriaAsync(id, evaluationCriteria);

            return NoContent();
        }

        [HttpDelete("delete-evaluation-criteria/{id}")]
        public async Task<IActionResult> DeleteEvaluationCriteria(int id)
        {
            await _evaluationCriteriaAppService.DeleteEvaluationCriteriaAsync(id);

            return NoContent();
        }

        // GET: api/ChecklistItems/get-all-checklist-items
        [HttpGet("get-all-checklist-items")]
        public async Task<IActionResult> GetAllChecklistItems()
        {
            var items = await _checklistItemsAppService.GetAllChecklistItemsAsync();
            return Ok(items);
        }

        // GET: api/ChecklistItems/get-checklist-item-by-id/{id}
        [HttpGet("get-checklist-item-by-id/{id}")]
        public async Task<IActionResult> GetChecklistItemById(int id)
        {
            var item = await _checklistItemsAppService.GetByChecklistItemsIdAsync(id);
            return Ok(item);
        }

        // POST: api/ChecklistItems/post-add-checklist-item
        [HttpPost("post-add-checklist-item")]
        public async Task<IActionResult> AddChecklistItem([FromBody] ChecklistItemDTO checklistItem)
        {
            if (checklistItem == null)
                return BadRequest("Checklist item is null.");

            await _checklistItemsAppService.AddChecklistItemsAsync(checklistItem);
            return CreatedAtAction(nameof(GetChecklistItemById), new { id = checklistItem.ItemId }, checklistItem);
        }

        // PUT: api/ChecklistItems/put-update-checklist-item/{id}
        [HttpPut("put-update-checklist-item/{id}")]
        public async Task<IActionResult> UpdateChecklistItem(int id, [FromBody] ChecklistItemDTO checklistItem)
        {
            if (checklistItem == null || id != checklistItem.ItemId)
                return BadRequest("Invalid data.");

            await _checklistItemsAppService.UpdateChecklistItemsAsync(id, checklistItem);
            return NoContent();
        }

        // DELETE: api/ChecklistItems/delete-checklist-item/{id}
        [HttpDelete("delete-checklist-item/{id}")]
        public async Task<IActionResult> DeleteChecklistItem(int id)
        {
            await _checklistItemsAppService.DeleteChecklistItemsAsync(id);
            return NoContent();
        }

        [HttpGet("get-all-teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamAppService.GetAllTeamAppAsync();
            return Ok(teams);
        }

        [HttpDelete("delete-team-by-id/{id}")]
        public async Task<IActionResult> DeleteTeamById(int id)
        {
            await _teamAppService.DeleteTeamAppAsync(id);
            return Ok();
        }
    }
}
