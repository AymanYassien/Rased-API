using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.RecommendSystem;
using System.Threading.Tasks;

namespace Rased.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetRecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public BudgetRecommendationsController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecommendationsByUserAsync(string userId)
        {
            var response = await _recommendationService.GetRecommendationsByUserAsync(userId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetRecommendationsByWalletAsync(int walletId)
        {
            var response = await _recommendationService.GetRecommendationsByWalletAsync(walletId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("wallet-group/{walletGroupId}")]
        public async Task<IActionResult> GetRecommendationsByWalletGroupAsync(int walletGroupId)
        {
            var response = await _recommendationService.GetRecommendationsByWalletGroupAsync(walletGroupId);
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecommendationAsync([FromBody] CreateBudgetRecommendationDto dto)
        {
            var response = await _recommendationService.CreateRecommendationAsync(dto);
            return response.Succeeded ? Ok(response.Message) : BadRequest(response.Errors);
        }

        [HttpPut("mark-read/{recommendationId}")]
        public async Task<IActionResult> MarkRecommendationAsReadAsync(int recommendationId)
        {
            var response = await _recommendationService.MarkRecommendationAsReadAsync(recommendationId);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }


        // للاختبار فقط - يمكن حذفها في الإنتاج
        [HttpPost("generate-now")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateNow()
        {
            try
            {
                await _recommendationService.GenerateMonthlyRecommendationsAsync();
                return Ok(new ApiResponse<object>
                {
                    Succeeded = true,
                    Message = "تم إنشاء التوصيات بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Succeeded = false,
                    Message = "خطأ في إنشاء التوصيات"
                });
            }
        }
    }
}
