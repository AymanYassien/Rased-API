using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rased.Business;
using Rased.Business.IncomesConfigures;
using Rased.Infrastructure.Data.Config.Debts;
using Rased.Infrastructure.Data.Config.Extras;
using Rased.Infrastructure.Data.Config.Goals;
using Rased.Infrastructure.Data.Config.Preferences;
using Rased.Infrastructure.Data.Config.Savings;
using Rased.Infrastructure.Data.Config.Subscriptions;
using Rased.Infrastructure.Data.Config.User;
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Preferences;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.Subscriptions;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Data
{
    public class RasedDbContext: IdentityDbContext<RasedUser>
    {
        public RasedDbContext(DbContextOptions<RasedDbContext> options) : base(options) { }

        // DbSets ...
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<StaticBudgetTypesData> StaticBudgetTypes { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseTemplate> ExpenseTemplates { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<StaticPaymentMethodsData> StaticPaymentMethods { get; set; }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<StaticFriendRequestStatusData> StaticFriendRequestStatus { get; set; }
        public DbSet<StaticFriendshipStatusData> StaticFriendshipStatus { get; set; }

        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalTransaction> GoalTransactions { get; set; }

        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeTemplate> IncomeTemplates { get; set; }
        public DbSet<StaticIncomeSourceTypeData> StaticIncomeSourceTypes { get; set; }

        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }

        public DbSet<Saving> Savings { get; set; }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletStatistics> WalletStatistics { get; set; }

        public DbSet<SharedWallet> SharedWallets { get; set; }
        public DbSet<SharedWalletMembers> SharedWalletMembers { get; set; }
        public DbSet<StaticSharedWalletAccessLevelData> StaticSharedWalletAccessLevels { get; set; }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDetail> PlanDetails { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        
        public DbSet<ExpenseTransactionRecord> ExpenseTransactionRecords { get; set; }
        public DbSet<PersonalIncomeTrasactionRecord> PersonalIncomeTrasactionRecords { get; set; }
        public DbSet<SharedWalletIncomeTransaction> SharedWalletIncomeTransactions { get; set; }
        public DbSet<StaticReceiverTypeData> StaticReceiverTypes { get; set; }
        public DbSet<StaticTransactionStatusData> StaticTransactionStatus { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionApproval> TransactionApprovals { get; set; }

        public DbSet<AutomationRule> AutomationRules { get; set; }
        public DbSet<StaticColorTypeData> StaticColorTypes { get; set; }
        public DbSet<StaticDaysOfWeekNamesData> StaticDaysOfWeekNames { get; set; }
        public DbSet<StaticThresholdTypeData> StaticThresholdTypes { get; set; }
        public DbSet<StaticTriggerTypeData> StaticTriggerTypes { get; set; }
        public DbSet<StaticWalletStatusData> StaticWalletStatus { get; set; }


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // This is for Saving Folder
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FriendRequestConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoalConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IncomeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserPreferenceConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SavingConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SharedWalletConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlanConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTransactionRecordConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RasedUserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutomationRuleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletConfiguration).Assembly);
        }
    }
}
