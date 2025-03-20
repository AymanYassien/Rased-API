using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Utility;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;

using Rased.Infrastructure.UnitsOfWork;
using Microsoft.AspNetCore.Identity;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Wallets;

namespace Rased.Infrastructure.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // All IRepositoryies to be injected 
        private readonly RasedDbContext _context;
        private readonly UserManager<RasedUser> _userManager;
        // ....


        public IPaymentMethodRepository PaymentMethods { get; private set; }      
        public IAttachmentRepository Attachments { get; private set; }
        public IWalletRepository Wallets { get; private set; }
        public IExpensesRepository Expenses { get; private set; }
        public IExpenseTemplateRepository ExpenseTemplates { get; private set; }
        public IAutomationRuleRepository AutomationRules { get; private set; }
        public IIncomeRepository   Income   { get; private set; }
        public IBudgetRepository   Budget   { get; private set; }



        // All System IRepositoryies to be instantiated by the constructor
     
        public ISavingRepository SavingRepository { get; private set; }

        public IRepository<Goal, int> GoalRepository { get; private set; }


        // Constructor to inject all Repositoryies
        public UnitOfWork(RasedDbContext context, UserManager<RasedUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            Wallets = new WalletRepository(_context, _userManager);
            Expenses = new ExpenseRepository(_context);
            ExpenseTemplates = new ExpenseTemplateRepository(_context);
            AutomationRules = new AutomationRuleRepository(_context);
            Income = new IncomeRepository(_context);
            Budget = new BudgetRepository(_context);
            SavingRepository = new SavingRepository(context);
            GoalRepository = new Repository<Goal, int>(context);
            Attachments = new AttachmentRepository(_context);
            PaymentMethods = new PaymentMethodRepository(_context);
        }




        // Save All System Changes and return the number of affected rows
        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose the context to free memory space
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
