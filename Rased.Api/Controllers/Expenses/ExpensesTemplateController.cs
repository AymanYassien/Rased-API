using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/expense-templates")]
[Authorize]
public class ExpensesTemplateController : ControllerBase
{
    private readonly IExpenseTemplateService _expenseService; 

    public ExpensesTemplateController(IExpenseTemplateService expenseService)
    {
        _expenseService = expenseService;
    }

    
    [HttpGet("get-total-expenses-template-by-WalletId/{walletId}")]
    public async Task<IActionResult> GetTotalByWalletId([FromQuery] int walletId, [FromQuery] bool isShared = false, [FromQuery] string filter = null) // e.g., "name=rent,amount>100,categoryName=household"
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetUserExpenseTemplatesByWalletId(walletId, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{expenseTemplateId}")]
    public async Task<IActionResult> GetUserById([FromRoute] int expenseTemplateId)
    {
        var response = await _expenseService.GetUserExpenseTemplate(expenseTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddExpenseTemplateDto newExpenseTemplate)
    {
        var response = await _expenseService.AddUserExpenseTemplate(newExpenseTemplate);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{expenseTemplateId}")]
    public async Task<IActionResult> Update([FromRoute] int expenseTemplateId, [FromBody] UpdateExpenseTemplateDto updateExpenseTemplateDto)
    {
        var response = await _expenseService.UpdateUserExpenseTemplate( expenseTemplateId, updateExpenseTemplateDto);
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;

        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("{expenseTemplateId}")]
    public async Task<IActionResult> Delete([FromRoute] int expenseTemplateId)
    {
        var response = await _expenseService.DeleteUserExpenseTemplate(expenseTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/count")]
    public async Task<IActionResult> GetWalletTemplateCount(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.CountExpensesTemplate(walletId, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/total")]
    public async Task<IActionResult> GetWalletTemplateTotal(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.CalculateTotalExpensesTemplateAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: Admin - Get all expenses templates
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllTemplate([FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetAllExpensesTemplatesForAdmin(filters);
        return StatusCode((int)response.StatusCode, response);
    }
}