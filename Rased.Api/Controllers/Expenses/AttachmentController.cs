using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;


namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/user/wallet/attachment")]
[Authorize]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAttachmentByExpenseId(
        [FromQuery] int expenseId,
        [FromQuery] string filter = null) // e.g., "fileType=pdf,size>1024"
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Attachment>(filter);
        var response = await _attachmentService.GetAttachmentByExpenseId(expenseId, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> AddAttachment(
        [FromBody] AddAttachmentDto newAttachment)
    {
        var response = await _attachmentService.AddAttachment(newAttachment);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{attachmentId}")]
    public async Task<IActionResult> UpdateAttachment(
        [FromRoute] int attachmentId,
        [FromBody] UpdateAttachmentDto updateAttachment)
    {
        var response = await _attachmentService.UpdateAttachment(attachmentId, updateAttachment);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment(
        [FromRoute] int attachmentId)
    {
        var response = await _attachmentService.DeleteAttachment(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/size")]
    public async Task<IActionResult> GetFileSize(
        [FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileSize(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/path")]
    public async Task<IActionResult> GetFilePath(
        [FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFilePath(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/type")]
    public async Task<IActionResult> GetFileType(
        [FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileType(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{attachmentId}/upload-date")]
    public async Task<IActionResult> GetFileUploadDate(
        [FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileUploadDate(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }
}