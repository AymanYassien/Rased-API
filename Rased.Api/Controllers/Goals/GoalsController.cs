using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.Goals;
using Rased.Infrastructure.Models.Goals;

namespace Rased.Api.Controllers.Goals
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetGoalsByStatusAsync(GoalStatusEnum status)
        {
            var response = await _goalService.GetGoalsByStatusAsync(status);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet/{walletId}/user/{userId}")]
        public async Task<IActionResult> GetGoalsByWalletIdAndUserIdAsync(int walletId, String userId)
        {
            var response = await _goalService.GetGoalsByWalletIdAndUserIdAsync(walletId, userId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("{goalId}/total-saved")]
        public async Task<IActionResult> GetTotalSavedAmountAsync(int goalId)
        {
            var response = await _goalService.GetTotalSavedAmountAsync(goalId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);


        }

        [HttpGet("{goalId}/progress-percentage")]
        public async Task<IActionResult> GetGoalProgressPercentageAsync(int goalId)
        {
            var response = await _goalService.GetGoalProgressPercentageAsync(goalId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet/{walletId}/user/{userId}/progress-percentage")]
        public async Task<IActionResult> GetTotalGoalsProgressPercentageByWalletIdAsync(int walletId, string userId)
        {
            var response = await _goalService.GetTotalGoalsProgressPercentageByWalletIdAsync(walletId, userId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet/{walletId}/user/{userId}/stats")]
        public async Task<IActionResult> GetGoalsStatsByWalletIdAsync(int walletId, string userId)
        {
            var response = await _goalService.GetGoalsStatsByWalletIdAsync(walletId, userId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }



    }
}
