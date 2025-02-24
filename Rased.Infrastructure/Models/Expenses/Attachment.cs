namespace Rased.Infrastructure.Models.Expenses
{

    public class Attachment
    {
        public int AttachmentId { get; set; }
        public int ExpenseId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public long? FileSize { get; set; }

        // Navigation properties
        public virtual Expense Expense { get; set; }
    }
}
