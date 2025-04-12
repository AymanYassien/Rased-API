using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Repositoryies.Utility;

using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;
using Rased.Infrastructure.Repositoryies.Wallets;
using Rased.Infrastructure.Repositoryies.Categories;
using Rased.Infrastructure.Repositoryies.SubCategories;
using Rased.Infrastructure.Repositoryies.Goals;
using Rased.Infrastructure.Models.Transfer;

namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IRepositoryies ..
        public  ISavingRepository SavingRepository { get; }
        public IGoalRepository GoalRepository { get; }
        public IGoalTransactionRepository GoalTransactionRepository { get; }
        public IRepository<Transaction , int> Transactions { get; }
        public IRepository<ExpenseTransactionRecord, int> ExpenseTransactionRecords { get; }
        public IRepository<PersonalIncomeTrasactionRecord, int> PersonalIncomeTrasactionRecords { get; }
        public IRepository<SharedWalletIncomeTransaction, int> SharedWalletIncomeTransactions { get; }
        public IRepository<StaticReceiverTypeData, int> StaticReceiverTypes { get; }
        public IRepository<StaticTransactionStatusData, int> StaticTransactionStatus{ get; }
        public IRepository<TransactionApproval, int> TransactionApprovals { get; }
        public IRepository<TransactionRejection, int> TransactionRejections { get; }


        // All System IServices ..
        // IAuthService RasedAuth { get; }
        // ....
        


        public IPaymentMethodRepository PaymentMethods { get; }                
        public IAttachmentRepository Attachments { get; }                


        public IWalletRepository Wallets { get; }
        public IExpensesRepository Expenses { get; }
        public IIncomeRepository Income { get; }

        public ICategoryRepository Categories { get; }
        public ISubCategoryRepository SubCategories { get; }

        public IExpensesRepository Expenses { get; }                
        public IExpenseTemplateRepository ExpenseTemplates{ get; }                
        public IAutomationRuleRepository AutomationRules{ get; }                
        public IBudgetRepository Budget { get; }
        public IIncomeTemplateRepository IncomeTemplate { get; }
        public IStaticIncomeSourceTypeDataRepository StaticIncomeSourceTypeData { get; }
        // Commit Changes
        Task<int> CommitChangesAsync(); // return Affects Row!
    }
}
