namespace Rased.Business.Dtos.SharedWallets
{
    public class ReadSharedWalletDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Relations
        public string? UserAccessLvl { get; set; }
        public string? Status { get; set; }
        public string? Color { get; set; }
        public string? Currency { get; set; }
    }
}
