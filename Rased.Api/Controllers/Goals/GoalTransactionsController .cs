using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.Goals;

namespace Rased.Api.Controllers.Goals
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GoalTransactionsController : ControllerBase
    {
        private readonly IGoalTransactionService _goalTransactionService;

        public GoalTransactionsController(IGoalTransactionService goalTransactionService)
        {
            _goalTransactionService = goalTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGoalTransactionsAsync()
        {
            var response = await _goalTransactionService.GetAllGoalsTransactionAsync();
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetGoalTransactionByIdAsync(int id)
        {
            var response = await _goalTransactionService.GetGoalTransactionByIdAsync(id);
            return response.Succeeded ? Ok(response.Data) : NotFound(response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoalTransactionAsync([FromBody] AddGoalTransactionDto addGoalTransactionDto)
        {
            var response = await _goalTransactionService.AddGoalTransactionAsync(addGoalTransactionDto);
            return response.Succeeded ? Ok(response.Message) : BadRequest(response.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoalTransactionAsync(int id, [FromBody] UpdateGoalTransactionDto updateGoalTransactionDto)
        {
            if (id != updateGoalTransactionDto.Id)
                return BadRequest(new ApiResponse<string>("ID mismatch."));

            var response = await _goalTransactionService.UpdateGoalTransactionAsync(updateGoalTransactionDto);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoalTransactionAsync(int id)
        {
            var response = await _goalTransactionService.DeleteGoalTransactionAsync(id);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }

        [HttpGet("by-goal/{goalId}")]
        public async Task<IActionResult> GetTransactionsByGoalIdAsync(int goalId)
        {
            var response = await _goalTransactionService.GetTransactionsByGoalIdAsync(goalId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetTransactionsByWalletIdAsync(int walletId)
        {
            var response = await _goalTransactionService.GetTransactionsByWalletIdAsync(walletId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("goal/{goalId}/is-completed")]
        public async Task<IActionResult> IsGoalCompletedAsync(int goalId)
        {
            var response = await _goalTransactionService.IsGoalCompletedAsync(goalId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("goal/{goalId}/total-saved")]
        public async Task<IActionResult> GetTotalSavedAmountByDateRangeAsync(int goalId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _goalTransactionService.GetTotalSavedAmountByDateRangeAsync(goalId, startDate, endDate);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }


    }

}
