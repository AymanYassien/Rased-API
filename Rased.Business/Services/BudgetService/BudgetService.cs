using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.ExpenseService;
using Rased.Business.Services.SubCategories;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased_API.Rased.Business.Services.BudgetService;

public class BudgetService : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpenseService _unitOfWorkExpenseService;
    private ISubCategoryService _subCategoryService;
    private ApiResponse<object> _response;
    public BudgetService(IUnitOfWork unitOfWork, IExpenseService unitOfWorkExpenseService, ISubCategoryService subCategoryService)
    {
        _unitOfWork = unitOfWork;
        _unitOfWorkExpenseService = unitOfWorkExpenseService;
        _subCategoryService = subCategoryService;
        _response = new ApiResponse<object>();
    }


    public async Task<ApiResponse<object>> GetBudgetsById(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.GetByIdAsync(budgetId);
            if (res is null)
                return _response.Response(false, null, "", "Not Found",
                    HttpStatusCode.NotFound);

            var mapped = MapToBudgetDto(res);
            return _response.Response(true, mapped, "Success", "", HttpStatusCode.OK);

        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<object>> AddBudgetAsync(AddBudgetDto dto)
    {
        if (!IsAddDtoValid(dto, out var errorMessage))
        {
            return _response.Response(false, dto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            var budget = MapToBudgetFromAdd(dto);
            await _unitOfWork.Budget.AddAsync(budget);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, budget, $"Success add Budget with id: {budget.BudgetId}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, dto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> UpdateBudgetAsync(int budgetId, UpdateBudgetDto dto)
    {
        if (!IsUpdateDtoValid(dto, out var errorMessage))
        {
            return _response.Response(false, dto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            var budget =
                await _unitOfWork.Budget.GetByIdAsync(budgetId);

            if (budget.BudgetId == budgetId)
            {
                
                _unitOfWork.Budget.Update(MapToBudgetFromUpdate(budget, dto));
                await _unitOfWork.CommitChangesAsync();
                
                return _response.Response(true, null, $"Success Update Budget with id: {budget.BudgetId}", $"",
                    HttpStatusCode.NoContent);
            }
            else
                return _response.Response(false, dto, "", $"Bad Request",
                    HttpStatusCode.BadRequest);
            
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, dto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> DeleteBudgetAsync(int id)
    {
        if (1 > id)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.Budget.RemoveById(id);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetBudgetsForAdminAsync(Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10)
    {
        try
        {
            var res = await _unitOfWork.Budget.GetAllAsync(filter, null, pageNumber, pageSize);
            if (!res.Any())
                return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",
                    HttpStatusCode.NotFound);

            return _response.Response(true, res, "Success", "", HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.GetBudgetsByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
            if (!res.Any())
                return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",
                    HttpStatusCode.NotFound);
            
            var mapped = MapToBudgetDto(res);
            return _response.Response(true, mapped, "Success", "", HttpStatusCode.OK);

            return _response.Response(true, res, "Success", "", HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetValidBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.GetValidBudgetsByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
            if (!res.Any())

                return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",

                    HttpStatusCode.NotFound);
            

            var mapped = MapToBudgetDto(res).ToList();
            
            foreach (var budget in mapped)
            {
                var relatedExpenses = await _unitOfWorkExpenseService.GetLast3ExpensesByBudgetId(budget.BudgetId);
                var subcategoryName = await _subCategoryService.GetSubCategoryNameById((int)budget.SubCategoryId);
                var categoryId = await _unitOfWork.Categories.GetCategoryIdByName(budget.CategoryName);
                
                budget.relatedExpenses = relatedExpenses.ToList();
                budget.subCategoryName = subcategoryName;
                budget.CategoryId = categoryId;
            }
            
            
            return _response.Response(true, mapped, "Success", "", HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(int walletId, DateTime startDate, DateTime endDate,
        Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(walletId, startDate, endDate, filter, pageNumber, pageSize, isShared);
            if (!res.Any())
                return _response.Response(false, null, "", "Not Found, or Nothing to Fetch",
                    HttpStatusCode.NotFound);

            var mapped = MapToBudgetDto(res);
            return _response.Response(true, mapped, "Success", "", HttpStatusCode.OK);

        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> CountValidBudgetsByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.CountValidBudgetsByWalletIdAsync(walletId, isShared);
            if (res < 0)
                return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",
                    HttpStatusCode.NotFound);

            return _response.Response(true, res, "Success", "", HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> IsBudgetValidAsync(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.IsBudgetValidAsync(budgetId);
            if (res is false)
                return _response.Response(false, null, "", "Not Found, or Not Valid Budget with this Id", HttpStatusCode.NotFound);
            return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetBudgetAmountAsync(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Budget.GetBudgetAmountAsync(budgetId);
        if (res < 0)
            return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> IsBudgetRolloverAsync(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.IsBudgetRolloverAsync(budgetId);
            // if (res i)
                return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
                //return _response.Response(false, null, "", "Not Found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetBudgetSpentAmountAsync(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Budget.GetBudgetSpentAmountAsync(budgetId);
        if (res < 0)
            return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> UpdateBudgetSpentAmountAsync(int budgetId, decimal newSpent)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        try
        {
            var res = await _unitOfWork.Budget.UpdateBudgetSpentAmountAsync(budgetId, newSpent);
            if (res is false)
                return _response.Response(false, null, "", "Not Found, or Nothing to Update", HttpStatusCode.NotFound);
            return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return _response.Response(false, null, "", $"Internal Server Error : {e.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> GetRemainingAmountAsync(int budgetId)
    {
        if (1 > budgetId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Budget.GetRemainingAmountAsync(budgetId);
        if (res < 0)
            return _response.Response(false, null, "", "لا يوجد ميزانيات لتلك المحفظة!",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
    }
    
    
    public async Task<string> GetBudgetNameById(int id)
    {
        if (1 > id)
            return string.Empty;
        
        var res = await _unitOfWork.Budget.GetByIdAsync(id);
        return res.Name;

    }
    private bool IsAddDtoValid(AddBudgetDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.Name))
        {
            errorMessage = "Title is required.";
            return false;
        }
        if (dto.Name.Length > 50)
        {
            errorMessage = "Title cannot exceed 50 characters.";
            return false;
        }
        
        
        // 4. StartDate: Required
        if (dto.StartDate == default(DateTime))
        {
            errorMessage = "StartDate is required.";
            return false;
        }

        // 5. EndDate: Required
        if (dto.EndDate == default(DateTime))
        {
            errorMessage = "EndDate is required.";
            return false;
        }
    
        if ( dto.StartDate > dto.EndDate )
        {
            errorMessage = "Ensure End Date.";
            return false;
        }

        // 6. DayOfMonth: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 1-31)
        if (dto.DayOfMonth.HasValue && (dto.DayOfMonth < 1 || dto.DayOfMonth > 29))
        {
            errorMessage = "DayOfMonth must be between 1 and 28.";
            return false;
        }

        // 7. DayOfWeek: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 0-6 for Sunday-Saturday)
        if (dto.DayOfWeek.HasValue && (dto.DayOfWeek < 0 || dto.DayOfWeek > 6))
        {
            errorMessage = "DayOfWeek must be between 0 and 7.";
            return false;
        }
        

        if (dto.CategoryName?.Length > 50)
        {
            errorMessage = "CategoryName cannot exceed 50 characters.";
            return false;
        }
        
        if (dto.BudgetAmount == default(decimal))
        {
            errorMessage = "Amount is required.";
            return false;
        }
        if (dto.BudgetAmount <= 0)
        {
            errorMessage = "Amount must be greater than 0.";
            return false;
        }
        if (dto.BudgetAmount != decimal.Round(dto.BudgetAmount, 2) || dto.BudgetAmount > 999999.99m)
        {
            errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
            return false;
        }

        if (dto.SubCategoryId != null && dto.SubCategoryId < 1 )
        {
            errorMessage = "Sub Category Id must > zero";
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
    
    private bool IsUpdateDtoValid(UpdateBudgetDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.Name))
        {
            errorMessage = "Title is required.";
            return false;
        }
        if (dto.Name.Length > 50)
        {
            errorMessage = "Title cannot exceed 50 characters.";
            return false;
        }
        
        
        // 4. StartDate: Required
        if (dto.StartDate == default(DateTime))
        {
            errorMessage = "StartDate is required.";
            return false;
        }

        // 5. EndDate: Required
        if (dto.EndDate == default(DateTime))
        {
            errorMessage = "EndDate is required.";
            return false;
        }
    
        if ( dto.StartDate > dto.EndDate )
        {
            errorMessage = "Ensure End Date.";
            return false;
        }

        // 6. DayOfMonth: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 1-31)
        if (dto.DayOfMonth.HasValue && (dto.DayOfMonth < 1 || dto.DayOfMonth > 29))
        {
            errorMessage = "DayOfMonth must be between 1 and 28.";
            return false;
        }

        // 7. DayOfWeek: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 0-6 for Sunday-Saturday)
        if (dto.DayOfWeek.HasValue && (dto.DayOfWeek < 0 || dto.DayOfWeek > 6))
        {
            errorMessage = "DayOfWeek must be between 0 and 7.";
            return false;
        }
        

        if (dto.CategoryName?.Length > 50)
        {
            errorMessage = "CategoryName cannot exceed 50 characters.";
            return false;
        }
        
        if (dto.BudgetAmount == default(decimal))
        {
            errorMessage = "Amount is required.";
            return false;
        }
        if (dto.BudgetAmount <= 0)
        {
            errorMessage = "Amount must be greater than 0.";
            return false;
        }
        if (dto.BudgetAmount != decimal.Round(dto.BudgetAmount, 2) || dto.BudgetAmount > 999999.99m)
        {
            errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
            return false;
        }

        if (dto.SubCategoryId != null && dto.SubCategoryId < 1 )
        {
            errorMessage = "Sub Category Id must > zero";
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
    
    private IQueryable<validBudgetDto> MapToBudgetDto(IQueryable<Budget> budgets)
    {
        return budgets.Select(budget => new validBudgetDto()
        {
            BudgetId = budget.BudgetId,
            WalletId = budget.WalletId,
            SharedWalletId = budget.SharedWalletId,
            Name = budget.Name,
            CategoryName = budget.CategoryName,
            SubCategoryId = budget.SubCategoryId,
            BudgetAmount = budget.BudgetAmount,
            DayOfWeek = budget.DayOfWeek,
            DayOfMonth = budget.DayOfMonth,
            BudgetTypeId = budget.BudgetId,
            RolloverUnspent = budget.RolloverUnspent,
            RemainingAmount = budget.RemainingAmount,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            SpentAmount = budget.SpentAmount
        });
    }

    private BudgetDto MapToBudgetDto(Budget budget)
    {
        return new BudgetDto()
        {
            BudgetId = budget.BudgetId,
            WalletId = budget.WalletId,
            SharedWalletId = budget.SharedWalletId,
            Name = budget.Name,
            CategoryName = budget.CategoryName,
            SubCategoryId = budget.SubCategoryId,
            BudgetAmount = budget.BudgetAmount,
            DayOfWeek = budget.DayOfWeek,
            DayOfMonth = budget.DayOfMonth,
            BudgetTypeId = budget.BudgetId,
            RolloverUnspent = budget.RolloverUnspent,
            RemainingAmount = budget.RemainingAmount,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            SpentAmount = budget.SpentAmount
        };
    }

    private Budget MapToBudgetFromAdd(AddBudgetDto dto)
    {
        return new Budget()
        {
            WalletId = dto.WalletId,
            SharedWalletId = dto.SharedWalletId,
            Name = dto.Name,
            CategoryName = dto.CategoryName,
            SubCategoryId = dto.SubCategoryId,
            BudgetAmount = dto.BudgetAmount,
            DayOfWeek = dto.DayOfWeek,
            DayOfMonth = dto.DayOfMonth,
            BudgetTypeId = 1,
            RolloverUnspent = dto.RolloverUnspent,
            RemainingAmount = dto.BudgetAmount,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            SpentAmount = 0
        };
    }

    private Budget MapToBudgetFromUpdate(Budget budget, UpdateBudgetDto dto)
    {
        budget.Name = dto.Name;
        budget.CategoryName = dto.CategoryName;
        budget.SubCategoryId = dto.SubCategoryId;
        budget.BudgetAmount = dto.BudgetAmount;
        budget.DayOfWeek = dto.DayOfWeek;
        budget.DayOfMonth = dto.DayOfMonth;
        budget.BudgetTypeId = 1;
        budget.RolloverUnspent = dto.RolloverUnspent;
        budget.RemainingAmount += dto.BudgetAmount - budget.RemainingAmount;
        budget.StartDate = dto.StartDate;
        budget.EndDate = dto.EndDate;
        
        return budget;
    }
    
    public async Task<ApiResponse<object>> GetFinancialStatusAsync(int walletId, bool isShared = false)
    {
        
        if (walletId <= 0)
        {
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        }

        try
        {
            
            var (totalIncome, totalExpenses, expensesOperationsNumber) = await _unitOfWork.Budget.GetFinancialStatusAsync(walletId, isShared);

            // Map to DTO
            var res =  new BudgetStatistics
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                ExpensesOperationsNumber = expensesOperationsNumber
            };
            
            return _response.Response(true, res, "",
                "",  HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return _response.Response(false, null, "",
                $"Internal Server Error: {ex.Message}",  HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<object>> GetFinancialGraphDataAsync(int walletId, bool isShared = false)
    {
        if (walletId <= 0)
        {
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        }

        try
        {
            var data = await _unitOfWork.Budget.GetFinancialGraphDataAsync(walletId, isShared);

            if (data == null) 
                return _response.Response(false, null, "",
                    "Not Found ",  HttpStatusCode.NotFound);
            
                var res = data.Select(d => new FinancialGraphDto
                {
                    Period = d.period,
                    Income = d.income,
                    Expense = d.expense
                }).ToList();
            
            
            return _response.Response(true, res, "",
                "",  HttpStatusCode.OK);
            
        }
        catch (Exception ex)
        {
            return _response.Response(false, null, "",
                $"Internal Server Error: {ex.Message}",  HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<object>> GetBudgetsStatisticsAsync(int walletId, bool isShared = false)
    {
        if (walletId <= 0)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        

        try
        {
            var (total, budgetExpenses) = await _unitOfWork.Budget.GetBudgetsStatisticsAsync(walletId, isShared);
            if (total == 0 && budgetExpenses is null)
                return _response.Response(false, null, "",
                    "Not Found ",  HttpStatusCode.NotFound);
            
            var res = new ExpensesByBudgetDto
            {
                Total = total,
                Budgets = budgetExpenses.Select(be => new BudgetExpenseDto
                {
                    Budget = be.budget,
                    Amount = be.amount
                }).ToList()
            };
            return _response.Response(true, res, "",
                "",  HttpStatusCode.OK);
            
        }
        catch (Exception ex)
        {
            return _response.Response(false, null, "",
                $"Internal Server Error: {ex.Message}",  HttpStatusCode.InternalServerError);
        }
    }
}