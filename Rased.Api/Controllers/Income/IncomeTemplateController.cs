using System.Net;
using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Api.Controllers.Helper;
using Rased.Infrastructure;

namespace Rased.Api.Controllers.Income;

[ApiController]
[Route("api/income-templates")]
[Authorize]
public class IncomeTemplateController : Controller
{
    private readonly IIncomeTemplateService _incomeTemplateService; 

    public IncomeTemplateController(IIncomeTemplateService incomeTemplateService)
    {
        _incomeTemplateService = incomeTemplateService;
    }
    
    [HttpGet("get-total-income-template-by-WalletId/{walletId}")]
    public async Task<IActionResult> GetTotalByWalletId([FromRoute] int walletId, [FromQuery] bool isShared = false, [FromQuery] string filter = null) // e.g., "name=rent,amount>100,categoryName=household"
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.GetUserIncomeTemplatesByWalletId(walletId, filters, 0, 0, isShared);
        
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("{incomeTemplateId}")]
    public async Task<IActionResult> GetById([FromRoute] int incomeTemplateId )
    {
        var response = await _incomeTemplateService.GetTemplateById(incomeTemplateId );
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddIncomeTemplateDto  newIncomeTemplate)
    {
        var response = await _incomeTemplateService.AddUserIncomeTemplate(newIncomeTemplate);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPut("{incomeTemplateId}")]
    public async Task<IActionResult> Update( [FromRoute] int incomeTemplateId, [FromBody] UpdateIncomeTemplateDto updateIncomeTemplateDto)
    {
        var response = await _incomeTemplateService.UpdateUserIncomeTemplate( incomeTemplateId, updateIncomeTemplateDto);
        
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;
        
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpDelete("{incomeTemplateId}")]
    public async Task<IActionResult> Delete([FromRoute] int incomeTemplateId)
    {
        var response = await _incomeTemplateService.DeleteUserIncomeTemplate(incomeTemplateId);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("wallets/{walletId}/count")]
    public async Task<IActionResult> GetWalletIncomeTemplatesCount(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.CountIncomeTemplate(walletId, filters, isShared);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("wallets/{walletId}/total")]
    public async Task<IActionResult> GetWalletIncomeTemplateTotal(
        [FromRoute] int walletId,
        [FromQuery] bool isShared = false,
        [FromQuery] string filter = null) 
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.CalculateTotalIncomeTemplateAmount(walletId, isShared, filters);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: Admin - Get all Incomes templates
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllTemplate([FromQuery] string filter = null)
    {
        var filters = ExpressionBuilder.ParseFilter<IncomeTemplate>(filter);
        var response = await _incomeTemplateService.GetAllIncomeTemplatesForAdmin(filters);
        return StatusCode((int)response.StatusCode, response);
    }


}