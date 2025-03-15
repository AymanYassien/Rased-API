using System.Linq.Expressions;
using System.Net;
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
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        IQueryable<Expense> res = await _unitOfWork.Expenses.GetUserExpensesByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if (res == null)
            _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable < ExpenseDto > newResult = MapToExpenseDto(res);
        
        _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

        return  _response;
    }

    public async Task<ApiResponse<object>> GetUserExpense(int walletId, int ExpenseId, bool isShared = false)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Expenses.GetUserExpenseAsync(walletId, ExpenseId, isShared);
        
        if (res == null)
            _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = _MapToExpenseDto(res);
        
        _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>> AddUserExpense(AddExpenseDto newExpenseDto)
    {
        if (!IsExpenseDtoValid(newExpenseDto, out var errorMessage))
        {
            return _response.Response(false, newExpenseDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var expense = _MapToExpenseDtoFromAdd(newExpenseDto);

        try
        {
            await _unitOfWork.Expenses.AddAsync(expense);
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
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
        
        if (res == null)
            _response.Response(false, null, "", $"Not Found Expense with id {expenseId}",  HttpStatusCode.NotFound);

        
        var expense = _MapToExpenseDtoFromUpdate(updateExpenseDto);

        try
        {
            _unitOfWork.Expenses.Update(expense);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateExpenseDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, expense, $"Success Update Expense with id: {expense.ExpenseId}", $"",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<object>> DeleteUserExpense(int expenseId)
    {
        if (1 > expenseId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.Expenses.RemoveById(expenseId);
        if (res is false)
            _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmount(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountAsync(walletId, isShared, filter);
        if (res < 0)
            _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastWeek(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastWeekAsync(walletId, isShared, filter);
        if (res < 0)
            _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastMonth(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastMonthAsync(walletId, isShared, filter);
        if (res < 0)
            _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastYear(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForLastYearAsync(walletId, isShared, filter);
        if (res < 0)
            _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

        return _response;
    }

    public async Task<ApiResponse<object>>CalculateTotalExpensesAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime,
        Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false)
    {
        if (1 > walletId)
            _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Expenses.CalculateTotalExpensesAmountForSpecificPeriodAsync(walletId, startDateTime, endDateTime, filter, isShared);
        if (res < 0)
            _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

        return _response;
    }
    

    public async Task<ApiResponse<object>> GetAllExpensesForAdmin(Expression<Func<Expense, bool>>[]? filter = null, Expression<Func<Expense, object>>[]? includes = null, int pageNumber = 0,
        int pageSize = 10)
    {
        
        IQueryable<Expense> res = await _unitOfWork.Expenses.GetAllAsync(filter, includes, pageNumber, pageSize);
        
        if (res == null)
            _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable < ExpenseDto > newResult = MapToExpenseDto(res);
        
        _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

        return  _response;
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
            Title = expense.Title
        };
    }

    private Expense _MapToExpenseDtoFromUpdate(UpdateExpenseDto updateExpenseDto)
    {
        return new Expense()
        {
            // ExpenseId = updateExpenseDto.ExpenseId,
            WalletId = updateExpenseDto.WalletId,
            SharedWalletId = updateExpenseDto.SharedWalletId,
            Amount = updateExpenseDto.Amount,
            CategoryName = updateExpenseDto.CategoryName,
            SubCategoryId = updateExpenseDto.SubCategoryId,
            Date = updateExpenseDto.Date,
            PaymentMethodId = updateExpenseDto.PaymentMethodId,
            IsAutomated = updateExpenseDto.IsAutomated,
            Description = updateExpenseDto.Description,
            Title = updateExpenseDto.Title,
            RelatedBudgetId = updateExpenseDto.RelatedBudgetId
        };
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
            if ((dto.WalletId.HasValue && dto.SharedWalletId.HasValue) || 
                (!dto.WalletId.HasValue && !dto.SharedWalletId.HasValue))
            {
                errorMessage = "Exactly one of WalletId or SharedWalletId must be provided.";
                return false;
            }
            

            

            return true;
        }
    
    
}