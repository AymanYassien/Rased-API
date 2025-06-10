using Microsoft.AspNetCore.Http;

namespace Rased.Business.Dtos;

public class AddExpenseWithAttachmentDto
{
    public AddExpenseDto Expense { get; set; }
    public IFormFile? Attachment { get; set; }
}