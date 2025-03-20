using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/user/wallet/ExpensesTemplate")]
[Authorize]
public class ExpensesTemplateController : ControllerBase
{
    private readonly IExpenseTemplateService _expenseService; 

    public ExpensesTemplateController(IExpenseTemplateService expenseService)
    {
        _expenseService = expenseService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetUserExpenseTemplatesByWalletId(
        [FromQuery] int walletId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "name=rent,amount>100,categoryName=household"
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetUserExpenseTemplatesByWalletId(walletId, filters, pageNumber, pageSize, isShared);
        
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{expenseTemplateId}")]
    public async Task<IActionResult> GetUserExpenseTemplate(
        [FromRoute] int walletId,
        [FromRoute] int expenseTemplateId,
        [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.GetUserExpenseTemplate(walletId, expenseTemplateId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> AddUserExpenseTemplate(
        [FromBody] AddExpenseTemplateDto newExpenseTemplate)
    {
        var response = await _expenseService.AddUserExpenseTemplate(newExpenseTemplate);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{expenseTemplateId}")]
    public async Task<IActionResult> UpdateUserExpenseTemplate(
        [FromRoute] int walletId,
        [FromRoute] int expenseTemplateId,
        [FromBody] UpdateExpenseTemplateDto updateExpenseTemplateDto,
        [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.UpdateUserExpenseTemplate(walletId, expenseTemplateId, updateExpenseTemplateDto, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("{expenseTemplateId}")]
    public async Task<IActionResult> DeleteUserExpenseTemplate(
        [FromRoute] int expenseTemplateId)
    {
        var response = await _expenseService.DeleteUserExpenseTemplate(expenseTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("count")]
    public async Task<IActionResult> CountExpensesTemplate(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.CountExpensesTemplate(walletId, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("totals")]
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
    public async Task<IActionResult> GetAllExpensesTemplatesForAdmin(
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<ExpenseTemplate>(filter);
        var response = await _expenseService.GetAllExpensesTemplatesForAdmin(isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }
}