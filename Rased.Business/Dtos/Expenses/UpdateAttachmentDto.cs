namespace Rased.Business.Dtos;

public class UpdateAttachmentDto
{
    public int AttachmentId { get; set; }
    public int ExpenseId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public long? FileSize { get; set; }
}