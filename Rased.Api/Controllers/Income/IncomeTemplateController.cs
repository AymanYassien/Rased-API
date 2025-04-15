using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Income;

[ApiController]
[Route("/api/IncomeTemplate")]
[Authorize]
public class IncomeTemplateController : Controller
{
    private readonly IIncomeTemplateService _incomeTemplateService; 

    public IncomeTemplateController(IIncomeTemplateService incomeTemplateService)
    {
        _incomeTemplateService = incomeTemplateService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserIncomeTemplatesByWalletId(
        [FromQuery] int walletId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) // e.g., "name=rent,amount>100,categoryName=household"
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.GetUserIncomeTemplatesByWalletId(walletId, filters, pageNumber, pageSize, isShared);
        
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{incomeTemplateId}")]
    public async Task<IActionResult> GetUserIncomeTemplate(
        [FromRoute] int walletId,
        [FromRoute] int expenseTemplateId,
        [FromQuery] bool isShared = false)
    {
        var response = await _incomeTemplateService.GetUserIncomeTemplate(walletId, expenseTemplateId, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> AddUserIncomeTemplate(
        [FromBody] AddIncomeTemplateDto  newIncomeTemplate)
    {
        var response = await _incomeTemplateService.AddUserIncomeTemplate(newIncomeTemplate);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPut("{incomeTemplateId}")]
    public async Task<IActionResult> UpdateUserIncomeTemplate(
        [FromRoute] int walletId,
        [FromRoute] int incomeTemplateId,
        [FromBody] UpdateIncomeTemplateDto updateIncomeTemplateDto,
        [FromQuery] bool isShared = false)
    {
        var response = await _incomeTemplateService.UpdateUserIncomeTemplate(walletId, incomeTemplateId, updateIncomeTemplateDto, isShared);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("{incomeTemplateId}")]
    public async Task<IActionResult> DeleteUserIncomeTemplate(
        [FromRoute] int incomeTemplateId)
    {
        var response = await _incomeTemplateService.DeleteUserIncomeTemplate(incomeTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("count")]
    public async Task<IActionResult> CountIncomeTemplate(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.CountIncomeTemplate(walletId, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("totals")]
    public async Task<IActionResult> CalculateTotalIncomesTemplateAmount(
        [FromQuery] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.CalculateTotalIncomeTemplateAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: Admin - Get all Incomes templates
    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllIncomesTemplatesForAdmin(
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.GetAllIncomeTemplatesForAdmin(isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }


}