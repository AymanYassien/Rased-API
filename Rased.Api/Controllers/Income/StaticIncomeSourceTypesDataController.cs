using System.Net;
using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
using Rased.Infrastructure;
using Rased.Api.Controllers.Helper;

namespace Rased.Api.Controllers.Income;

[ApiController]
[Route("/api/IncomeSourceType")]
[Authorize]
public class StaticIncomeSourceTypesDataController : Controller
{
    private readonly IStaticIncomeSourceTypeDataService _staticIncomeSource;

    public StaticIncomeSourceTypesDataController(IStaticIncomeSourceTypeDataService staticIncomeSource)
    {
        _staticIncomeSource = staticIncomeSource;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string filter = null) // e.g., "name=visa"
    {
        var filters = ExpressionBuilder.ParseFilter<StaticIncomeSourceTypeData>(filter);
        var response = await _staticIncomeSource.GetAllIncomeSources(filters, 0, 0);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpGet("{incomeSourceId}")]
    public async Task<IActionResult> GetById([FromRoute] int incomeSourceId)
    {
        var response = await _staticIncomeSource.GetIncomeSourcesById(incomeSourceId);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StaticIncomeSourceTypeDataDto incomeSource)
    {
        var response = await _staticIncomeSource.AddIncomeSources(incomeSource);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{incomeSourceId}")]
    public async Task<IActionResult> Update([FromRoute] int incomeSourceId, [FromBody] StaticIncomeSourceTypeDataDto update)
    {
        var response = await _staticIncomeSource.UpdateIncomeSources(incomeSourceId, update);
        
        if ((int)response.StatusCode == 204) response.StatusCode = HttpStatusCode.OK;
        
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{incomeSourceId}")]
    public async Task<IActionResult> Delete([FromRoute] int incomeSourceId)
    {
        var response = await _staticIncomeSource.DeleteIncomeSources(incomeSourceId);
        return StatusCode((int)response.StatusCode, response);
    }
    
}