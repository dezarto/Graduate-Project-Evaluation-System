using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduateProjectEvaluationSystemAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateCustomer([FromBody] UserDTO userDto)
        {
            await _userAppService.AddUserAppAsync(userDto);
            return Ok(userDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllCustomers()
        {
            return await _userAppService.GetAllUserAppAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetCustomerById(int id)
        {
            var customer = await _userAppService.GetByUserAppIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UserDTO userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }

            await _userAppService.UpdateUserAppAsync(userDto);
            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _userAppService.DeleteUserAppAsync(id);
            return Ok("Successful");
        }
    }
}
