using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.Goals;

namespace Rased.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGoalsAsync()
        {
            var response = await _goalService.GetAllGoalsAsync();
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGoalByIdAsync(int id)
        {
            var response = await _goalService.GetGoalByIdAsync(id);
            return response.Succeeded ? Ok(response.Data) : NotFound(response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoalAsync([FromBody] AddGoalDto addGoalDto)
        {
            var response = await _goalService.AddGoalAsync(addGoalDto);
            return response.Succeeded ? Ok(response.Message) : BadRequest(response.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoalAsync(int id, [FromBody] UpdateGoalDto updateGoalDto)
        {
            if (id != updateGoalDto.Id)
                return BadRequest(new ApiResponse<string>("ID mismatch."));

            var response = await _goalService.UpdateGoalAsync(updateGoalDto);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoalAsync(int id)
        {
            var response = await _goalService.DeleteGoalAsync(id);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }
    }
}
