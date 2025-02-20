namespace Rased.Infrastructure.Models.Saving
{
    public class Saving
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsSaving { get; set; }     // Y(Yes) - N(No) 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int WalletId { get; set; }
        public int CatId { get; set; }

        // Navigation Properties
        //public Wallet Wallet { get; set; } = null!;
        public SavingCategory SavingCategories { get; set; } = new SavingCategory();
    }
}
