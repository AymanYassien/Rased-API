using System.Linq.Expressions;
using System.Net.Mail;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;
using Attachment = Rased.Infrastructure.Attachment;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public interface IAttachmentRepository : IRepository_Test<Attachment, int>
{
    Task<Attachment?> GetAttachmentByExpenseId(int expenseId, Expression<Func<Attachment, bool>>[]? filter = null);
    Task<string?> GetFilePath(int attachmentId);
    Task<string?> GetFileType(int attachmentId);
    Task<long?> GetFileSize(int attachmentId);
    Task<DateTime?> GetUploadDate(int attachmentId);
    
}