using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("/api/Expense")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService; 
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    
    [HttpGet("getAllExpenses{walletId}")]
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

    
    [HttpGet("getExpenseById/{expenseId}")]
    public async Task<IActionResult> GetUserExpense(int expenseId)
    {
        var response = await _expenseService.GetUserExpense(expenseId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 3. Add User Expense
    [HttpPost("Add")]
    public async Task<IActionResult> AddUserExpense([FromBody] AddExpenseDto newExpenseDto)
    {
        var response = await _expenseService.AddUserExpense(newExpenseDto);
        if (response.Succeeded)
            return CreatedAtAction(nameof(GetUserExpense), new { walletId = newExpenseDto.WalletId ?? newExpenseDto.SharedWalletId, expenseId = ((Expense)response.Data).ExpenseId }, response);
        return StatusCode((int)response.StatusCode, response);
    }

    // 4. Update User Expense
    [HttpPut("Update/{expenseId}")]
    public async Task<IActionResult> UpdateUserExpense(int expenseId, [FromBody] UpdateExpenseDto updateExpenseDto)
    {
        var response = await _expenseService.UpdateUserExpense(expenseId, updateExpenseDto);
        return StatusCode((int)response.StatusCode, response);
    }

    // 5. Delete User Expense
    [HttpDelete("Delete/{expenseId}")]
    public async Task<IActionResult> DeleteUserExpense(int expenseId)
    {
        var response = await _expenseService.DeleteUserExpense(expenseId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 6. Calculate Total Expenses Amount
    [HttpGet("get-wallet-total-expenses-amount/{walletId}")]
    public async Task<IActionResult> CalculateTotalExpensesAmount(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025")
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 7. Calculate Total Expenses for Last Week
    [HttpGet("get-wallet-total-expenses-amount-for-last-week/{walletId}")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastWeek(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastWeek(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 8. Calculate Total Expenses for Last Month
    [HttpGet("get-wallet-total-expenses-amount-for-last-month/{walletId}")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastMonth(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastMonth(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 9. Calculate Total Expenses for Last Year
    [HttpGet("get-wallet-total-expenses-amount-for-last-year/{walletId}")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastYear(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForLastYear(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 10. Calculate Total Expenses for Specific Period
    [HttpGet("get-wallet-total-expenses-amount-for-specific-period/{walletId}")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForSpecificPeriod(
        int walletId, 
        [FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.CalculateTotalExpensesAmountForSpecificPeriod(walletId, startDateTime, endDateTime, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 11. Get All Expenses for Admin
    [HttpGet("admin/expenses")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAllExpensesForAdmin(
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Expense>(filter);
        var response = await _expenseService.GetAllExpensesForAdmin(filters, null, 0, 0);
        return StatusCode((int)response.StatusCode, response);
    }

    
    
    // get user Expenses
    // get user Specific Expense
    // Add - Update - Delete
    
    // filter collection   :
    // name : amount : date : amount Range : categoryName - sub cat :
    // payment Method : Budget ID : IsAutomation
    
    // Calculated : Total Expenses : Total + date + specific Date : last week : last Month
    // : last Year : prev + categorized : prev + Budgeted : prev + Method : Prev + IsAutomated : 
    // Composite filters
    
    //  for admin : prev + get all Expenses
}

  

    

    
