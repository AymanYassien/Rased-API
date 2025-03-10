using Rased_API.Rased.Business.Services.BudgetService;
using Microsoft.AspNetCore.Mvc;

namespace Rased.Api.Controllers.Budget;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        try
        {
            await _budgetService.DeleteBudgetAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) // can handle exception Class 
        {
            return NotFound(ex.Message);
        }
    }
}

// Before Start : Points To handle
// _____________________
// Documentation
// Exception Handler
// api Request
// Log 
// Mapping
// Testing
// Versioning
