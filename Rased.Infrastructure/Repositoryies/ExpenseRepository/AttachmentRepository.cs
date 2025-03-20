using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public class AttachmentRepository : Repository_Test<Attachment, int>, IAttachmentRepository
{
    private readonly DbSet<Attachment> _dbSet;
    

    public AttachmentRepository(RasedDbContext context) : base(context)
    {
        _dbSet = context.Set<Attachment>();
    }


    public async Task<Attachment?> GetAttachmentByExpenseId(int expenseId, Expression<Func<Attachment, bool>>[]? filter = null)
    {
        return await GetAsync(new Expression<Func<Attachment, bool>>[]
        {
            x => x.ExpenseId == expenseId
        });
    }

    public async Task<string?> GetFilePath(int attachmentId)
    {
        var attachment = await GetByIdAsync(attachmentId);
        return attachment?.FilePath;
    }

    public async Task<string?> GetFileType(int attachmentId)
    {
        var attachment = await GetByIdAsync(attachmentId);
        return attachment?.FileType;
    }

    public async Task<long?> GetFileSize(int attachmentId)
    {
        var attachment = await GetByIdAsync(attachmentId);
        return attachment?.FileSize;
    }

    public async Task<DateTime?> GetUploadDate(int attachmentId)
    {
        var attachment = await GetByIdAsync(attachmentId);
        return attachment?.UploadDate;
    }
}