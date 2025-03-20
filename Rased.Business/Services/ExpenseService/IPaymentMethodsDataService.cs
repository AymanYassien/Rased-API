using System.Linq.Expressions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased.Business.Services.ExpenseService;

public interface IPaymentMethodsDataService
{
    public Task<ApiResponse<object>> GetAllPayments(Expression<Func<StaticPaymentMethodsData, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10);
    public Task<ApiResponse<object>> GetPaymentMethodById(int paymentId);
    public Task<ApiResponse<object>> AddPaymentMethod(PaymentMethodsDto paymentMethod);
    public Task<ApiResponse<object>> UpdatePaymentMethod(int paymentId, PaymentMethodsDto update);
    public Task<ApiResponse<object>> DeletePaymentMethod(int paymentId);
}