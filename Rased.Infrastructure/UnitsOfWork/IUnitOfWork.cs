using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Repositoryies.Utility;

using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;
using Rased.Infrastructure.Repositoryies.Wallets;

namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IRepositoryies ..
         public  ISavingRepository SavingRepository { get; }
         public IRepository<Goal , int> GoalRepository { get; }

        // All System IServices ..
        // IAuthService RasedAuth { get; }
        // ....
        


        public IPaymentMethodRepository PaymentMethods { get; }                
        public IAttachmentRepository Attachments { get; }                

        public IWalletRepository Wallets { get; }

        public IExpensesRepository Expenses { get; }                
        public IExpenseTemplateRepository ExpenseTemplates{ get; }                
        public IAutomationRuleRepository AutomationRules{ get; }                
        public IIncomeRepository Income { get; }
        public IBudgetRepository Budget { get; }
        public IIncomeTemplateRepository IncomeTemplate { get; }
        public IStaticIncomeSourceTypeDataRepository StaticIncomeSourceTypeData { get; }
        // Commit Changes
        Task<int> CommitChangesAsync(); // return Affects Row!
    }
}
