using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Income;

public class incomeController : Controller
{
    
    private readonly IIncomeService _incomeService; 
    public incomeController(IIncomeService incomeService )
    {
        _incomeService =  incomeService;
    }
    
    
    [HttpGet("{walletId}")]
    public async Task<IActionResult> GetUserIncomesByWalletId(
        int walletId, 
        [FromQuery] int pageNumber = 0, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025")
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.GetUserIncomesByWalletId(walletId, filters, pageNumber, pageSize, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("income/{IncomeId}")]
    public async Task<IActionResult> GetUserIncome(int walletId, int incomeId, [FromQuery] bool isShared = false)
    {
        var response = await _incomeService.GetUserIncome(walletId, incomeId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 3. Add User Income
    [HttpPost("income")]
    public async Task<IActionResult> AddUserIncome([FromBody] AddIncomeDto newIncomeDto)
    {
        var response = await _incomeService.AddUserIncome(newIncomeDto);
        if (response.Succeeded)
            return CreatedAtAction(nameof(GetUserIncome), new { walletId = newIncomeDto.WalletId ?? newIncomeDto.SharedWalletId, incomeId = ((Infrastructure.Income)response.Data).IncomeId }, response);
        return StatusCode((int)response.StatusCode, response);
    }

    // 4. Update User Income
    [HttpPut("income/{incomeId}")]
    public async Task<IActionResult> UpdateUserIncome(int incomeId, [FromBody] UpdateIncomeDto updateIncomeDto)
    {
        var response = await _incomeService.UpdateUserIncome(incomeId, updateIncomeDto);
        return StatusCode((int)response.StatusCode, response);
    }

    // 5. Delete User Income
    [HttpDelete("income/{incomeId}}")]
    public async Task<IActionResult> DeleteUserIncome(int incomeId)
    {
        var response = await _incomeService.DeleteUserIncome(incomeId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 6. Calculate Total Incomes Amount
    [HttpGet("wallet/{walletId}/total")]
    public async Task<IActionResult> CalculateTotalIncomesAmount(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025")
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 7. Calculate Total Incomes for Last Week
    [HttpGet("wallet/{walletId}/total/last-week")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastWeek(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastWeek(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 8. Calculate Total Incomes for Last Month
    [HttpGet("wallet/{walletId}/total/last-month")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastMonth(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastMonth(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 9. Calculate Total Incomes for Last Year
    [HttpGet("wallet/{walletId}/total/last-year")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastYear(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastYear(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // 10. Calculate Total Incomes for Specific Period
    [HttpGet("wallet/{walletId}/total/period")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForSpecificPeriod(
        int walletId, 
        [FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForSpecificPeriod(walletId, startDateTime, endDateTime, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    // 11. Get All Incomes for Admin
    [HttpGet("admin/incomes")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAllIncomesForAdmin(
        [FromQuery] int pageNumber = 0, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025"))
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.GetAllIncomesForAdmin(filters, null, pageNumber, pageSize);
        return StatusCode((int)response.StatusCode, response);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // get user Incomes
    // get user Specific Income
    // Add - Update - Delete
    
    // filter collection   :
    // name : amount : date : amount Range : categoryName - sub cat :
    
    // Calculated : Total Incomes : Total + date + specific Date : last week : last Month
    // : last Year : prev + categorized : prev + Budgeted : prev + Method : Prev + IsAutomated : 
    // Composite filters
    
    //  for admin : prev + get all Incomes
    
    
}