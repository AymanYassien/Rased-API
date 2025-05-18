using System.Linq.Expressions;
using System.Net;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Business.Services.ExpenseService;

public class ExpenseService : IExpenseService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public ExpenseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
         
    }

    public  async Task<ApiResponse<object>> GetUserExpensesByWalletId(int walletId, Expression<Func<Expense, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        IQueryable<Expense> res = await _unitOfWork.Expenses.GetUserExpensesByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if ( !res.Any() )
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable < ExpenseDto > newResult = MapToExpenseDto(res);
        
        return  _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

          
    }

    public async Task<ApiResponse<object>> GetUserExpense(int expenseId)
    {
        if (1 > expenseId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        var res = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
        //GetUserExpenseAsync(walletId, ExpenseId, isShared);
        
        if (res == null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = _MapToExpenseDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

       
    }

    public async Task<ApiResponse<object>> AddUserExpense(AddExpenseDto newExpenseDto)
    {
        if (!IsExpenseDtoValid(newExpenseDto, out var errorMessage))
        {
            return _response.Response(false, newExpenseDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var expense = _MapToExpenseDtoFromAdd(newExpenseDto);
        bool isSuccessUpdateRelatedBudget = true;
        bool isSuccessUpdateTotalBalance = true;
        
        try
        {
            await _unitOfWork.Expenses.AddAsync(expense);
            
            if (expense.RelatedBudgetId is not null)
                isSuccessUpdateRelatedBudget = await UpdateRelatedBudget(expense.RelatedBudgetId, expense.Amount );

            if (newExpenseDto.WalletId is not null)
                isSuccessUpdateTotalBalance = await UpdateTotalAmount(newExpenseDto.WalletId, newExpenseDto.Amount * -1);
            else
                isSuccessUpdateTotalBalance = await UpdateTotalAmount_shared(newExpenseDto.SharedWalletId, newExpenseDto.Amount * -1);
            
            
            if (isSuccessUpdateRelatedBudget == false)
            {
                return _response.Response(false, newExpenseDto, "", $"Failed to Update Related Budget, Error Messages : {errorMessage}",
                    HttpStatusCode.InternalServerError);
            }
            
            if (isSuccessUpdateTotalBalance == false)
            {
                return _response.Response(false, newExpenseDto, "", $"Failed to Update Total Balance, Error Messages : {errorMessage}",
                    HttpStatusCode.InternalServerError);
            }
            
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newExpenseDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, expense, $"Success add Expense with id: {expense.ExpenseId}", $"",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<object>> UpdateUserExpense(int expenseId, UpdateExpenseDto updateExpenseDto)
    {
        if (!IsExpenseDtoValid(updateExpenseDto, out var errorMessage))
        {
            return _response.Response(false, updateExpenseDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        if (1 > expenseId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var expense = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
        
        if (expense == null)
            return _response.Response(false, null, "", $"Not Found Expense with id {expenseId}",  HttpStatusCode.NotFound);

        
        bool isSuccessUpdateRelatedBudget = true;
        bool isSuccessUpdateTotalBalance = true;
        try
        {
            
            if (expense.RelatedBudgetId is not null && updateExpenseDto.Amount != expense.Amount)
            {
                decimal spentAmount = (updateExpenseDto.Amount - expense.Amount) ;
                isSuccessUpdateRelatedBudget = await UpdateRelatedBudget(expense.RelatedBudgetId, spentAmount );
                
                
                if(updateExpenseDto.Amount > expense.Amount)
                    if (expense.WalletId is not null)
                        isSuccessUpdateTotalBalance = await UpdateTotalAmount(expense.WalletId, expense.Amount * -1);
                    else
                        isSuccessUpdateTotalBalance = await UpdateTotalAmount_shared(expense.SharedWalletId, expense.Amount * -1);
                else
                if (expense.WalletId is not null)
                    isSuccessUpdateTotalBalance = await UpdateTotalAmount(expense.WalletId, expense.Amount );
                else
                    isSuccessUpdateTotalBalance = await UpdateTotalAmount_shared(expense.SharedWalletId, expense.Amount);

            }
            
            _MapToExpenseDtoFromUpdate(updateExpenseDto, expense);
            
            
            _unitOfWork.Expenses.Update(expense);
            
            if (isSuccessUpdateRelatedBudget == false)
            {
                return _response.Response(false, updateExpenseDto, "", $"Failed to Update Related Budget, Error Messages : {errorMessage}",
                    HttpStatusCode.InternalServerError);
            }
            
            if (isSuccessUpdateTotalBalance == false)
            {
                return _response.Response(false, expense, "", $"Failed to Update Total Balance, Error Messages : {errorMessage}",
                    HttpStatusCode.InternalServerError);
            }
            
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateExpenseDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, updateExpenseDto, $"Success Update Expense with id: {expense.ExpenseId}", $"",
            HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> DeleteUserExpense(int expenseId)
    {
        if (1 > expenseId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        Expense expense = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
        bool isSuccessUpdateRelatedBudget = true;
        bool isSuccessUpdateTotalBalance = true;
        
        if (expense?.RelatedBudgetId is not null)
            isSuccessUpdateRelatedBudget = await UpdateRelatedBudget(expense.RelatedBudgetId, expense.Amount  * -1);

        if (expense.WalletId is not null)
            isSuccessUpdateTotalBalance = await UpdateTotalAmount(expense.WalletId, expense.Amount );
        else
            isSuccessUpdateTotalBalance = await UpdateTotalAmount_shared(expense.SharedWalletId, expense.Amount );

        if (isSuccessUpdateRelatedBudget == false)
        {
            return _response.Response(false, expenseId, "", $"Failed to Delete Related Budget",
                HttpStatusCode.InternalServerError);
        }
        
        
        if (isSuccessUpdateTotalBalance == false)
        {
            return _response.Response(false, expense, "", $"Failed to Update Total Balance",
                HttpStatusCode.InternalServerError);
        }
        
        
        var res =  _unitOfWork.Expenses.RemoveById(expenseId);
        await _unitOfWork.CommitChangesAsync();
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmount(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
       
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastWeek(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastWeekAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastMonth(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastMonthAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastYear(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastYearAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>>CalculateTotalExpensesAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime,
        Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForSpecificPeriodAsync(walletId, startDateTime, endDateTime, filter, isShared);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }
    
    public async Task<ApiResponse<object>> GetAllExpensesForAdmin(Expression<Func<Expense, bool>>[]? filter = null, Expression<Func<Expense, object>>[]? includes = null, int pageNumber = 0,
        int pageSize = 10)
    {
        
        IQueryable<Expense> res = await _unitOfWork.Expenses.GetAllAsync(filter, includes, pageNumber, pageSize);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable < ExpenseDto > newResult = MapToExpenseDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);
    }
    
    private ExpenseDto _MapToExpenseDto(Expense expense)
    {
        return new ExpenseDto()
        {
            ExpenseId = expense.ExpenseId,
            WalletId = expense.WalletId,
            SharedWalletId = expense.SharedWalletId,
            Amount = expense.Amount,
            CategoryName = expense.CategoryName,
            SubCategoryId = expense.SubCategoryId,
            Date = expense.Date,
            PaymentMethodId = expense.PaymentMethodId,
            IsAutomated = expense.IsAutomated,
            Description = expense.Description,
            Title = expense.Title, 
            RelatedBudgetId = expense.RelatedBudgetId
        };
    }

    private Expense _MapToExpenseDtoFromUpdate(UpdateExpenseDto updateExpenseDto, Expense expense)
    {
        expense.Amount = updateExpenseDto.Amount;
        expense.CategoryName = updateExpenseDto.CategoryName;
        expense.SubCategoryId = updateExpenseDto.SubCategoryId;
        expense.Date = updateExpenseDto.Date;
        expense.PaymentMethodId = updateExpenseDto.PaymentMethodId;
        expense.IsAutomated = updateExpenseDto.IsAutomated;
        expense.Description = updateExpenseDto.Description;
        expense.Title = updateExpenseDto.Title;
        expense.RelatedBudgetId = updateExpenseDto.RelatedBudgetId;
        return expense;
        // {
        //     // ExpenseId = updateExpenseDto.ExpenseId,
        //     //WalletId = updateExpenseDto.WalletId,
        //     //SharedWalletId = updateExpenseDto.SharedWalletId,
        //     expense.Amount = updateExpenseDto.Amount;
        //     expense.CategoryName = updateExpenseDto.CategoryName;
        //     expense.SubCategoryId = updateExpenseDto.SubCategoryId;
        //     expense.Date = updateExpenseDto.Date;
        //     expense.PaymentMethodId = updateExpenseDto.PaymentMethodId;
        //     expense.IsAutomated = updateExpenseDto.IsAutomated;
        //     expense.Description = updateExpenseDto.Description;
        //     expense.Title = updateExpenseDto.Title;
        //     expense.RelatedBudgetId = updateExpenseDto.RelatedBudgetId;
        // };
    }
    
    private Expense _MapToExpenseDtoFromAdd(AddExpenseDto addExpenseDto)
    {
        return new Expense()
        {
            WalletId = addExpenseDto.WalletId,
            SharedWalletId = addExpenseDto.SharedWalletId,
            Amount = addExpenseDto.Amount,
            CategoryName = addExpenseDto.CategoryName,
            SubCategoryId = addExpenseDto.SubCategoryId,
            Date = addExpenseDto.Date,
            PaymentMethodId = addExpenseDto.PaymentMethodId,
            IsAutomated = false,
            Description = addExpenseDto.Description,
            RelatedBudgetId = addExpenseDto.RelatedBudgetId,
            Title = addExpenseDto.Title
            

        };
    }
    
    private IQueryable<ExpenseDto> MapToExpenseDto(IQueryable<Expense> expenses)
    {
        return expenses.Select(expense => new ExpenseDto
        {
            ExpenseId = expense.ExpenseId,
            WalletId = expense.WalletId,
            SharedWalletId = expense.SharedWalletId,
            Amount = expense.Amount,
            CategoryName = expense.CategoryName,
            SubCategoryId = expense.SubCategoryId,
            Date = expense.Date,
            PaymentMethodId = expense.PaymentMethodId,
            IsAutomated = expense.IsAutomated,
            Description = expense.Description,
            Title = expense.Title
        });
    }

    private bool IsExpenseDtoValid(AddExpenseDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 1. Title: Required, MaxLength(50)
            if (string.IsNullOrEmpty(dto.Title))
            {
                errorMessage = "Title is required.";
                return false;
            }
            if (dto.Title.Length > 50)
            {
                errorMessage = "Title cannot exceed 50 characters.";
                return false;
            }

            // 2. CategoryName: MaxLength(50), optional
            if (dto.CategoryName?.Length > 50)
            {
                errorMessage = "CategoryName cannot exceed 50 characters.";
                return false;
            }

            // 3. Amount: Required, > 0, decimal(8,2)
            if (dto.Amount == default(decimal))
            {
                errorMessage = "Amount is required.";
                return false;
            }
            if (dto.Amount <= 0)
            {
                errorMessage = "Amount must be greater than 0.";
                return false;
            }
            if (dto.Amount != decimal.Round(dto.Amount, 2) || dto.Amount > 999999.99m)
            {
                errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
                return false;
            }

            // 4. Date: Required
            if (dto.Date == default(DateTime))
            {
                errorMessage = "Date is required.";
                return false;
            }

            // 5. Description: MaxLength(200), optional
            if (dto.Description?.Length > 200)
            {
                errorMessage = "Description cannot exceed 200 characters.";
                return false;
            }
            

            // 6. Check Constraint: WalletId XOR SharedWalletId
            if ((dto.WalletId.HasValue && dto.SharedWalletId.HasValue) || 
                (!dto.WalletId.HasValue && !dto.SharedWalletId.HasValue))
            {
                errorMessage = "Exactly one of WalletId or SharedWalletId must be provided.";
                return false;
            }
            

            

            return true;
        }
    
    private bool IsExpenseDtoValid(UpdateExpenseDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 1. Title: Required, MaxLength(50)
            if (string.IsNullOrEmpty(dto.Title))
            {
                errorMessage = "Title is required.";
                return false;
            }
            if (dto.Title.Length > 50)
            {
                errorMessage = "Title cannot exceed 50 characters.";
                return false;
            }

            // 2. CategoryName: MaxLength(50), optional
            if (dto.CategoryName?.Length > 50)
            {
                errorMessage = "CategoryName cannot exceed 50 characters.";
                return false;
            }

            // 3. Amount: Required, > 0, decimal(8,2)
            if (dto.Amount == default(decimal))
            {
                errorMessage = "Amount is required.";
                return false;
            }
            if (dto.Amount <= 0)
            {
                errorMessage = "Amount must be greater than 0.";
                return false;
            }
            if (dto.Amount != decimal.Round(dto.Amount, 2) || dto.Amount > 999999.99m)
            {
                errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
                return false;
            }

            // 4. Date: Required
            if (dto.Date == default(DateTime))
            {
                errorMessage = "Date is required.";
                return false;
            }

            // 5. Description: MaxLength(200), optional
            if (dto.Description?.Length > 200)
            {
                errorMessage = "Description cannot exceed 200 characters.";
                return false;
            }
            

            // 6. Check Constraint: WalletId XOR SharedWalletId
            // if ((dto.WalletId.HasValue && dto.SharedWalletId.HasValue) || 
            //     (!dto.WalletId.HasValue && !dto.SharedWalletId.HasValue))
            // {
            //     errorMessage = "Exactly one of WalletId or SharedWalletId must be provided.";
            //     return false;
            // }
            

            

            return true;
        }

    private async Task<bool> UpdateRelatedBudget(int? budgetId, decimal spentAmount)
    {
        var res = await _unitOfWork.Budget.UpdateBudgetSpentAmountAsync((int)budgetId, spentAmount);
        return res;
    }
    
    private async Task<bool> UpdateTotalAmount(int? walletId, decimal number)
    {
        var res =  await _unitOfWork.Wallets.UpdateTotalBalance((int)walletId, number);
        return res;

    }
    
    private async Task<bool> UpdateTotalAmount_shared(int? sharedWalletId, decimal number)
    {
        var res =  await _unitOfWork.SharedWallets.UpdateTotalBalance((int)sharedWalletId, number);
        return res;

    }
    
    public async Task<int> AddUserExpense_forInternalUsage(AddExpenseDto newExpenseDto)
    {
        if (!IsExpenseDtoValid(newExpenseDto, out var errorMessage))
        {
            return -1;
        }

        var expense = _MapToExpenseDtoFromAdd(newExpenseDto);
        bool isSuccessUpdateRelatedBudget = true;
        bool isSuccessUpdateTotalBalance = true;
        
        try
        {
            await _unitOfWork.Expenses.AddAsync(expense);
            
            if (expense.RelatedBudgetId is not null)
                isSuccessUpdateRelatedBudget = await UpdateRelatedBudget(expense.RelatedBudgetId, expense.Amount );

            if (newExpenseDto.WalletId is not null)
                isSuccessUpdateTotalBalance = await UpdateTotalAmount(newExpenseDto.WalletId, newExpenseDto.Amount * -1);
            else
                isSuccessUpdateTotalBalance = await UpdateTotalAmount_shared(newExpenseDto.SharedWalletId, newExpenseDto.Amount * -1);
            
            
            if (isSuccessUpdateRelatedBudget == false)
            {
                return -1;
            }
            
            if (isSuccessUpdateTotalBalance == false)
            {
                return -1;
            }
            
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return -1;
        }
        
        return expense.ExpenseId;
    }
    
    
    
    // add ex          - delete ex      -  update ex with greater        - update ex with lowest
    // t -= amount     t += amount           t -= amount                    t += amount  
    
    // add in          - delete in      -  update in with greater        - update in with lowest
    // t += amount     t -= amount           t += amount                    t -= amount
    
    // 100  50   => 100 += 50 = 150,   100 += -50 = 50
    
    // into repo => by id ==>    t += amount
    // when call =>  if + => pass amount  | if - => pass times -1
    
    
    
    
    

    


}