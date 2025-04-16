using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("api/expenses")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService; 
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    
    [HttpGet("get-all-expenses/{walletId}")]
    public async Task<IActionResult> GetUserExpensesByWalletId(
        int walletId, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025")
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        if (filters.Length == 0) filters = null;
        var response = await _expenseService.GetUserExpensesByWalletId(walletId, filters, 0,0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{expenseId}")]
    public async Task<IActionResult> GetById(int expenseId)
    {
        var response = await _expenseService.GetUserExpense(expenseId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddExpenseDto newExpenseDto)
    {
        var response = await _expenseService.AddUserExpense(newExpenseDto);
        if (response.Succeeded)
            return CreatedAtAction(nameof(GetById), new { walletId = newExpenseDto.WalletId ?? newExpenseDto.SharedWalletId, expenseId = ((Expense)response.Data).ExpenseId }, response);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpPut("{expenseId}")]
    public async Task<IActionResult> Update(int expenseId, [FromBody] UpdateExpenseDto updateExpenseDto)
    {
        var response = await _expenseService.UpdateUserExpense(expenseId, updateExpenseDto);
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;

        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpDelete("{expenseId}")]
    public async Task<IActionResult> Delete(int expenseId)
    {
        var response = await _expenseService.DeleteUserExpense(expenseId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/total-amount")]
    public async Task<IActionResult> CalculateTotalExpensesAmount(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/weekly-expenses")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastWeek(
        [FromRoute] int walletId, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastWeek(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    
    [HttpGet("wallets/{walletId}/statistics/monthly-expenses")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastMonth([FromRoute] int walletId, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastMonth(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/yearly-expenses")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastYear([FromRoute] int walletId, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastYear(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/period-summary")]
    public async Task<IActionResult> GetExpensesTotalForPeriod(
        [FromRoute] int walletId,
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForSpecificPeriod(walletId, startDate, endDate, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAllExpenses([FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.GetAllExpensesForAdmin(filters, null, 0, 0);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
}