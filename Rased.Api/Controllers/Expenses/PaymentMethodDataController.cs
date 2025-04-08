using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/user/wallet/payment-methods")]
[Authorize]
public class PaymentMethodDataController : ControllerBase
{
    private readonly IPaymentMethodsDataService _paymentMethodService;

    public PaymentMethodDataController(IPaymentMethodsDataService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAllPayments(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string filter = null) // e.g., "name=visa"
    {
        var filters = ExpressionBuilder.ParseFilter<StaticPaymentMethodsData>(filter);
        var response = await _paymentMethodService.GetAllPayments(filters, pageNumber, pageSize);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentMethodById(
        [FromRoute] int paymentId)
    {
        var response = await _paymentMethodService.GetPaymentMethodById(paymentId);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddPaymentMethod(
        [FromBody] PaymentMethodsDto paymentMethod)
    {
        var response = await _paymentMethodService.AddPaymentMethod(paymentMethod);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdatePaymentMethod(
        [FromRoute] int paymentId,
        [FromBody] PaymentMethodsDto update)
    {
        var response = await _paymentMethodService.UpdatePaymentMethod(paymentId, update);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> DeletePaymentMethod(
        [FromRoute] int paymentId)
    {
        var response = await _paymentMethodService.DeletePaymentMethod(paymentId);
        return StatusCode((int)response.StatusCode, response);
    }
    
}