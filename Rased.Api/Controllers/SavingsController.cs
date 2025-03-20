using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Business.Services.Savings;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Repositoryies.Savings;

namespace Rased.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ////[Authorize(Roles = $"{RasedRolesConstants.Admin},{RasedRolesConstants.User}")]
    [Authorize]
    public class SavingsController : ControllerBase
    {
        private readonly ISavingService _savingService;

        public SavingsController(ISavingService savingService)
        {
            _savingService = savingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSavingAsync()
        {
            var response = await _savingService.GetAllSavingAsync();
            return response.Succeeded ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSavingByIdAsync(int id)
        {
            var response = await _savingService.GetSavingByIdAsync(id);
            return response.Succeeded ? Ok(response.Data) : NotFound(response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddSavingAsync([FromBody] AddSavingDto addSavingDto)
        {
            var response = await _savingService.AddSavingAsync(addSavingDto);
            return response.Succeeded ? Ok(response.Message) : BadRequest(response.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSavingAsync(int id, [FromBody] UpdateSavingDto updateSavingDto)
        {
            if (id != updateSavingDto.Id)
                return BadRequest(new ApiResponse<string>("ID mismatch."));

            var response = await _savingService.UpdateSavingAsync(updateSavingDto);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSavingAsync(int id)
        {
            var response = await _savingService.DeleteSavingAsync(id);
            return response.Succeeded ? Ok(response.Message) : NotFound(response.Errors);
        }
    }


}






