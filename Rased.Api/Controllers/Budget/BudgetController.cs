using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Business.Dtos;


namespace Rased.Api.Controllers.Budget;

[ApiController]
[Route("api/budget")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBudget(
        [FromBody] AddBudgetDto dto)
    {
        var response = await _budgetService.AddBudgetAsync(dto);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpPut("{budgetId}")]
    public async Task<IActionResult> UpdateBudget(
        [FromRoute] int budgetId,
        [FromBody] UpdateBudgetDto dto)
    {
        var response = await _budgetService.UpdateBudgetAsync(budgetId, dto);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpDelete("{budgetId}")]
    public async Task<IActionResult> DeleteBudget(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.DeleteBudgetAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBudgetsForAdmin(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string filter = null) // e.g., "amount>1000"
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsForAdminAsync(filters, pageNumber, pageSize);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}")]
    public async Task<IActionResult> GetBudgetsByWalletId(
        [FromRoute] int walletId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsByWalletIdAsync(walletId, filters, pageNumber, pageSize, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}/valid")]
    public async Task<IActionResult> GetValidBudgetsByWalletId(
        [FromRoute] int walletId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetValidBudgetsByWalletIdAsync(walletId, filters, pageNumber, pageSize, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}/categorized")]
    public async Task<IActionResult> GetBudgetsByWalletIdCategorizedAtSpecificPeriod(
        [FromRoute] int walletId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<Infrastructure.Budget>(filter);
        var response = await _budgetService.GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(walletId, startDate, endDate, filters, pageNumber, pageSize, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallet/{walletId}/count")]
    public async Task<IActionResult> CountValidBudgetsByWalletId(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false)
    {
        var response = await _budgetService.CountValidBudgetsByWalletIdAsync(walletId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/valid")]
    public async Task<IActionResult> IsBudgetValid(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.IsBudgetValidAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/amount")]
    public async Task<IActionResult> GetBudgetAmount(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.GetBudgetAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/rollover")]
    public async Task<IActionResult> IsBudgetRollover(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.IsBudgetRolloverAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/spent")]
    public async Task<IActionResult> GetBudgetSpentAmount(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.GetBudgetSpentAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPut("{budgetId}/spent")]
    public async Task<IActionResult> UpdateBudgetSpentAmount(
        [FromRoute] int budgetId,
        [FromQuery] decimal newSpent)
    {
        var response = await _budgetService.UpdateBudgetSpentAmountAsync(budgetId, newSpent);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{budgetId}/remaining")]
    public async Task<IActionResult> GetRemainingAmount(
        [FromRoute] int budgetId)
    {
        var response = await _budgetService.GetRemainingAmountAsync(budgetId);
        return StatusCode((int)response.StatusCode, response);
    }
}