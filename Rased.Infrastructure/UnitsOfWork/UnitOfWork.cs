using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Utility;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;
using Microsoft.AspNetCore.Identity;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Wallets;
using Rased.Infrastructure.Repositoryies.Categories;
using Rased.Infrastructure.Repositoryies.SubCategories;
using Rased.Infrastructure.Repositoryies.Goals;
using Rased.Infrastructure.Models.Transfer;
using Rased.Infrastructure.Repositoryies.SharedWallets;
using Rased.Infrastructure.Repositoryies.Friendships;

namespace Rased.Infrastructure.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // All IRepositoryies to be injected 
        private readonly RasedDbContext _context;
        private readonly UserManager<RasedUser> _userManager;
        // ....
        
        public IWalletRepository Wallets { get; private set; }
        public ISharedWalletRepository SharedWallets { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ISubCategoryRepository SubCategories { get; private set; }
        public IFriendshipRepository Friendships { get; private set; }

        public IPaymentMethodRepository PaymentMethods { get; private set; }      
        public IAttachmentRepository Attachments { get; private set; }

        public IExpensesRepository Expenses { get; private set; }
        public IExpenseTemplateRepository ExpenseTemplates { get; private set; }
        public IAutomationRuleRepository AutomationRules { get; private set; }
        public IIncomeRepository   Income   { get; private set; }
        public IBudgetRepository   Budget   { get; private set; }
        public IIncomeTemplateRepository IncomeTemplate { get; }
        public IStaticIncomeSourceTypeDataRepository StaticIncomeSourceTypeData { get; }


        // All System IRepositoryies to be instantiated by the constructor
        public ISavingRepository SavingRepository { get; private set; }

        public IGoalRepository GoalRepository { get; private set; }

        public IGoalTransactionRepository GoalTransactionRepository { get; private set; }

        public IRepository<Transaction, int> Transactions { get; private set; }

        public IRepository<ExpenseTransactionRecord, int> ExpenseTransactionRecords { get; private set; }

        public IRepository<PersonalIncomeTrasactionRecord, int> PersonalIncomeTrasactionRecords { get; private set; }

        public IRepository<SharedWalletIncomeTransaction, int> SharedWalletIncomeTransactions { get; private set; }

        public IRepository<StaticReceiverTypeData, int> StaticReceiverTypes { get; private set; }

        public IRepository<StaticTransactionStatusData, int> StaticTransactionStatus { get; private set; }

        public IRepository<TransactionApproval, int> TransactionApprovals { get; private set; }

        public IRepository<TransactionRejection, int> TransactionRejections { get; private set; }


        // Constructor to inject all Repositoryies
        public UnitOfWork(RasedDbContext context, UserManager<RasedUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            Wallets = new WalletRepository(_context, _userManager);
            SharedWallets = new SharedWalletRepository(_context, _userManager);
            Categories = new CategoryRepository(_context);
            SubCategories = new SubCategoryRepository(_context);
            Friendships = new FriendshipRepository(_context, _userManager);
            Expenses = new ExpenseRepository(_context);
            ExpenseTemplates = new ExpenseTemplateRepository(_context);
            AutomationRules = new AutomationRuleRepository(_context);
            Income = new IncomeRepository(_context);
            Budget = new BudgetRepository(_context);
            SavingRepository = new SavingRepository(context);
            GoalRepository = new GoalRepository(context);
            GoalTransactionRepository = new GoalTransactionRepository(context);
            Transactions = new Repository<Transaction, int>(context);
            ExpenseTransactionRecords = new Repository<ExpenseTransactionRecord, int>(context);
            PersonalIncomeTrasactionRecords = new Repository<PersonalIncomeTrasactionRecord,  int>(context);
            SharedWalletIncomeTransactions = new Repository<SharedWalletIncomeTransaction, int>(context);
            StaticReceiverTypes = new Repository<StaticReceiverTypeData, int>(context);
            StaticTransactionStatus = new Repository<StaticTransactionStatusData , int> (context);
            TransactionApprovals = new Repository<TransactionApproval, int>(context);
            TransactionRejections = new Repository<TransactionRejection, int>(context);
            //GoalRepository = new Repository<Goal, int>(context);
            Attachments = new AttachmentRepository(_context);
            PaymentMethods = new PaymentMethodRepository(_context);
            IncomeTemplate = new IncomeTemplateRepository(_context);
            StaticIncomeSourceTypeData = new StaticIncomeSourceTypeDataRepository(_context);
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
