using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Expenses;

[ApiController]
[Route("/api/Expense")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService; 
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    
    [HttpGet("{walletId}")]
    public async Task<IActionResult> GetUserExpensesByWalletId(
        int walletId, 
        [FromQuery] int pageNumber = 0, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.GetUserExpensesByWalletId(walletId, null, pageNumber, pageSize, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("expense/{expenseId}")]
    public async Task<IActionResult> GetUserExpense(int walletId, int expenseId, [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.GetUserExpense(walletId, expenseId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 3. Add User Expense
    [HttpPost("expense")]
    public async Task<IActionResult> AddUserExpense([FromBody] AddExpenseDto newExpenseDto)
    {
        var response = await _expenseService.AddUserExpense(newExpenseDto);
        if (response.Succeeded)
            return CreatedAtAction(nameof(GetUserExpense), new { walletId = newExpenseDto.WalletId ?? newExpenseDto.SharedWalletId, expenseId = ((Expense)response.Data).ExpenseId }, response);
        return StatusCode((int)response.StatusCode, response);
    }

    // 4. Update User Expense
    [HttpPut("expense/{expenseId}")]
    public async Task<IActionResult> UpdateUserExpense(int expenseId, [FromBody] UpdateExpenseDto updateExpenseDto)
    {
        var response = await _expenseService.UpdateUserExpense(expenseId, updateExpenseDto);
        return StatusCode((int)response.StatusCode, response);
    }

    // 5. Delete User Expense
    [HttpDelete("expense/{expenseId}")]
    public async Task<IActionResult> DeleteUserExpense(int expenseId)
    {
        var response = await _expenseService.DeleteUserExpense(expenseId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 6. Calculate Total Expenses Amount
    [HttpGet("wallet/{walletId}/total")]
    public async Task<IActionResult> CalculateTotalExpensesAmount(int walletId, [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.CalculateTotalExpensesAmount(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 7. Calculate Total Expenses for Last Week
    [HttpGet("wallet/{walletId}/total/last-week")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastWeek(int walletId, [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.CalculateTotalExpensesAmountForLastWeek(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 8. Calculate Total Expenses for Last Month
    [HttpGet("wallet/{walletId}/total/last-month")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastMonth(int walletId, [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.CalculateTotalExpensesAmountForLastMonth(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 9. Calculate Total Expenses for Last Year
    [HttpGet("wallet/{walletId}/total/last-year")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForLastYear(int walletId, [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.CalculateTotalExpensesAmountForLastYear(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 10. Calculate Total Expenses for Specific Period
    [HttpGet("wallet/{walletId}/total/period")]
    public async Task<IActionResult> CalculateTotalExpensesAmountForSpecificPeriod(
        int walletId, 
        [FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime, 
        [FromQuery] bool isShared = false)
    {
        var response = await _expenseService.CalculateTotalExpensesAmountForSpecificPeriod(walletId, startDateTime, endDateTime, null, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 11. Get All Expenses for Admin
    [HttpGet("admin/expenses")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAllExpensesForAdmin(
        [FromQuery] int pageNumber = 0, 
        [FromQuery] int pageSize = 10)
    {
        var response = await _expenseService.GetAllExpensesForAdmin(null, null, pageNumber, pageSize);
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

  

    

    
