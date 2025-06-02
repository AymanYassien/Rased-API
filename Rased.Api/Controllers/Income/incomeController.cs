using System.Net;
using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Income;

[ApiController]
[Route("/api/incomes")]
[Authorize]
public class incomeController : Controller
{
    
    private readonly IIncomeService _incomeService; 
    public incomeController(IIncomeService incomeService )
    {
        _incomeService =  incomeService;
    }
    
    
    [HttpGet("GetAllIncomes/{walletId}")]
    public async Task<IActionResult> GetUserIncomesByWalletId(
        [FromRoute]int walletId, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "Amount > 100, Date = 12-01-2025")
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.GetUserIncomesByWalletId(walletId, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{incomeId}")]
    public async Task<IActionResult> GetById( [FromRoute]int incomeId)
    {
        var response = await _incomeService.GetUserIncome(incomeId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddIncomeDto newIncomeDto)
    {
        var response = await _incomeService.AddUserIncome(newIncomeDto);
        if (response.Succeeded)
            return Ok(response);

        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{incomeId}")]
    public async Task<IActionResult> Update(int incomeId, [FromBody] UpdateIncomeDto updateIncomeDto)
    {
        var response = await _incomeService.UpdateUserIncome(incomeId, updateIncomeDto);
        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }

    
    [HttpDelete("{incomeId}")]
    public async Task<IActionResult> Delete(int incomeId)
    {
        var response = await _incomeService.DeleteUserIncome(incomeId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/total-amount")]
    public async Task<IActionResult> CalculateTotalIncomesAmount(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

   
    [HttpGet("wallets/{walletId}/statistics/weekly-expenses")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastWeek(int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastWeek(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/monthly-expenses")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastMonth([FromRoute]int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastMonth(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/statistics/yearly-expenses")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForLastYear([FromRoute]int walletId, [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForLastYear(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/period-summary")]
    public async Task<IActionResult> CalculateTotalIncomesAmountForSpecificPeriod(
        [FromRoute] int walletId,
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, 
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.CalculateTotalIncomesAmountForSpecificPeriod(walletId, startDate, endDate, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAll([FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Income>(filter);
        var response = await _incomeService.GetAllIncomesForAdmin(filters, null, 0, 0);
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