using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.RecommendSystem;
using System.Security.Claims;
using System.Text.Json;

namespace Rased.Api.Controllers.Recomm
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiRecommendationController : ControllerBase
    {
        private readonly IAiRecommendationService _recommendationService;

        public AiRecommendationController(IAiRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }


        [HttpGet("wallet/{walletId}/recommendation")]
        [Authorize]
        public async Task<IActionResult> GetRecommendation(int walletId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<List<RecommendationTipDto>>
                    {
                        Succeeded = false,
                        Message = "المستخدم غير مصرح له"
                    });
                }

                // التحقق من صحة walletId
                if (walletId <= 0)
                {
                    return BadRequest(new ApiResponse<List<RecommendationTipDto>>
                    {
                        Succeeded = false,
                        Message = "معرف المحفظة غير صحيح"
                    });
                }

                var recommendations = await _recommendationService.GetRecommendationAsync(walletId, userId);

                // التحقق من وجود توصيات
                if (recommendations == null || !recommendations.Any())
                {
                    return Ok(new ApiResponse<List<RecommendationTipDto>>
                    {
                        Succeeded = true,
                        Data = new List<RecommendationTipDto>(),
                        Message = "لا توجد بيانات كافية لإنشاء توصيات حالياً"
                    });
                }

                return Ok(new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = true,
                    Data = recommendations,
                    Message = $"تم الحصول على {recommendations.Count} توصية بنجاح"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = ex.Message
                });
            }
            catch (HttpRequestException ex)
            {
                // خطأ في الاتصال بـ Gemini API
                return StatusCode(503, new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = "خدمة التوصيات غير متاحة حالياً، يرجى المحاولة لاحقاً"
                });
            }
            catch (TaskCanceledException ex)
            {
                // انتهاء مهلة الطلب
                return StatusCode(408, new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = "انتهت مهلة انتظار الطلب، يرجى المحاولة مرة أخرى"
                });
            }
            catch (JsonException ex)
            {
                // خطأ في تحليل استجابة Gemini
                return StatusCode(502, new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = "خطأ في معالجة استجابة خدمة التوصيات"
                });
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ (لو عندك logging)
                // _logger.LogError(ex, "Error getting recommendations for wallet {WalletId} and user {UserId}", walletId, userId);

                return StatusCode(500, new ApiResponse<List<RecommendationTipDto>>
                {
                    Succeeded = false,
                    Message = "حدث خطأ غير متوقع أثناء استدعاء التوصيات"
                });
            }
        }



    }

}
