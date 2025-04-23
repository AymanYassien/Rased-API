using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Bills;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.Bills;
using Rased.Business.Services.ExpenseService;

namespace Rased.Api.Controllers.Bills
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IAttachmentService _attachmentService;

        public BillController(IBillService billService , IAttachmentService attachmentService)
        {
            _billService = billService;
            _attachmentService = attachmentService;
        }

        // ميثود لتحليل البيانات من الصورة
        [HttpPost("scan")]
        public async Task<IActionResult> ScanBill([FromForm] BillScanRequestDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest(new ApiResponse<BillDtos>("Please upload a valid image."));

            var result = await _billService.ExtractBillDataFromImageAsync(dto.ImageFile);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        // ميثود لحفظ مسودة الفاتورة
        [HttpPost("save-draft")]
        public async Task<IActionResult> SaveBillDraft([FromForm] SaveBillDraftDto draftDto)
        {
            // التحقق من وجود الصورة في الـ DTO
            if (draftDto.ImageFile == null || draftDto.ImageFile.Length == 0)
                return BadRequest(new ApiResponse<int>("Please upload a valid image."));

            // إرسال البيانات إلى الخدمة لحفظ المسودة
            var result = await _billService.SaveBillDraftAsync(draftDto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new ApiResponse<int>(result.Data, "Bill draft saved successfully."));
        }

        [HttpGet("draft/{id}")]
        public async Task<IActionResult> GetDraftBillById(int id)
        {
            var result = await _billService.GetDraftBillByIdAsync(id);

            if (!result.Succeeded)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("add-expense")]
        public async Task<IActionResult> AddBillExpense([FromForm] AddBillExpenseDto dto)
        {
            // التحقق من وجود BillDraftId
            if (dto.BillDraftId == 0)
                return BadRequest(new ApiResponse<int>("BillDraftId is required."));

            // إرسال البيانات إلى الخدمة لإضافة المصروف
            var result = await _billService.AddBillExpenseAsync(dto);

            // إذا فشلت العملية، أرسل الأخطاء للمستخدم
            if (!result.Succeeded)
                return BadRequest(new ApiResponse<int>(result.Errors));

            // إذا تم بنجاح، أرسل الـ BillExpenseId
            return Ok(new ApiResponse<int>(result.Data, "Bill expense added successfully."));
        }


        [HttpGet("draft/{draftId}/attachment")]
        public async Task<IActionResult> GetAttachmentByDraftId(int draftId)
        {
            var result = await _attachmentService.GetAttachmentByDraftId(draftId);

            if (!result.Succeeded)
                return NotFound(new ApiResponse<string>(result.Errors));

            return Ok(new ApiResponse<object>(result.Data, "Attachment retrieved successfully."));
        }




    }
}
