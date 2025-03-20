using System.Linq.Expressions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased.Business.Services.ExpenseService;

public interface IAttachmentService
{
    // Basic CRUD
    //public Task<ApiResponse<object>> GetAllAttachmentsByWalletId(int walletId, Expression<Func<Attachment, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetAttachmentByExpenseId(int expenseId, Expression<Func<Attachment, bool>>[]? filter = null);
    public Task<ApiResponse<object>> AddAttachment(AddAttachmentDto newAttachment);
    public Task<ApiResponse<object>> UpdateAttachment(int attachmentId,UpdateAttachmentDto updateAttachment);
    public Task<ApiResponse<object>> DeleteAttachment(int attachmentId);
    
    // Own Methods
    public Task<ApiResponse<object>> GetFileSize(int attachmentId);
    public Task<ApiResponse<object>> GetFilePath(int attachmentId);
    public Task<ApiResponse<object>> GetFileType(int attachmentId);
    public Task<ApiResponse<object>> GetFileUploadDate(int attachmentId); 
}