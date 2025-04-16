using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;


namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/expense/attachment")]
[Authorize]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    
    [HttpGet("/get-by-expense-id/{expenseId}")]
    public async Task<IActionResult> GetByExpenseId([FromRoute] int expenseId)
    {
        //var filters = ExpressionBuilder.ParseFilter<Infrastructure.Attachment>(filter);
        var response = await _attachmentService.GetAttachmentByExpenseId(expenseId, null);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("{attachmentId}")]
    public async Task<IActionResult> GetById([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetAttachmentById(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddAttachmentDto newAttachment)
    {
        var response = await _attachmentService.AddAttachment(newAttachment);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{attachmentId}")]
    public async Task<IActionResult> Update([FromRoute] int attachmentId, [FromBody] UpdateAttachmentDto updateAttachment)
    {
        var response = await _attachmentService.UpdateAttachment(attachmentId, updateAttachment);
        
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;
        
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> Delete([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.DeleteAttachment(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/size")]
    public async Task<IActionResult> GetFileSize([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileSize(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/path")]
    public async Task<IActionResult> GetFilePath([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFilePath(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{attachmentId}/type")]
    public async Task<IActionResult> GetFileType([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileType(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{attachmentId}/upload-date")]
    public async Task<IActionResult> GetFileUploadDate([FromRoute] int attachmentId)
    {
        var response = await _attachmentService.GetFileUploadDate(attachmentId);
        return StatusCode((int)response.StatusCode, response);
    }
}