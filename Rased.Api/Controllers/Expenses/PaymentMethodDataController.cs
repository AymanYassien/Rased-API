using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/payment-methods")]
[Authorize]
public class PaymentMethodDataController : ControllerBase
{
    private readonly IPaymentMethodsDataService _paymentMethodService;

    public PaymentMethodDataController(IPaymentMethodsDataService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string filter = null) // e.g., "name=visa"
    {
        var filters = ExpressionBuilder.ParseFilter<StaticPaymentMethodsData>(filter);
        var response = await _paymentMethodService.GetAllPayments(filters, 0, 0);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetById([FromRoute] int paymentId)
    {
        var response = await _paymentMethodService.GetPaymentMethodById(paymentId);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentMethodsDto paymentMethod)
    { 
        var response = await _paymentMethodService.AddPaymentMethod(paymentMethod);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{paymentId}")]
    public async Task<IActionResult> Update([FromRoute] int paymentId, [FromBody] PaymentMethodsDto update)
    {
        var response = await _paymentMethodService.UpdatePaymentMethod(paymentId, update);
        
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;
        
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> Delete([FromRoute] int paymentId)
    {
        var response = await _paymentMethodService.DeletePaymentMethod(paymentId);
        return StatusCode((int)response.StatusCode, response);
    }
    
}