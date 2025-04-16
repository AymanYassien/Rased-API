using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;


namespace Rased.Api.Controllers.Budget;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }
    
    [HttpGet("{budgetId}")]
    public async Task<IActionResult> GetById([FromRoute] int budgetId)
    {
        var response = await _budgetService.GetBudgetsById(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddBudgetDto dto)
    {
        var response = await _budgetService.AddBudgetAsync(dto);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpPut("{budgetId}")]
    public async Task<IActionResult> Update([FromRoute] int budgetId, [FromBody] UpdateBudgetDto dto)
    {
        var response = await _budgetService.UpdateBudgetAsync(budgetId, dto);
        
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;
        
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpDelete("{budgetId}")]
    public async Task<IActionResult> Delete([FromRoute] int budgetId)
    {
        var response = await _budgetService.DeleteBudgetAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBudgets([FromQuery] string filter = null) // e.g., "amount>1000"
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsForAdminAsync(filters, 0, 0);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("wallets/{walletId}")]
    public async Task<IActionResult> GetBudgetsByWalletId(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsByWalletIdAsync(walletId, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/valid")]
    public async Task<IActionResult> GetValidBudgets(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetValidBudgetsByWalletIdAsync(walletId, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}/categorized")]
    public async Task<IActionResult> GetBudgetsByWalletIdCategorizedAtSpecificPeriod([FromRoute] int walletId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        [FromQuery] bool isShared = false, [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(walletId, startDate, endDate, filters, 0, 0, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}/count-valid")]
    public async Task<IActionResult> CountValidBudgetsByWalletId([FromRoute] int walletId, [FromQuery] bool isShared = false)
    {
        var response = await _budgetService.CountValidBudgetsByWalletIdAsync(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/is-valid")]
    public async Task<IActionResult> IsBudgetValid([FromRoute] int budgetId)
    {
        var response = await _budgetService.IsBudgetValidAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{budgetId}/amount")]
    public async Task<IActionResult> GetBudgetAmount([FromRoute] int budgetId)
    {
        var response = await _budgetService.GetBudgetAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{budgetId}/rollover")]
    public async Task<IActionResult> IsBudgetRollover([FromRoute] int budgetId)
    {
        var response = await _budgetService.IsBudgetRolloverAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{budgetId}/spent")]
    public async Task<IActionResult> GetBudgetSpentAmount([FromRoute] int budgetId)
    {
        var response = await _budgetService.GetBudgetSpentAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpPut("{budgetId}/spent")]
    public async Task<IActionResult> UpdateBudgetSpentAmount([FromRoute] int budgetId, [FromQuery] decimal newSpent)
    {
        var response = await _budgetService.UpdateBudgetSpentAmountAsync(budgetId, newSpent);
        return StatusCode((int)response.StatusCode, response);
    }
    
    
    [HttpGet("{budgetId}/remaining")]
    public async Task<IActionResult> GetRemainingAmount([FromRoute] int budgetId)
    {
        var response = await _budgetService.GetRemainingAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
}