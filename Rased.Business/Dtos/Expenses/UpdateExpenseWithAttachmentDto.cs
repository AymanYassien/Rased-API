using Microsoft.AspNetCore.Http;

namespace Rased.Business.Dtos;

public class UpdateExpenseWithAttachmentDto
{
    public UpdateExpenseDto Expense { get; set; }
    public IFormFile? Attachment { get; set; } 
}