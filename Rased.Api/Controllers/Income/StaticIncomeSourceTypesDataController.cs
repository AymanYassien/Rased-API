using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
using Rased.Infrastructure;
using Rased.Api.Controllers.Helper;

namespace Rased.Api.Controllers.Income;

public class StaticIncomeSourceTypesDataController : Controller
{
    private readonly IStaticIncomeSourceTypeDataService _staticIncomeSource;

    public StaticIncomeSourceTypesDataController(IStaticIncomeSourceTypeDataService staticIncomeSource)
    {
        _staticIncomeSource = staticIncomeSource;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllIncomeSources(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string filter = null) // e.g., "name=visa"
    {
        var filters = ExpressionBuilder.ParseFilter<StaticIncomeSourceTypeData>(filter);
        var response = await _staticIncomeSource.GetAllIncomeSources(filters, pageNumber, pageSize);
        return StatusCode((int)response.StatusCode, response);
    }

    
    [HttpGet("{incomeSourceId}")]
    public async Task<IActionResult> GetIncomeSourceTypeById(
        [FromRoute] int incomeSourceId)
    {
        var response = await _staticIncomeSource.GetIncomeSourcesById(incomeSourceId);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddIncomeSourceType(
        [FromBody] StaticIncomeSourceTypeDataDto incomeSource)
    {
        var response = await _staticIncomeSource.AddIncomeSources(incomeSource);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{incomeSourceId}")]
    public async Task<IActionResult> UpdateIncomeSourceType(
        [FromRoute] int incomeSourceId,
        [FromBody] StaticIncomeSourceTypeDataDto update)
    {
        var response = await _staticIncomeSource.UpdateIncomeSources(incomeSourceId, update);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{incomeSourceId}")]
    public async Task<IActionResult> DeleteIncomeSourceType(
        [FromRoute] int incomeSourceId)
    {
        var response = await _staticIncomeSource.DeleteIncomeSources(incomeSourceId);
        return StatusCode((int)response.StatusCode, response);
    }
}