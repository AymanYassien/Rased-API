using System.Linq.Expressions;
using System.Net;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public class StaticIncomeSourceTypeDataService : IStaticIncomeSourceTypeDataService
{
    
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;

    public StaticIncomeSourceTypeDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
    }
    
    public async Task<ApiResponse<object>> GetAllIncomeSources(Expression<Func<StaticIncomeSourceTypeData, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10)
    {
        var res =  await _unitOfWork.StaticIncomeSourceTypeData.GetAllAsync(filter);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetIncomeSourcesById(int incomeSourceTypeId)
    {
        if (1 > incomeSourceTypeId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  await _unitOfWork.StaticIncomeSourceTypeData.GetByIdAsync(incomeSourceTypeId);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> AddIncomeSources(StaticIncomeSourceTypeDataDto incomeSourceType)
    {
        if (!IsSourceTypeIncomeValid(incomeSourceType, out var errorMessage))
        {
            return _response.Response(false, incomeSourceType, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var newIncomeSource = PrepareAddIncomeSource(incomeSourceType);

        try
        {
            await _unitOfWork.StaticIncomeSourceTypeData.AddAsync(newIncomeSource);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, newIncomeSource, $"Success add Income Source Type with id: {newIncomeSource.Id}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, incomeSourceType, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> UpdateIncomeSources(int incomeSourceTypeId, StaticIncomeSourceTypeDataDto update)
    {
        if (!IsSourceTypeIncomeValid(update, out var errorMessage) || incomeSourceTypeId < 0)
        {
            return _response.Response(false, update, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        var res =  await _unitOfWork.StaticIncomeSourceTypeData.GetByIdAsync(incomeSourceTypeId);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        

        try
        {
            res.Name = update.Name;
            
            _unitOfWork.StaticIncomeSourceTypeData.Update(res);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, update, $"Success Update Income Source Type with id: {update.Id}", $"",
                HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, update, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> DeleteIncomeSources(int incomeSourceTypeId)
    {
        if (1 > incomeSourceTypeId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.StaticIncomeSourceTypeData.RemoveById(incomeSourceTypeId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }
    
    private bool IsSourceTypeIncomeValid(StaticIncomeSourceTypeDataDto dto, out string errorMessages)
    {
        errorMessages = String.Empty;
        if (dto.Name?.Length > 20 )
        {
            errorMessages = " Name must less Than 20 Chars";
            return false;
        }
        return true;
    }

    private StaticIncomeSourceTypeData PrepareAddIncomeSource(StaticIncomeSourceTypeDataDto dto)
    {
        return new StaticIncomeSourceTypeData()
        {
            Name = dto.Name
        };
    }
    
    private StaticIncomeSourceTypeData PrepareUpdateIncomeSource(int paymentId, StaticIncomeSourceTypeDataDto dto)
    {
        return new StaticIncomeSourceTypeData()
        {
            Name = dto.Name,
            Id = paymentId
        };
    }

}