using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/ExpensesTemplate")]
[Authorize]
public class ExpensesTemplateController : ControllerBase
{
    private readonly IExpenseTemplateService _expenseService; 

    public ExpensesTemplateController(IExpenseTemplateService expenseService)
    {
        _expenseService = expenseService;
    }

    
    [HttpGet("get-user-expenses-template-by-WalletId")]
    public async Task<IActionResult> GetUserExpenseTemplatesByWalletId(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "name=rent,amount>100,categoryName=household"
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetUserExpenseTemplatesByWalletId(walletId, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("get-expense-by-id{expenseTemplateId}")]
    public async Task<IActionResult> GetUserExpenseTemplate([FromRoute] int expenseTemplateId)
    {
        var response = await _expenseService.GetUserExpenseTemplate(expenseTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost("add")]
    public async Task<IActionResult> AddUserExpenseTemplate(
        [FromBody] AddExpenseTemplateDto newExpenseTemplate)
    {
        var response = await _expenseService.AddUserExpenseTemplate(newExpenseTemplate);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("update{expenseTemplateId}")]
    public async Task<IActionResult> UpdateUserExpenseTemplate(
        [FromRoute] int expenseTemplateId, [FromBody] UpdateExpenseTemplateDto updateExpenseTemplateDto)
    {
        var response = await _expenseService.UpdateUserExpenseTemplate( expenseTemplateId, updateExpenseTemplateDto);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("delete{expenseTemplateId}")]
    public async Task<IActionResult> DeleteUserExpenseTemplate(
        [FromRoute] int expenseTemplateId)
    {
        var response = await _expenseService.DeleteUserExpenseTemplate(expenseTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("count-wallet-expenseTemplate")]
    public async Task<IActionResult> CountExpensesTemplate(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.CountExpensesTemplate(walletId, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet-total-amount-for-expense-template")]
    public async Task<IActionResult> CalculateTotalExpensesTemplateAmount(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.CalculateTotalExpensesTemplateAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: Admin - Get all expenses templates
    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllExpensesTemplatesForAdmin([FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetAllExpensesTemplatesForAdmin(filters);
        return StatusCode((int)response.StatusCode, response);
    }
}