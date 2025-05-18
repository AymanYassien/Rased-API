using Rased.Infrastructure.Models.Bills;

namespace Rased.Infrastructure
{

    public class Attachment
    {
        public int AttachmentId { get; set; }

        // Nullable
        public int? ExpenseId { get; set; }
        public int? BillDraftId { get; set; }

        public string? FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public long? FileSize { get; set; }

        
        public virtual Expense? Expense { get; set; }
        public virtual BillDraft? BillDraft { get; set; }
    }

}
