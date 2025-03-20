using System.Linq.Expressions;
using System.Net;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Business.Services.ExpenseService;

public class PaymentMethodsDataService : IPaymentMethodsDataService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public PaymentMethodsDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
    }
    public async Task<ApiResponse<object>> GetAllPayments(Expression<Func<StaticPaymentMethodsData, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10)
    {
        var res =  _unitOfWork.PaymentMethods.GetAllAsync(filter);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> GetPaymentMethodById(int paymentId)
    {
        if (1 > paymentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.PaymentMethods.GetByIdAsync(paymentId);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> AddPaymentMethod(PaymentMethodsDto paymentMethod)
    {
        if (!IsValidPaymentMethod(paymentMethod, out var errorMessage))
        {
            return _response.Response(false, paymentMethod, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var newPaymentMethod = PrepareAddPaymentMethod(paymentMethod);

        try
        {
            await _unitOfWork.PaymentMethods.AddAsync(newPaymentMethod);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, newPaymentMethod, $"Success add Payment Method with id: {newPaymentMethod.Id}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, paymentMethod, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
    }

    public async Task<ApiResponse<object>> UpdatePaymentMethod(int paymentId, PaymentMethodsDto update)
    {
        if (!IsValidPaymentMethod(update, out var errorMessage) || paymentId < 0)
        {
            return _response.Response(false, update, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        var res =  _unitOfWork.PaymentMethods.GetByIdAsync(paymentId);
        if (res is null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        

        try
        {
            var updatePaymentMethod = PrepareUpdatePaymentMethod(paymentId, update);
            
            _unitOfWork.PaymentMethods.Update(updatePaymentMethod);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, update, $"Success Update Payment Method with id: {updatePaymentMethod.Id}", $"",
                HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, update, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> DeletePaymentMethod(int paymentId)
    {
        if (1 > paymentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.PaymentMethods.RemoveById(paymentId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }

    private bool IsValidPaymentMethod(PaymentMethodsDto dto, out string errorMessages)
    {
        errorMessages = String.Empty;
        if (dto.Name?.Length > 20 )
        {
            errorMessages = " Name must less Than 20 Chars";
            return false;
        }
        return true;
    }

    private StaticPaymentMethodsData PrepareAddPaymentMethod(PaymentMethodsDto dto)
    {
        return new StaticPaymentMethodsData()
        {
            Name = dto.Name
        };
    }
    
    private StaticPaymentMethodsData PrepareUpdatePaymentMethod(int paymentId, PaymentMethodsDto dto)
    {
        return new StaticPaymentMethodsData()
        {
            Name = dto.Name,
            Id = paymentId
        };
    }
}