using System.Linq.Expressions;
using System.Net;
using Rased_API.Rased.Infrastructure;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public class IncomeService : IIncomeService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public IncomeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
         
    }

    public async Task<ApiResponse<object>> GetUserIncomesByWalletId(int walletId, Expression<Func<Income, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        IQueryable<Income> res = await _unitOfWork.Income.GetUserIncomesByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable < IncomeDto > newResult = MapToIncomeDto(res);
        
        return  _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetUserIncome(int walletId, int IncomeId, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Income.GetUserIncomesAsync(walletId, IncomeId, isShared);
        
        if (res == null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = _MapToIncomeDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);


    }

    public async Task<ApiResponse<object>> AddUserIncome(AddIncomeDto newIncomeDto)
    {
        if (!IsIncomeDtoValid(newIncomeDto, out var errorMessage))
        {
            return _response.Response(false, newIncomeDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var income = _MapToIncomeFromAdd(newIncomeDto);

        try
        {
            await _unitOfWork.Income.AddAsync(income);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newIncomeDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, income, $"Success add Income with id: {income.IncomeId}", $"",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<object>> UpdateUserIncome(int incomeId, UpdateIncomeDto updateIncomeDto)
    {
        if (!IsIncomeDtoValid(updateIncomeDto, out var errorMessage))
        {
            return _response.Response(false, updateIncomeDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        if (1 > incomeId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Income.GetByIdAsync(incomeId);
        
        if (res == null)
            return _response.Response(false, null, "", $"Not Found Income with id {incomeId}",  HttpStatusCode.NotFound);

        
        var income = _MapToIncomeFromUpdate(updateIncomeDto);

        try
        {
            _unitOfWork.Income.Update(income);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateIncomeDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, income, $"Success Update Income with id: {income.IncomeId}", $"",
            HttpStatusCode.NoContent);
    }

    public async Task<ApiResponse<object>> DeleteUserIncome(int incomeId)
    {
        if (1 > incomeId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.Income.RemoveById(incomeId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomesAmount(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Income.CalculateTotalIncomesAmountAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastWeek(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Income.CalculateTotalIncomesAmountForLastWeekAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastMonth(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Income.CalculateTotalIncomesAmountForLastMonthAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastYear(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Income.CalculateTotalIncomesAmountForLastYearAsync(walletId, isShared, filter);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomesAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime,
        Expression<Func<Income, bool>>[]? filter = null, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        decimal res =  await _unitOfWork.Income.CalculateTotalIncomesAmountForSpecificPeriodAsync(walletId, startDateTime, endDateTime, filter, isShared);
        if (res < 0)
            return _response.Response(false, null, "", "Not Found, or Nothing to calculate",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetAllIncomesForAdmin(Expression<Func<Income, bool>>[]? filter = null, Expression<Func<Income, object>>[]? includes = null, int pageNumber = 0,
        int pageSize = 10)
    {
        IQueryable<Income> res = await _unitOfWork.Income.GetAllAsync(filter, includes, pageNumber, pageSize);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable<IncomeDto> newResult = MapToIncomeDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

    }
    private IncomeDto _MapToIncomeDto(Income income)
    {
        return new IncomeDto()
        {
            IncomeId = income.IncomeId,
            WalletId = income.WalletId,
            SharedWalletId = income.SharedWalletId,
            Amount = income.Amount,
            CategoryName = income.CategoryName,
            SubCategoryId = income.SubCategoryId,
            CreatedDate = income.CreatedDate,
            IncomeSourceTypeId = income.IncomeSourceTypeId,
            IsAutomated = income.IsAutomated,
            Description = income.Description,
            Title = income.Title,
            IncomeTemplateId = income.IncomeTemplateId
        };
    }

    private Income _MapToIncomeFromUpdate(UpdateIncomeDto updateIncomeDto)
    {
        return new Income()
        {
            //IncomeId = income.IncomeId,
            WalletId = updateIncomeDto.WalletId,
            SharedWalletId = updateIncomeDto.SharedWalletId,
            Amount = updateIncomeDto.Amount,
            CategoryName = updateIncomeDto.CategoryName,
            SubCategoryId = updateIncomeDto.SubCategoryId,
            CreatedDate = updateIncomeDto.CreatedDate,
            IncomeSourceTypeId = updateIncomeDto.IncomeSourceTypeId,
            IsAutomated = updateIncomeDto.IsAutomated,
            Description = updateIncomeDto.Description,
            Title = updateIncomeDto.Title,
            IncomeTemplateId = updateIncomeDto.IncomeTemplateId
        };
    }
    
    private Income _MapToIncomeFromAdd(AddIncomeDto addIncomeDto)
    {
        return new Income()
        {
            WalletId = addIncomeDto.WalletId,
            SharedWalletId = addIncomeDto.SharedWalletId,
            Amount = addIncomeDto.Amount,
            CategoryName = addIncomeDto.CategoryName,
            SubCategoryId = addIncomeDto.SubCategoryId,
            CreatedDate = addIncomeDto.CreatedDate,
            IncomeSourceTypeId = addIncomeDto.IncomeSourceTypeId,
            IsAutomated = false,
            Description = addIncomeDto.Description,
            Title = addIncomeDto.Title,
            IncomeTemplateId = addIncomeDto.IncomeTemplateId
        };
    }
    
    private IQueryable<IncomeDto> MapToIncomeDto(IQueryable<Income> incomes)
    {
        return incomes.Select(income => new IncomeDto()
        {
            IncomeId = income.IncomeId,
            WalletId = income.WalletId,
            SharedWalletId = income.SharedWalletId,
            Amount = income.Amount,
            CategoryName = income.CategoryName,
            SubCategoryId = income.SubCategoryId,
            CreatedDate = income.CreatedDate,
            IncomeSourceTypeId = income.IncomeSourceTypeId,
            IsAutomated = income.IsAutomated,
            Description = income.Description,
            Title = income.Title,
            IncomeTemplateId = income.IncomeTemplateId
            
        });
    }

    private bool IsIncomeDtoValid(AddIncomeDto dto, out string errorMessage)
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
            if (dto.CreatedDate == default(DateTime))
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
    
    private bool IsIncomeDtoValid(UpdateIncomeDto dto, out string errorMessage)
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
            if (dto.CreatedDate == default(DateTime))
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